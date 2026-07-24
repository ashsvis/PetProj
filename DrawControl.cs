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

        public bool IsDynamicalEnter { get; set; } = true;
        public bool IsDrawOrtoMode { get; set; } = false;

        public EditorMode EditorMode => editorMode;
        public int MouseClickCount => mouseClickCount;

        private readonly BlowedSelection underCursor = new BlowedSelection();

        private readonly List<Figure> figures = new List<Figure>();
        private readonly SelectionController selectionController;

        private readonly UndoRedoManager undoRedoManager;

        public bool Changed { get; private set; }
        public SelectionController SelectionController => selectionController;

        public event EventHandler OnSelectionMode;
        public event EventHandler<string> OnToolTipChanged;
        public event EventHandler<EditorMode> OnChangeMode;
        public event EventHandler<object[]> OnChangeParams;
        public event EventHandler<(int, PointF, Point)> OnCursorMoved;
        public event EventHandler<Selection> OnSelected;

        public Figure Layer { get; set; }

        public DrawControl()
        {
            InitializeComponent();
            Layer = new Figure();
            FigureBuilder.BuildLayerGeomentry(Layer);
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

        public void UpdateInterface()
        {
            Invalidate();
            zoomPad.Invalidate();
        }

        private void zoomPad_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;
            // рисуем начало координат и направление осей
            DrawZeroOrigin(graphics, Color.LightGray, (float)zoomPad.ZoomScale);

            // отрисовка созданных фигур
            foreach (var fig in figures)
                fig.Renderer.Render(graphics, fig);

            // отрисовка временно подсвеченных под курсором или рамкой выделения
            underCursor.Render(graphics, Color.White, (float)zoomPad.ZoomScale);

            // отрисовка выделения
            selectionController.Selection.Render(graphics,
                editorMode == EditorMode.MoveSelected && mouseClickCount == 1 ? Color.Silver : Color.Magenta);

            //// отрисовка маркеров
            //foreach (var marker in selectionController.Markers)
            //{
            //    //marker.Transform.Matrix.Scale(1 / scaleFactor, 1 / scaleFactor);
            //    marker.Renderer.Render(graphics, marker);
            //    //marker.Transform.Matrix.Scale(scaleFactor, scaleFactor);
            //}

            DrawDefaultCursor(graphics, mousePosition);
            float kf = (float)(1f / zoomPad.ZoomScale);
            PointF pt;
            string text;
            switch (editorMode)
            {
                case EditorMode.Selection:
                    if (mouseClickCount == 1)
                        DrawRibbonSelectionRect(graphics, firstMouseDown, mousePosition);
                    break;
                case EditorMode.BuildLines:
                    {
                        pt = PrepareMousePosition(mousePosition);
                        text = (mouseClickCount == 0 ? $"Первая точка " : $"Следующая точка ") + $" X:{pt.X} Y:{pt.Y}";
                        using (var pen = new Pen(Color.Black, kf))
                        using (var font = new Font("Arial", (float)(10f * kf)))
                            graphics.DrawString(text, font, Brushes.Black, PrepareMousePosition(PointF.Add(mousePosition, new SizeF(1f, 1f))));
                        if (mouseClickCount == 1)
                            DrawRibbonLine(graphics, firstMouseDown, mousePosition);
                        break;
                    }
                case EditorMode.BuildRectangles:
                    pt = PrepareMousePosition(mousePosition);
                    text = (mouseClickCount == 0 ? $"Укажите точку первого угла " : $"Укажите точку второго угла ") + $" X:{pt.X} Y:{pt.Y}";
                    using (var pen = new Pen(Color.Black, kf))
                    using (var font = new Font("Arial", (float)(10f * kf)))
                        graphics.DrawString(text, font, Brushes.Black, PrepareMousePosition(PointF.Add(mousePosition, new SizeF(1f, 1f))));
                    if (mouseClickCount == 1)
                        DrawRibbonRectangle(graphics, firstMouseDown, mousePosition);
                    break;
                case EditorMode.MoveSelected:
                case EditorMode.MoveCopySelected:
                    if (mouseClickCount == 1)
                        DrawRibbonMoved(graphics, firstMouseDown, mousePosition);
                    break;
            }
        }

        private void DrawZeroOrigin(Graphics graphics, Color color, float zoomScale)
        {
            using (var pen = new Pen(color, (float)(2f / zoomScale)))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                var gs = graphics.Save();
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                using (var penX = new Pen(Color.FromArgb(80, Color.LightSalmon), (float)(2f / zoomScale)))
                {
                    graphics.DrawLine(penX, new PointF(0f, 0f), PrepareMousePositionX(new PointF(zoomPad.Width, 0f)));
                }
                using (var penY = new Pen(Color.FromArgb(80, Color.LightGreen), (float)(2f / zoomScale)))
                {
                    graphics.DrawLine(penY, new PointF(0f, 0f), PrepareMousePositionY(new PointF(0f, zoomPad.Height)));
                }
                graphics.DrawLine(pen, new PointF(0f, 0f), new PointF(50f / zoomScale, 0f));
                graphics.DrawLine(pen, new PointF(0f, 0f), new PointF(0f, 50f / zoomScale));
                var rect = new RectangleF(-4f / zoomScale, -4f / zoomScale, 8f / zoomScale, 8f / zoomScale);
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
                using (var font = new Font("Arial", 10f / zoomScale))
                using (var brush = new SolidBrush(color))
                {
                    graphics.DrawString("X", font, brush, new PointF(50f / zoomScale, 0f));
                    graphics.DrawString("Y", font, brush, new PointF(0f, 50f / zoomScale));
                }
                graphics.Restore(gs);
            }
        }

        private void DrawRibbonMoved(Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = PrepareMousePosition(mousePosition);
            using (var pen = new Pen(Color.Gray, (float)(2.6f / zoomPad.ZoomScale)) { DashStyle = DashStyle.Dash })
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
            using (var pen = new Pen(Color.Magenta, (float)(2.6f / zoomPad.ZoomScale)))
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
            if (mouseClickCount == 1)
            {
                if (IsDynamicalEnter)
                {
                    using (var pen = new Pen(Color.Gray, 0) { DashStyle = DashStyle.Dot })
                    {
                        var kf = (float)(50f / zoomPad.ZoomScale);
                        var p1 = pt1;
                        var p2 = pt2;
                        if (pt2.X > pt1.X)
                        {
                            DrawSizeLine(graphics, pen, new PointF(p1.X, p2.Y), new PointF(p2.X, p2.Y), kf, pt2.Y > pt1.Y);
                            DrawSizeLine(graphics, pen, new PointF(p2.X, p1.Y), new PointF(p2.X, p2.Y), kf, pt2.Y < pt1.Y);
                        }
                        else
                        {
                            DrawSizeLine(graphics, pen, new PointF(p1.X, p2.Y), new PointF(p2.X, p2.Y), kf, pt2.Y < pt1.Y);
                            DrawSizeLine(graphics, pen, new PointF(p2.X, p1.Y), new PointF(p2.X, p2.Y), kf, pt2.Y > pt1.Y);
                        }
                    }
                }
            }
        }

        private void DrawRibbonLine(Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = PrepareMousePosition(mousePosition);
            if (IsDrawOrtoMode)
            {
                var dx = Math.Abs(pt2.X - firstMouseDown.X);
                var dy = Math.Abs(pt2.Y - firstMouseDown.Y);
                if (dx < dy)
                    pt2.X = firstMouseDown.X;
                else
                    pt2.Y = firstMouseDown.Y;
            }
            using (var pen = new Pen(Color.Magenta, (float)(2.6f / zoomPad.ZoomScale)))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                graphics.DrawLine(pen, pt1, pt2);
            }
            if (mouseClickCount == 1)
            {
                if (IsDynamicalEnter)
                {
                    using (var pen = new Pen(Color.Gray, 0) { DashStyle = DashStyle.Dot })
                    {
                        DrawSizeLine(graphics, pen, pt1, pt2, (float)(50f / zoomPad.ZoomScale)); // Выноска размера 50 пикселей
                        DrawAngleLine(graphics, pen, pt1, pt2);
                    }
                }
            }
        }

        /// <summary>
        /// Рисуем дугу, показывающую угол наклона отрезка к горизонтали
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void DrawAngleLine(Graphics graphics, Pen pen, PointF start, PointF end)
        {
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            if (length == 0)
            { 
                // Отрезок вырожден в точку
                return;
            }
            // выносная линия, горизонтальная
            var b1 = PointF.Add(start, new SizeF(length, 0));
            graphics.DrawLine(pen, start, b1);
            var arcrect = new RectangleF(start.X - length, start.Y - length, length * 2, length * 2);
            var angle = Math.Atan2(dy, dx);
            var cx = start.X + length * Math.Cos(angle / 2);
            var cy = start.Y + length * Math.Sin(angle / 2);
            var mid = new PointF((float)cx, (float)cy);
            var angleDegree = (float)(angle * 180 / Math.PI);
            var L = Math.PI * length * Math.Abs(angleDegree) / 180;
            if (L < 35)
                mid = new PointF(end.X + 5, end.Y + 25);
            var sarc = $"{angleDegree}°";
            try
            {
                graphics.DrawArc(pen, arcrect, 0, angleDegree);
                DrawTextAtCenter(graphics, pen, Brushes.White, mid, sarc);
            }
            catch { }
        }

        /// <summary>
        /// Рисуем размерную линию для отрезка
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="halfLength"></param>
        private void DrawSizeLine(Graphics graphics, Pen pen, PointF start, PointF end, float halfLength, bool? side = null)
        {
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float px = dy;
            float py = -dx;
            float length = (float)Math.Sqrt(px * px + py * py);
            if (length == 0)
            {
                // Отрезок вырожден в точку
                return;
            }
            px /= length;
            py /= length;

            bool kind = side ?? px > 0;
            // перпендикуляр в начале отрезка
            PointF df = kind ? new PointF(start.X, start.Y) : new PointF(start.X + px * halfLength, start.Y + py * halfLength);
            PointF ef = !kind ? new PointF(start.X, start.Y) : new PointF(start.X - px * halfLength, start.Y - py * halfLength);
            graphics.DrawLine(pen, df, ef);
            // перпендикуляр в конце отрезка
            PointF de = kind ? new PointF(end.X, end.Y) : new PointF(end.X + px * halfLength, end.Y + py * halfLength);
            PointF ee = !kind ? new PointF(end.X, end.Y) : new PointF(end.X - px * halfLength, end.Y - py * halfLength);
            graphics.DrawLine(pen, de, ee);
            // выносная линия, соединяющая два перпендикуляра
            graphics.DrawLine(pen, kind ? ef : df, kind ? ee : de);
            PointF mid = kind ? new PointF((ef.X + ee.X) / 2, (ef.Y + ee.Y) / 2) : new PointF((df.X + de.X) / 2, (df.Y + de.Y) / 2);
            var slength = $"{length}";
            DrawTextAtCenter(graphics, pen, Brushes.White, mid, slength);
        }

        private void DrawTextAtCenter(Graphics graphics, Pen pen, Brush background, PointF mid, string text)
        {
            using (var font = new Font("Segoe UI", (float)(10f / zoomPad.ZoomScale)))
            {
                var ms = graphics.MeasureString(text, font);
                var rect = new RectangleF(mid.X - ms.Width / 2, mid.Y - ms.Height / 2, ms.Width, ms.Height);
                graphics.FillRectangles(background, new RectangleF[] { rect });
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
                using (var brush = new SolidBrush(Color.Black))
                    graphics.DrawString(text, font, brush, rect);
            }
        }

        private void DrawTextAtLocation(Graphics graphics, Pen pen, Brush background, PointF location, string text)
        {
            using (var font = new Font("Segoe UI", (float)(10f / zoomPad.ZoomScale)))
            {
                var ms = graphics.MeasureString(text, font);
                var rect = new RectangleF(location.X, location.Y, ms.Width, ms.Height);
                graphics.FillRectangles(background, new RectangleF[] { rect });
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
                using (var brush = new SolidBrush(Color.Black))
                    graphics.DrawString(text, font, brush, rect);
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
        public PointF PrepareMousePosition(PointF p)
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
        /// Обратный перерасчёт позиции первого нажатия мыши
        /// при масштабировании и панарамировании
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PointF GetFirstMouseDownPosition()
        {
            PointF[] arr = new PointF[] { firstMouseDown };
            var origin = zoomPad.Origin;
            var zoom = (float)zoomPad.ZoomScale;

            Matrix matrix = new Matrix();
            matrix.Scale(zoom, zoom);
            matrix.Translate(-origin.X, -origin.Y);
            matrix.TransformPoints(arr);
            matrix.Dispose();
            return new PointF(arr[0].X, arr[0].Y);
        }

        private PointF PrepareMousePositionX(PointF p)
        {
            PointF[] arr = new PointF[] { p };
            Matrix matrix = new Matrix();

            var zoom = (float)zoomPad.ZoomScale;
            var origin = zoomPad.Origin;

            matrix.Translate(origin.X, 0);
            matrix.Scale(1 / zoom, 1);
            matrix.TransformPoints(arr);
            matrix.Dispose();
            return new PointF(arr[0].X, arr[0].Y);
        }

        private PointF PrepareMousePositionY(PointF p)
        {
            PointF[] arr = new PointF[] { p };
            Matrix matrix = new Matrix();

            var zoom = (float)zoomPad.ZoomScale;
            var origin = zoomPad.Origin;

            matrix.Translate(0, origin.Y);
            matrix.Scale(1, 1 / zoom);
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

        /// <summary>
        /// Нажатие кнопки указателя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomPad_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PressLeftMouseButton(e.Location);
            else if (e.Button == MouseButtons.Right)
                PressRightMouseButton(e.Location);
        }

        private void PressLeftMouseButton(PointF point, bool calledByCode = false)
        {
            underCursor.Clear();
            mousePosition = point;
            if (mouseClickCount == 0)
            {
                // при первом нажатии запоминаем точку нажатия
                firstMouseDown = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
                mouseClickCount++;
                if (editorMode == EditorMode.Selection)
                {
                    selectionController.OnMouseDown(figures, firstMouseDown, ModifierKeys);
                    OnSelected?.Invoke(this, selectionController.Selection);
                }
            }
            else if (mouseClickCount == 1) // это второе нажатие
            {
                PointF pt1, pt2, pt3, pt4;
                switch (editorMode)
                {
                    case EditorMode.Selection:
                        pt1 = firstMouseDown;
                        pt2 = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
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
                                        OnSelected?.Invoke(this, selectionController.Selection);
                                    }
                                },
                                (manager, fig) =>
                                {
                                    if (selectionController.Selection.Contains(fig))
                                    {
                                        selectionController.Selection.Remove(fig);
                                        OnSelected?.Invoke(this, selectionController.Selection);
                                    }
                                }
                            );
                        // при отсутствии других режимов - режим выбора, и второе нажатие
                        // сбрасывает количество нажатий
                        mouseClickCount = 0;
                        break;
                    case EditorMode.BuildLines:
                        // построение отрезков линий по двум точкам (концы отрезка)
                        pt1 = firstMouseDown;
                        if (IsDrawOrtoMode)
                        {
                            pt2 = PrepareMousePosition(mousePosition);
                            var dx = Math.Abs(firstMouseDown.X - pt2.X);
                            var dy = Math.Abs(firstMouseDown.Y - pt2.Y);
                            if (dx < dy)
                                pt2.X = firstMouseDown.X;
                            else
                                pt2.Y = firstMouseDown.Y;
                        }
                        else
                            pt2 = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
                        AddLine(pt1, pt2);
                        // сброс количества нажатий, следующий прямоугольник будет строиться заново
                        mouseClickCount = 0;
                        // точка начала следующего отрезка совпадает с концом предыдущего отрезка
                        firstMouseDown = pt2;
                        mouseClickCount++;
                        Changed = true;
                        break;
                    case EditorMode.BuildRectangles:
                        // построение прямоугольника по двум точкам диагонали
                        pt1 = firstMouseDown; // первая точка нажатия
                        pt3 = calledByCode ? mousePosition : PrepareMousePosition(mousePosition); // вторая точка нажатия
                        pt2 = new PointF(pt3.X, pt1.Y); // раcчётная точка
                        pt4 = new PointF(pt1.X, pt3.Y); // раcчётная точка
                        AddRectangle(pt1, pt2, pt3, pt4);
                        // сброс количества нажатий, следующий прямоугольник будет строиться заново
                        mouseClickCount = 0;
                        Changed = true;
                        break;
                    case EditorMode.MoveSelected:
                        pt1 = firstMouseDown;
                        pt2 = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
                        selectionController.Selection.Translate(pt2.X - pt1.X, pt2.Y - pt1.Y,
                            (movedoffsets) =>
                            {
                                undoRedoManager.Execute(new MoveFiguresCommand(movedoffsets));
                            });
                        // предыдущий выбор стирается, т.к. перемещение - однократная операция
                        selectionController.Selection.Clear();
                        mouseClickCount = 0;
                        SetMode(EditorMode.Selection);
                        Changed = true;
                        break;
                    case EditorMode.MoveCopySelected:
                        pt1 = firstMouseDown;
                        pt2 = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
                        selectionController.Selection.TranslateCopy(pt2.X - pt1.X, pt2.Y - pt1.Y,
                            (addedfigs) =>
                            {
                                undoRedoManager.Execute(new CreateFiguresCommand(figures, addedfigs));
                            });
                        Changed = true;
                        break;
                }
            }
            zoomPad.Invalidate();
        }

        private void PressRightMouseButton(Point screenMouseLocation, bool calledByCode = false)
        {
            OnToolTipChanged?.Invoke(this, string.Empty);
            if (editorMode == EditorMode.MoveCopySelected)
            {
                mouseClickCount = 0;
                selectionController.Clear();
                OnSelected?.Invoke(this, selectionController.Selection);
                SetMode(EditorMode.Selection);
            }
            else if (editorMode == EditorMode.Selection)
            {
                mouseClickCount = 0;
                selectionController.Clear();
                OnSelected?.Invoke(this, selectionController.Selection);
            }
            else if (editorMode != EditorMode.Selection)
                SetMode(EditorMode.Selection);
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
                            // захватываем рамкой объекты даже частично
                            if (fig.Intersects(rectangle))
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
                            if (fig.Contains(rectangle))
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

        private double Distance(PointF current, PointF next)
        {
            return Math.Sqrt(Math.Pow(next.X - current.X, 2) + Math.Pow(next.Y - current.Y, 2));
        }

        private void zoomPad_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            var pt = PrepareMousePosition(mousePosition);
            OnCursorMoved?.Invoke(this, (mouseClickCount, firstMouseDown, Point.Ceiling(/*e.Location*/pt)));

            if (e.Button == MouseButtons.None)
            {
                switch (editorMode)
                {
                    case EditorMode.BuildLines:
                        if (mouseClickCount == 0)
                            OnChangeParams?.Invoke(this, new object[] { pt });
                        else if (mouseClickCount == 1)
                        {
                            var pt1 = firstMouseDown;
                            var pt2 = PrepareMousePosition(mousePosition);
                            var vector = pt2.Vector(pt1);
                            OnChangeParams?.Invoke(this, new object[] { vector.Length(), vector.AngleDegree() });
                        }
                        break;
                    case EditorMode.BuildRectangles:
                        if (mouseClickCount == 0)
                            OnChangeParams?.Invoke(this, new object[] { pt });
                        else if (mouseClickCount == 1)
                        {
                            var pt1 = firstMouseDown; // первая точка нажатия
                            var pt3 = PrepareMousePosition(mousePosition); // вторая точка нажатия
                            var pt2 = new PointF(pt3.X, pt1.Y); // расчётная точка
                            var pt4 = new PointF(pt1.X, pt3.Y); // расчётная точка
                            var width = Math.Sqrt((pt2.X - pt1.X) * (pt2.X - pt1.X) + (pt2.Y - pt1.Y) * (pt2.Y - pt1.Y));
                            var height = Math.Sqrt((pt3.X - pt2.X) * (pt3.X - pt2.X) + (pt3.Y - pt2.Y) * (pt3.Y - pt2.Y));

                            var vector1 = pt2.Vector(pt1);
                            var vector2 = pt3.Vector(pt2);
                            OnChangeParams?.Invoke(this, new object[] { vector1.Length(), vector2.Length() });
                        }
                        break;
                    default:
                        OnToolTipChanged?.Invoke(this, $"Текущая точка курсора X:{pt.X} Y:{pt.Y}");
                        break;
                }
            }

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
                OnSelected?.Invoke(this, selectionController.Selection);
            }
            if (mouseClickCount == 0)
            {
                // определение фигуры непосредственно под курсором
                underCursor.Clear();
                var fig = figures.LastOrDefault(x => x.Contains(pt, (float)(1.0f / zoomPad.ZoomScale)));
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
            {
                selectionController.OnMouseUp(pt, ModifierKeys);
                OnSelected?.Invoke(this, selectionController.Selection);
            }

            zoomPad.Invalidate();
        }
        private Figure CreateLine(PointF pt1, PointF pt2)
        {
            Figure line = new Figure();
            line.Style.BorderStyle = Layer.Style.BorderStyle.DeepCopy();
            FigureBuilder.BuildAddLineGeometry(line, pt1);
            ((AddLineGeometry)line.Geometry).AddPoint(pt2);
            line.Style.FillStyle.IsVisible = false;
            return line;
        }

        /// <summary>
        /// Добавление отрезка линии по двум точкам
        /// </summary>
        /// <param name="pt1">Первая точка</param>
        /// <param name="pt2">Вторая точка</param>
        /// <param name="loading">Признак загрузки из внешнего источника (для исключения поддержки undo)</param>
        private void AddLine(PointF pt1, PointF pt2, bool loading = false)
        {
            Figure line = CreateLine(pt1, pt2);
            if (loading)
                figures.Add(line);
            else
                undoRedoManager.Execute(new CreateFigureCommand(figures, line));
        }

        /// <summary>
        /// Добавление прямоугольника по четырём точкам вершин отрезками линий
        /// </summary>
        /// <param name="pt1">Первая точка (левый верхний)</param>
        /// <param name="pt2">Вторая точка (правый верхний)</param>
        /// <param name="pt3">Третья точка (правый нижний)</param>
        /// <param name="pt4">Четвёртая точка (левый нижний)</param>
        /// <param name="loading">Признак загрузки из внешнего источника (для исключения поддержки undo)</param>
        private void AddRectangle(PointF pt1, PointF pt2, PointF pt3, PointF pt4, bool loading = false)
        {
            Figure line1 = CreateLine(pt1, pt2);
            Figure line2 = CreateLine(pt2, pt3);
            Figure line3 = CreateLine(pt3, pt4);
            Figure line4 = CreateLine(pt4, pt1);
            if (loading)
            {
                figures.Add(line1);
                figures.Add(line2);
                figures.Add(line3);
                figures.Add(line4);
            }
            else
                undoRedoManager.Execute(new CreateFiguresCommand(figures, new List<Figure>() { line1, line2, line3, line4 }));
        }

        /// <summary>
        /// Удаление выбранных фигур
        /// </summary>
        public void RemoveSelected()
        {
            foreach (var fig in selectionController.Selection)
                undoRedoManager.Execute(new RemoveFigureCommand(figures, fig));
            selectionController.Selection.Clear();
            underCursor.Clear();
            Changed = true;
        }

        /// <summary>
        /// Установка и запоминание режима работы редактора
        /// </summary>
        /// <param name="selection"></param>
        public void SetMode(EditorMode selection)
        {
            editorMode = selection;
            mouseClickCount = 0;
            zoomPad.Invalidate();
            switch (editorMode)
            {
                case EditorMode.Selection:
                    // при выборе режима "Выбор фигур" вызывается поключенное событие для обновления интерфейса
                    OnSelectionMode?.Invoke(this, EventArgs.Empty);
                    break;
                case EditorMode.BuildLines:
                    OnChangeMode?.Invoke(this, selection);
                    OnToolTipChanged?.Invoke(this, "Укажите первую точку отрезка:");
                    break;
                default:
                    OnChangeMode?.Invoke(this, selection);
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
                var name = root.Attribute("Name")?.Value;
                var model = root.Element("Model");
                figures.Clear();
                selectionController.Clear();
                zoomPad.Reset();
                foreach (var xelement in model.Descendants())
                {
                    var figureName = $"{xelement.Name}";
                    switch (figureName)
                    {
                        case "Line":
                            var pt1 = ParseHelper.ParsePointF(xelement.Attribute("Start")?.Value, PointF.Empty);
                            var pt2 = ParseHelper.ParsePointF(xelement.Attribute("End")?.Value, PointF.Empty);
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
        /// Создание нового документа
        /// </summary>
        public void CreateNewDocument()
        {
            figures.Clear();
            selectionController.Clear();
            undoRedoManager.Clear();
            zoomPad.Reset();
            Changed = false;
            zoomPad.Invalidate();
        }

        /// <summary>
        /// Выбор всех фигур в редакторе
        /// </summary>
        public void SelectAll()
        {
            selectionController.Selection.Clear();
            foreach (var fig in figures)
                selectionController.Selection.Add(fig);
            zoomPad.Invalidate();
        }

        /// <summary>
        /// Отменить предыдущее действие
        /// </summary>
        public void Undo()
        {
            selectionController.Selection.Clear();
            undoRedoManager.Undo();
            Changed = true;
        }

        /// <summary>
        /// Вернуть отменённое предыдущее действие
        /// </summary>
        public void Redo()
        {
            selectionController.Selection.Clear();
            undoRedoManager.Redo();
            Changed = true;
        }

        /// <summary>
        /// Возможность отмены
        /// </summary>
        /// <returns></returns>
        public bool CanUndo()
        {
            return undoRedoManager.UndoPossible();
        }

        /// <summary>
        /// Возможность вернуть отменённое
        /// </summary>
        /// <returns></returns>
        public bool CanRedo()
        {
            return undoRedoManager.RedoPossible();
        }

        /// <summary>
        /// Переключение в режим Переместить
        /// </summary>
        public void MoveSelected()
        {
            editorMode = EditorMode.MoveSelected;
        }

        /// <summary>
        /// Переключение в режим Копировать и Переместить
        /// </summary>
        public void MoveCopySelected()
        {
            editorMode = EditorMode.MoveCopySelected;
        }

        private void zoomPad_OnPanOrZoom(object sender, ZoomControl.PanOrZoomEventArgs e)
        {
            var pt = e.ViewPort;
            OnToolTipChanged?.Invoke(this, $"Смещение dX:{pt.X} dY:{pt.Y}, зум: {e.Zoom}");
        }

        public void EscapeKeyPressed()
        {
            PressRightMouseButton(MousePosition, calledByCode: true);
        }

        public void SetParameters(string[] strings)
        {
            switch (editorMode)
            {
                case EditorMode.BuildLines:
                    if (strings.Length == 2)
                    {
                        if (mouseClickCount == 0)
                        {
                            if (double.TryParse(strings[0], out double ppX) &&
                                double.TryParse(strings[1], out double ppY))
                                SetFirstPoint(ppX, ppY);
                        }
                        else
                        {
                            if (double.TryParse(strings[0], out double length) &&
                                double.TryParse(strings[1], out double angledeg))
                                SetLineLengthAndAngle(length, angledeg);
                        }
                    }
                    break;
                case EditorMode.BuildRectangles:
                    if (strings.Length == 2)
                    {
                        if (mouseClickCount == 0)
                        {
                            if (double.TryParse(strings[0], out double ppX) &&
                                double.TryParse(strings[1], out double ppY))
                                SetFirstPoint(ppX, ppY);
                        }
                        else
                        {
                            if (double.TryParse(strings[0], out double width) &&
                                double.TryParse(strings[1], out double height))
                            {
                                SetRectangleWidthAndHeight(
                                    Math.Sign(mousePosition.X - firstMouseDown.X) * width, 
                                    Math.Sign(mousePosition.Y - firstMouseDown.Y) * height);
                            }
                        }
                    }
                    break;
            }
        }

        private void SetRectangleWidthAndHeight(double width, double height)
        {
            // построение прямоугольника по двум точкам диагонали
            var pt1 = firstMouseDown; // первая точка нажатия
            var pt2 = new PointF(pt1.X + (float)width, pt1.Y); // раcчётная точка
            var pt3 = new PointF(pt1.X + (float)width, pt1.Y + (float)height); // раcчётная точка
            var pt4 = new PointF(pt1.X, pt1.Y + (float)height); // раcчётная точка
            AddRectangle(pt1, pt2, pt3, pt4);
            // сброс количества нажатий, следующий прямоугольник будет строиться заново
            mouseClickCount = 0;
            Changed = true;
        }

        public void SetFirstPoint(double pxX, double pxY)
        {
            mousePosition = new PointF((float)pxX, (float)pxY);
            PressLeftMouseButton(mousePosition, calledByCode: true);
            zoomPad_MouseMove(zoomPad, new MouseEventArgs(MouseButtons.None, 1, (int)pxX, (int)pxY, 0));
            var pt = PrepareMousePosition(mousePosition);
            OnToolTipChanged?.Invoke(this, $"Текущая точка курсора X:{pt.X} Y:{pt.Y}");
        }

        public void SetLineLengthAndAngle(double length, double angledeg)
        {
            // построение отрезков линий по первой точке (начало отрезка), длине и углу наклона
            var pt1 = firstMouseDown;
            double angleRad = angledeg * (Math.PI / 180);
            var pt2 = new PointF(pt1.X + (float)(length * Math.Cos(angleRad)), pt1.Y + (float)(length * Math.Sin(angleRad)));
            AddLine(pt1, pt2);
            zoomPad_MouseMove(zoomPad, new MouseEventArgs(MouseButtons.None, 1, (int)pt2.X, (int)pt2.Y, 0));
            // сброс количества нажатий, следующий прямоугольник будет строиться заново
            mouseClickCount = 0;
            // точка начала следующего отрезка совпадает с концом предыдущего отрезка
            firstMouseDown = pt2;
            mouseClickCount++;
            Changed = true;
        }
    }
}
