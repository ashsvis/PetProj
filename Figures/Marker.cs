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
            using (var brush = new SolidBrush(Color.Blue))
            {
                graphics.FillRectangle(brush, rect);
            }
        }
    }
}
