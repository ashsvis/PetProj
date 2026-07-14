using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PetProj
{
    public partial class DrawControl : UserControl
    {
        private Point firstMouseDown;
        private Point mousePosition;

        public DrawControl()
        {
            InitializeComponent();
        }

        private void zoomPad_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;
            DrawDefaultCursor(graphics, mousePosition);
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
            mousePosition = firstMouseDown = e.Location;
            zoomPad.Invalidate();
        }

        private void zoomPad_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            zoomPad.Invalidate();
        }

        private void zoomPad_MouseUp(object sender, MouseEventArgs e)
        {
            mousePosition = firstMouseDown = e.Location;
            zoomPad.Invalidate();
        }
    }
}
