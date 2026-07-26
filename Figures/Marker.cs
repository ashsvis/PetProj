using System.Drawing;
using System.Windows.Forms;

namespace PetProj.Figures
{
    public class Marker
    {
        protected const int MARKER_SIZE = 8;

        public Cursor Cursor;
        public PointF Position { get; set; }
        public PointF AnchorPosition { get; set; }
        public MarkerType MarkerType { get; set; }

        public virtual void Render(Graphics graphics, Color color, float zoomScale)
        {
            var rect = new RectangleF(-MARKER_SIZE / 2f / zoomScale, -MARKER_SIZE / 2f / zoomScale, MARKER_SIZE / zoomScale, MARKER_SIZE / zoomScale);
            rect.Offset(Position);
            using (var pen = new Pen(color, 1f / zoomScale))
            {
                graphics.DrawLines(pen, new PointF[] { new PointF(rect.X, rect.Y), new PointF(rect.X + rect.Width, rect.Y + rect.Height) });
                graphics.DrawLines(pen, new PointF[] { new PointF(rect.X + rect.Width, rect.Y), new PointF(rect.X, rect.Y + rect.Height) });
            }
        }
    }
}
