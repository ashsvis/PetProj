using PetProj.Commands;
using PetProj.Common;
using PetProj.Controllers;
using PetProj.Figures;
using PetProj.Geometries;
using PetProj.Selections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PetProj
{
    public partial class DrawControl : UserControl, IUndoRedoSupport
    {
        private int mouseClickCount;
        private PointF firstMouseDown;
        private PointF mousePosition;
        private EditorMode editorMode;

        public EditorMode EditorMode => editorMode;

        private readonly BlowedSelection underCursor = new BlowedSelection();

        private readonly List<Figure> figures = new List<Figure>();
        private readonly SelectionController selectionController;

        private readonly UndoRedoManager undoRedoManager;

        public bool Changed { get; private set; }
        public int SelectionCount => selectionController.Selection.Count;

        public DrawControl()
        {
            InitializeComponent();
            undoRedoManager = new UndoRedoManager();
            undoRedoManager.OnStateChaned += (o, e) => zoomPad.Invalidate();
            selectionController = new SelectionController();
            // подключение обработчиков событий для контроллера выбора
            selectionController.SelectedFigureChanged += BuildInterface;
            selectionController.EditorModeChanged += _ => UpdateInterface();
        }

        private void BuildInterface()
        {
            mouseClickCount = 0;
            UpdateInterface();
        }

        private void UpdateInterface()
        {
            Invalidate();
        }

        private void zoomPad_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;

            // отрисовка созданных фигур
            foreach (var fig in figures)
                fig.Renderer.Render(graphics, fig);

            // отрисовка временно подсвеченных под курсором или рамкой выделения
            underCursor.Render(graphics, Color.Gray);

            // отрисовка выделения
            selectionController.Selection.Render(graphics, 
                editorMode == EditorMode.MoveSelected && mouseClickCount == 1 ? Color.WhiteSmoke : Color.Magenta);

            //// отрисовка маркеров
            //foreach (var marker in selectionController.Markers)
            //{
            //    //marker.Transform.Matrix.Scale(1 / scaleFactor, 1 / scaleFactor);
            //    marker.Renderer.Render(graphics, marker);
            //    //marker.Transform.Matrix.Scale(scaleFactor, scaleFactor);
            //}

            DrawDefaultCursor(graphics, mousePosition);
            if (mouseClickCount == 1)
            {
                switch (editorMode)
                {
                    case EditorMode.Selection:
                        DrawRibbonSelectionRect(graphics, firstMouseDown, mousePosition);
                        break;
                    case EditorMode.BuildLines:
                        DrawRibbonLine(graphics, firstMouseDown, mousePosition);
                        break;
                    case EditorMode.BuildRectangles:
                        DrawRibbonRectangle(graphics, firstMouseDown, mousePosition);
                        break;
                    case EditorMode.MoveSelected:
                    case EditorMode.MoveCopySelected:
                        DrawRibbonMoved(graphics, firstMouseDown, mousePosition);
                        break;
                }
            }
        }

        private void DrawRibbonMoved(Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = PrepareMousePosition(mousePosition);
            using (var pen = new Pen(Color.Gray) { DashStyle = DashStyle.Dash })
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                var state = graphics.Save();

                graphics.TranslateTransform(pt2.X - pt1.X, pt2.Y - pt1.Y);
                // отрисовка выделения
                selectionController.Selection.Render(graphics, Color.Magenta);
                graphics.TranslateTransform(-pt2.X + pt1.X, -pt2.Y + pt1.Y);

                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                graphics.DrawLine(pen, pt1, pt2);
                graphics.Restore(state);
            }
        }

        private void DrawRibbonRectangle(Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = PrepareMousePosition(mousePosition);
            var rect = new RectangleF(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y),
                Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));
            using (var pen = new Pen(Color.Magenta))
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
        }

        private void DrawRibbonLine(Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = PrepareMousePosition(mousePosition);
            using (var pen = new Pen(Color.Magenta))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                graphics.DrawLine(pen, pt1, pt2);
            }
        }

        /// <summary>
        /// Рисуем резиновый прямоугольник выбора
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="firstMouseDown"></param>
        /// <param name="mousePosition"></param>
        private void DrawRibbonSelectionRect(Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = PrepareMousePosition(mousePosition);
            var rect = new RectangleF(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y),
                Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));
            var color = pt1.X > pt2.X ? Color.Green : Color.Blue;
            using (var brush = new SolidBrush(Color.FromArgb(50, color)))
                graphics.FillRectangle(brush, rect);
            using (var pen = new Pen(Color.Black, 0))
            {
                if (pt1.X > pt2.X) pen.DashStyle = DashStyle.Dash;
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
            }
        }

        /// <summary>
        /// Перерасчёт позиции мыши при масштабировании и панарамировании
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private PointF PrepareMousePosition(PointF p)
        {
            PointF[] arr = new PointF[] { p };
            Matrix matrix = new Matrix();

            var zoom = (float)zoomPad.ZoomScale;
            var origin = zoomPad.Origin;

            matrix.Translate(origin.X, origin.Y);
            matrix.Scale(1 / zoom, 1 / zoom);
            matrix.TransformPoints(arr);
            matrix.Dispose();
            return new PointF(arr[0].X, arr[0].Y);
        }

        /// <summary>
        /// Рисуем курсор-перекрестье на всё окно
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="mousePosition"></param>
        private void DrawDefaultCursor(Graphics graphics, PointF mousePosition)
        {
            var pt1 = PrepareMousePosition(new PointF(0, mousePosition.Y));
            var pt2 = PrepareMousePosition(new PointF(zoomPad.Width, mousePosition.Y));
            var pt3 = PrepareMousePosition(new PointF(mousePosition.X, 0));
            var pt4 = PrepareMousePosition(new PointF(mousePosition.X, zoomPad.Height));
            using (var pen = new Pen(Color.Gray, 0))
            {
                graphics.DrawLine(pen, pt1, pt2);
                graphics.DrawLine(pen, pt3, pt4);
            }
        }

        private void zoomPad_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                underCursor.Clear();
                mousePosition = e.Location;
                if (mouseClickCount == 0)
                {
                    // при первом нажатии запоминаем точку нажатия
                    firstMouseDown = PrepareMousePosition(mousePosition);
                    mouseClickCount++;
                    if (editorMode == EditorMode.Selection)
                        selectionController.OnMouseDown(figures, firstMouseDown, ModifierKeys);
                }
                else if (mouseClickCount == 1) // это второе нажатие
                {
                    PointF pt1, pt2, pt3, pt4;
                    switch (editorMode)
                    {
                        case EditorMode.Selection:
                            pt1 = firstMouseDown;
                            pt2 = PrepareMousePosition(mousePosition);
                            var selMode = pt1.X > pt2.X;
                            var rectangle = new RectangleF(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y),
                                Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));
                            SelectUnselectByFrame(selMode, rectangle, 
                                    (manager, fig) => 
                                    {
                                        if (!selectionController.Selection.Contains(fig))
                                        {
                                            fig.DrawGlowed(false);
                                            selectionController.Selection.Add(fig);
                                        }
                                    }, 
                                    (manager, fig) => 
                                    {
                                        if (selectionController.Selection.Contains(fig))
                                            selectionController.Selection.Remove(fig);
                                    }
                                );
                            // при отсутствии других режимов - режим выбора, и второе нажатие
                            // сбрасывает количество нажатий
                            mouseClickCount = 0;
                            break;
                        case EditorMode.BuildLines:
                            pt1 = firstMouseDown;
                            pt2 = PrepareMousePosition(mousePosition);
                            AddLine(pt1, pt2);
                            mouseClickCount = 0;
                            firstMouseDown = pt2;
                            mouseClickCount++;
                            Changed = true;
                            break;
                        case EditorMode.BuildRectangles:
                            pt1 = firstMouseDown;
                            pt3 = PrepareMousePosition(mousePosition);
                            pt2 = new PointF(pt3.X, pt1.Y);
                            pt4 = new PointF(pt1.X, pt3.Y);
                            AddLine(pt1, pt2);
                            AddLine(pt2, pt3);
                            AddLine(pt3, pt4);
                            AddLine(pt4, pt1);
                            mouseClickCount = 0;
                            Changed = true;
                            break;
                        case EditorMode.MoveSelected:
                            pt1 = firstMouseDown;
                            pt2 = PrepareMousePosition(mousePosition);
                            selectionController.Selection.Translate(pt2.X - pt1.X, pt2.Y - pt1.Y);
                            selectionController.Selection.Clear();
                            mouseClickCount = 0;
                            SetMode(EditorMode.Selection);
                            Changed = true;
                            break;
                        case EditorMode.MoveCopySelected:
                            pt1 = firstMouseDown;
                            pt2 = PrepareMousePosition(mousePosition);
                            selectionController.Selection.TranslateCopy(pt2.X - pt1.X, pt2.Y - pt1.Y);
                            selectionController.Selection.Clear();
                            mouseClickCount = 0;
                            SetMode(EditorMode.Selection);
                            Changed = true;
                            break;
                    }
                }
                zoomPad.Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (editorMode != EditorMode.Selection)
                    SetMode(EditorMode.Selection);
                else
                {
                    mouseClickCount = 0;
                    selectionController.Clear();
                }
            }
        }

        private void SelectUnselectByFrame(bool selMode, RectangleF rectangle, 
            Action<IListManage, Figure> onSelect, Action<IListManage, Figure> onUnselect)
        {
            using (var image = new Bitmap(Width, Height))
            using (var g = Graphics.FromImage(image))
            {
                foreach (var fig in figures)
                {
                    using (GraphicsPath path = fig.GetRendererPath())
                    {
                        if (selMode)
                        {
                            // захватываем рамкой объекты частично
                            var points = path.PathPoints;
                            List<PointF> interpolatedPoints = new List<PointF>();
                            for (int i = 0; i < points.Length - 1; i++)
                            {
                                PointF current = points[i];
                                PointF next = points[i + 1];
                                // Если отрезок длинный, interpolate
                                if (Distance(current, next) > 1.0f)
                                {
                                    int numSteps = 100; // Number of intermediate points
                                    for (int step = 1; step <= numSteps; step++)
                                    {
                                        float t = (step / (float)numSteps);
                                        PointF interpolated = new PointF(
                                            (int)(current.X + t * (next.X - current.X)),
                                            (int)(current.Y + t * (next.Y - current.Y))
                                        );
                                        interpolatedPoints.Add(interpolated);
                                    }
                                }
                                else
                                {
                                    // Keep the original point if it's a vertex
                                    interpolatedPoints.Add(current);
                                }
                            }
                            if (interpolatedPoints.Any(p => rectangle.Contains(p)))
                            {
                                if (ModifierKeys.HasFlag(Keys.Shift))
                                    onUnselect(selectionController.Selection, fig);
                                else
                                    onSelect(selectionController.Selection, fig);
                            }
                        }
                        else
                        {
                            // захватываем рамкой объекты целиком
                            var figrect = path.GetBounds();
                            var rect = new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                            figrect.Intersect(rect);
                            if (figrect.Equals(path.GetBounds()))
                            {
                                if (ModifierKeys.HasFlag(Keys.Shift))
                                    onUnselect(selectionController.Selection, fig);
                                else
                                    onSelect(selectionController.Selection, fig);
                            }
                        }
                    }
                }
            }
        }

        private float Distance(PointF current, PointF next)
        {
            return (float)Math.Sqrt(Math.Pow(next.X - current.X, 2) + Math.Pow(next.Y - current.Y, 2));
        }

        private void zoomPad_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            var pt = PrepareMousePosition(mousePosition);

            selectionController.OnMouseMove(pt, ModifierKeys);

            if (mouseClickCount == 1 && editorMode == EditorMode.Selection)
            {
                // определение фигуры под рамкой выбора
                var pt1 = firstMouseDown;
                var pt2 = PrepareMousePosition(mousePosition);
                var selMode = pt1.X > pt2.X;
                var rectangle = new RectangleF(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y),
                    Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));
                underCursor.Clear();
                SelectUnselectByFrame(selMode, rectangle,
                        (manager, fig) =>
                        {
                            if (!underCursor.Contains(fig))
                                underCursor.Add(fig);
                        },
                        (manager, fig) =>
                        {
                            if (underCursor.Contains(fig))
                                underCursor.Remove(fig);
                        }
                    );
            }
            if (mouseClickCount == 0)
            {
                // определение фигуры непосредственно под курсором
                underCursor.Clear();
                var fig = figures.LastOrDefault(x => x.Contains(pt));
                if (fig != null)
                    underCursor.Add(fig);
            }

            zoomPad.Invalidate();
        }

        private void zoomPad_MouseUp(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            var pt = PrepareMousePosition(mousePosition);

            if (e.Button == MouseButtons.Left)
                selectionController.OnMouseUp(pt, ModifierKeys);

            zoomPad.Invalidate();
        }

        private void AddLine(PointF pt1, PointF pt2, bool loading = false)
        {
            Figure line = new Figure();
            FigureBuilder.BuildAddLineGeometry(line, pt1);
            ((AddLineGeometry)line.Geometry).AddPoint(pt2);
            line.Style.FillStyle.IsVisible = false;
            if (loading)
                figures.Add(line);
            else
                undoRedoManager.Execute(new CreateFigureCommand(figures, line));
        }

        public void RemoveSelected()
        {
            foreach (var fig in selectionController.Selection)
                undoRedoManager.Execute(new RemoveFigureCommand(figures, fig));
            selectionController.Selection.Clear();
            underCursor.Clear();
            Changed = true;
        }

        public EventHandler OnSelectionMode;

        public void SetMode(EditorMode selection)
        {
            editorMode = selection;
            mouseClickCount = 0;
            zoomPad.Invalidate();
            switch (editorMode)
            {
                case EditorMode.Selection:
                    OnSelectionMode?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        /// <summary>
        /// Сохранение документа чертежа в xml файле
        /// </summary>
        /// <param name="filename"></param>
        public void SaveDocument(string filename)
        {
            try
            {
                var root = new XElement("Document");
                root.Add(new XAttribute("Name", System.IO.Path.GetFileNameWithoutExtension(filename)));
                var doc = new XDocument(new XComment("Данные чертёжного документа"), root);
                var xmodel = new XElement("Model");
                root.Add(xmodel);
                foreach (var figure in figures)
                {
                    if (figure.Geometry is AddLineGeometry lineGeometry)
                    {
                        if (lineGeometry.Points.Count == 2)
                        {
                            var xline = new XElement("Line");
                            xline.Add(new XAttribute("Start", lineGeometry.StartPoint));
                            xline.Add(new XAttribute("End", lineGeometry.EndPoint));
                            xmodel.Add(xline);
                        }
                    }
                }
                doc.Save(filename);
                Changed = false;
                zoomPad.Invalidate();
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Загрузка документа чертежа из xml файла
        /// </summary>
        /// <param name="filename"></param>
        public void LoadDocument(string filename)
        {
            try
            {
                undoRedoManager.Clear();
                var xdoc = XDocument.Load(filename);
                var root = xdoc.Element("Document");
                var name =  root.Attribute("Name")?.Value;
                var model = root.Element("Model");
                figures.Clear();
                zoomPad.Reset();
                foreach (var xelement in model.Descendants())
                {
                    var figureName = $"{xelement.Name}";
                    switch (figureName)
                    {
                        case "Line":
                            CultureInfo culture = CultureInfo.CurrentCulture;
                            var decsep = culture.NumberFormat.NumberDecimalSeparator;
                            var pt1 = Parse(xelement.Attribute("Start")?.Value, decsep);
                            var pt2 = Parse(xelement.Attribute("End")?.Value, decsep);
                            AddLine(pt1, pt2, loading: true);
                            break;
                    }
                }
                Changed = false;
                zoomPad.Invalidate();
            }
            catch
            {
                figures.Clear();
                Changed = false;
                throw;
            }
        }

        /// <summary>
        /// Разбор символьной записи для получения координат точки
        /// </summary>
        /// <param name="line"></param>
        /// <param name="decimalseparator"></param>
        /// <returns></returns>
        private PointF Parse(string line, string decimalseparator)
        {
            // Разбиваем по запятым и убираем пустые элементы
            string[] tokens = line.Trim('{', '}').Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 2)
            {
                string valueX = tokens[0].Split('=').Last();
                string valueY = tokens[1].Split('=').Last();

                switch (decimalseparator)
                {
                    case ".":
                        valueX = valueX.Replace(',', '.');
                        valueY = valueY.Replace(',', '.');
                        break;
                    case ",":
                        valueX = valueX.Replace('.', ',');
                        valueY = valueY.Replace('.', ',');
                        break;
                }

                // Проверяем, что удалось успешно преобразовать обе координаты
                if (float.TryParse(valueX, out float x) && float.TryParse(valueY, out float y))
                    return new PointF(x, y);
            }
            return PointF.Empty;
        }

        public void CreateNewDocument()
        {
            undoRedoManager.Clear();
            figures.Clear();
            zoomPad.Reset();
            Changed = false;
            zoomPad.Invalidate();
        }

        public void SelectAll()
        {
            selectionController.Selection.Clear();
            foreach(var fig in figures)
                selectionController.Selection.Add(fig);
            zoomPad.Invalidate();
        }

        public void Undo()
        {
            selectionController.Selection.Clear();
            undoRedoManager.Undo();
            Changed = true;
        }

        public void Redo()
        {
            selectionController.Selection.Clear();
            undoRedoManager.Redo();
            Changed = true;
        }

        public bool CanUndo()
        {
            return undoRedoManager.UndoPossible();
        }

        public bool CanRedo()
        {
            return undoRedoManager.RedoPossible();
        }

        public void MoveSelected()
        {
            editorMode = EditorMode.MoveSelected;
        }

        public void MoveCopySelected()
        {
            editorMode = EditorMode.MoveCopySelected;
        }
    }
}
