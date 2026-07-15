using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private Primitive underCursor;

        private readonly List<Primitive> figures = new List<Primitive>();

        public bool Changed { get; private set; }

        public DrawControl()
        {
            InitializeComponent();
        }

        private void zoomPad_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;

            foreach (Primitive p in figures)
            {
                p.DrawAt(graphics, Color.Black);
            }

            underCursor?.DrawHighlightAt(graphics, Color.Black);

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
                    case EditorMode.BuildRectangle:
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

                    }
                    else
                        mouseClickCount++;
                }
                else if (mouseClickCount == 1) // это второе нажатие
                {
                    PointF pt1, pt2, pt3, pt4;
                    Line line;
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
                            line = new Line(pt1, pt2);
                            figures.Add(line);
                            mouseClickCount = 0;
                            firstMouseDown = pt2;
                            mouseClickCount++;
                            Changed = true;
                            break;
                        case EditorMode.BuildRectangle:
                            pt1 = firstMouseDown;
                            pt3 = PrepareMousePosition(mousePosition);
                            pt2 = new PointF(pt3.X, pt1.Y);
                            pt4 = new PointF(pt1.X, pt3.Y);
                            line = new Line(pt1, pt2);
                            figures.Add(line);
                            line = new Line(pt2, pt3);
                            figures.Add(line);
                            line = new Line(pt3, pt4);
                            figures.Add(line);
                            line = new Line(pt4, pt1);
                            figures.Add(line);
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
            if (editorMode == EditorMode.Selection)
                OnSelectionMode?.Invoke(this, EventArgs.Empty);
        }

        public void Save(string filename)
        {
            try
            {
                var root = new XElement("Document");
                var doc = new XDocument(new XComment("Данные чертёжного документа"), root);
                var xmodel = new XElement("Model");
                root.Add(xmodel);
                doc.Save(filename);
                Changed = false;
            }
            catch 
            {
                throw;
            }
        }
    }
}
