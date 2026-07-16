using PetProj.Figures;
using PetProj.Geometries;
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
    public partial class DrawControl : UserControl
    {
        private int mouseClickCount;
        private PointF firstMouseDown;
        private PointF mousePosition;
        private EditorMode editorMode;
        private Figure underCursor;

        private readonly List<Figure> figures = new List<Figure>();
        private readonly List<Figure> selected = new List<Figure>();

        public bool Changed { get; private set; }

        public DrawControl()
        {
            InitializeComponent();
        }

        private void zoomPad_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;

            // отрисовка созданных фигур
            foreach (var fig in figures)
            {
                fig.DrawGlowed(fig == underCursor);
                fig.Renderer.Render(graphics, fig);
            }

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
                }
            }
        }

        private void DrawRibbonRectangle(Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = PrepareMousePosition(mousePosition);
            var rect = new RectangleF(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y),
                Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));
            using (var pen = new Pen(Color.Magenta, 1))
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
        }

        private void DrawRibbonLine(Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = PrepareMousePosition(mousePosition);
            using (var pen = new Pen(Color.Magenta, 1))
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
                mousePosition = e.Location;
                if (mouseClickCount == 0)
                {
                    // при первом нажатии запоминаем точку нажатия
                    firstMouseDown = PrepareMousePosition(mousePosition);
                    var fig = figures.LastOrDefault(x => x.Contains(firstMouseDown));
                    if (editorMode == EditorMode.Selection && fig != null)
                    {
                        if (!selected.Contains(fig))
                            selected.Add(fig);
                    }
                    else
                        mouseClickCount++;
                }
                else if (mouseClickCount == 1) // это второе нажатие
                {
                    PointF pt1, pt2, pt3, pt4;
                    switch (editorMode)
                    {
                        case EditorMode.Selection:
                            //var selMode = firstMouseDown.X > mousePosition.X;
                            //var rectangle = new RectangleF(Math.Min(firstMouseDown.X, mousePosition.X), Math.Min(firstMouseDown.Y, mousePosition.Y),
                            //    Math.Abs(firstMouseDown.X - mousePosition.X), Math.Abs(firstMouseDown.Y - mousePosition.Y));
                            // при отсутвии других режимов - режим выбора, и второе нажатие
                            // сбрасывает количество нажатий
                            mouseClickCount = 0;
                            // здесь будет обработка при выборе объектов
                            // при selMode == true выбираются все объекты, хотя бы частично попавшие в rect, а при false - только целиком
                            break;
                        case EditorMode.BuildLines:
                            pt1 = firstMouseDown;
                            pt2 = PrepareMousePosition(mousePosition);
                            AddFigureAsLine(pt1, pt2);
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
                            AddFigureAsLine(pt1, pt2);
                            AddFigureAsLine(pt2, pt3);
                            AddFigureAsLine(pt3, pt4);
                            AddFigureAsLine(pt4, pt1);
                            mouseClickCount = 0;
                            Changed = true;
                            break;
                    }
                }
                zoomPad.Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                SetMode(EditorMode.Selection);
            }
        }

        private void AddFigureAsLine(PointF pt1, PointF pt2)
        {
            Figure figure = new Figure();
            FigureBuilder.BuildAddLineGeometry(figure, pt1);
            ((AddLineGeometry)figure.Geometry).AddPoint(pt2);
            figure.Style.FillStyle.IsVisible = false;
            figures.Add(figure);
        }

        private void zoomPad_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            var pt = PrepareMousePosition(mousePosition);
            underCursor = figures.LastOrDefault(x => x.Contains(pt));
            zoomPad.Invalidate();
        }

        private void zoomPad_MouseUp(object sender, MouseEventArgs e)
        {
            zoomPad.Invalidate();
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
                selected.Clear();
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
                var xdoc = XDocument.Load(filename);
                var root = xdoc.Element("Document");
                var name =  root.Attribute("Name")?.Value;
                var model = root.Element("Model");
                selected.Clear();
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
                            AddFigureAsLine(pt1, pt2);
                            break;
                    }
                }
                Changed = false;
                zoomPad.Invalidate();
            }
            catch
            {
                selected.Clear();
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
            selected.Clear();
            figures.Clear();
            zoomPad.Reset();
            Changed = false;
            zoomPad.Invalidate();
        }
    }
}
