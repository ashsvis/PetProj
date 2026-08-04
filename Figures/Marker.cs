using PetProj.Common;
using System.Drawing;
using System.Windows.Forms;

namespace PetProj.Figures
{
    public class Marker
    {
        protected const int MARKER_SIZE = 8;

        public virtual Cursor Cursor { get; set; }
        public PointF Position { get; set; }
        public PointF AnchorPosition { get; set; }
        public MarkerType MarkerType { get; set; }
        public Figure Owner { get; set; }

        /// <summary>
        /// Допустимые операции над геометрией
        /// </summary>
        public virtual AllowedMarkerOperations AllowedOperations { get; }

        public RectangleF Target(float zoomScale)
        {
            var rect = new RectangleF(-MARKER_SIZE / 2f / zoomScale, -MARKER_SIZE / 2f / zoomScale, MARKER_SIZE / zoomScale, MARKER_SIZE / zoomScale);
            rect.Offset(Position);
            return rect;
        }

        public virtual void Render(Graphics graphics, Color color, float zoomScale)
        {
            var rect = new RectangleF(-MARKER_SIZE / 2f / zoomScale, -MARKER_SIZE / 2f / zoomScale, MARKER_SIZE / zoomScale, MARKER_SIZE / zoomScale);
            rect.Offset(Position);
            using (var brush = new SolidBrush(color))
            {
                graphics.FillRectangle(brush, rect);
            }
        }
    }
}
