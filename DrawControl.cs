using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PetProj
{
    public partial class DrawControl : UserControl
    {
        private int mouseClickCount;
        private Point firstMouseDown;
        private Point mousePosition;
        private EditorMode editorMode;

        public DrawControl()
        {
            InitializeComponent();
        }

        private void zoomPad_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;
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

        private void DrawRibbonRectangle(Graphics graphics, Point firstMouseDown, Point mousePosition)
        {
            var rect = new Rectangle(Math.Min(firstMouseDown.X, mousePosition.X), Math.Min(firstMouseDown.Y, mousePosition.Y),
                Math.Abs(firstMouseDown.X - mousePosition.X), Math.Abs(firstMouseDown.Y - mousePosition.Y));
            using (var pen = new Pen(Color.Black, 1) { DashStyle = DashStyle.Dash })
                graphics.DrawRectangle(pen, PrepareRect(rect));
        }

        private void DrawRibbonLine(Graphics graphics, Point firstMouseDown, Point mousePosition)
        {
            var pt1 = PrepareMousePosition(new Point(firstMouseDown.X, firstMouseDown.Y));
            var pt2 = PrepareMousePosition(new Point(mousePosition.X, mousePosition.Y));
            using (var pen = new Pen(Color.Black, 1) { DashStyle = DashStyle.Dash })
            {
                graphics.DrawLine(pen, pt1, pt2);
            }
        }

        /// <summary>
        /// Рисуем резиновый прямоугольник выбора
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="firstMouseDown"></param>
        /// <param name="mousePosition"></param>
        private void DrawRibbonSelectionRect(Graphics graphics, Point firstMouseDown, Point mousePosition)
        {
            var rect = new Rectangle(Math.Min(firstMouseDown.X, mousePosition.X), Math.Min(firstMouseDown.Y, mousePosition.Y),
                Math.Abs(firstMouseDown.X - mousePosition.X), Math.Abs(firstMouseDown.Y - mousePosition.Y));
            var color = firstMouseDown.X > mousePosition.X && firstMouseDown.Y > mousePosition.Y
                ? Color.Green : Color.Blue;
            using (var brush = new SolidBrush(Color.FromArgb(50, color)))
                graphics.FillRectangle(brush, PrepareRect(rect));
            using (var pen = new Pen(color, 0))
                graphics.DrawRectangle(pen, PrepareRect(rect));
        }

        private Rectangle PrepareRect(Rectangle rectangle)
        {
            var pt1 = PrepareMousePosition(rectangle.Location);
            var size = rectangle.Size;
            var pt2 = PrepareMousePosition(Point.Add(rectangle.Location, size));
            return Rectangle.Ceiling(new RectangleF(pt1, new SizeF(pt2.X - pt1.X, pt2.Y - pt1.Y)));
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
        private void DrawDefaultCursor(Graphics graphics, Point mousePosition)
        {
            var pt1 = PrepareMousePosition(new Point(0, mousePosition.Y));
            var pt2 = PrepareMousePosition(new Point(zoomPad.Width, mousePosition.Y));
            var pt3 = PrepareMousePosition(new Point(mousePosition.X, 0));
            var pt4 = PrepareMousePosition(new Point(mousePosition.X, zoomPad.Height));
            using (var pen = new Pen(Color.Black, 0) { DashStyle = DashStyle.Dash })
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
                    firstMouseDown = mousePosition;
                    mouseClickCount++;
                }
                else if (mouseClickCount == 1) // это второе нажатие
                {
                    switch (editorMode)
                    {
                        case EditorMode.Selection:
                            var selMode = firstMouseDown.X > mousePosition.X && firstMouseDown.Y > mousePosition.Y;
                            var rect = new Rectangle(Math.Min(firstMouseDown.X, mousePosition.X), Math.Min(firstMouseDown.Y, mousePosition.Y),
                                Math.Abs(firstMouseDown.X - mousePosition.X), Math.Abs(firstMouseDown.Y - mousePosition.Y));
                            // при отсутвии других режимов - режим выбора, и второе нажатие
                            // сбрасывает количество нажатий
                            mouseClickCount = 0;
                            // здесь будет обработка при выборе объектов
                            // при selMode == true выбираются все объекты, хотя бы частично попавшие в rect, а при false - только целиком
                            break;
                        case EditorMode.BuildLines:
                            mouseClickCount = 0;
                            firstMouseDown = mousePosition;
                            mouseClickCount++;
                            break;
                        case EditorMode.BuildRectangle:
                            //SetMode(EditorMode.Selection);
                            mouseClickCount = 0;
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
    }
}
