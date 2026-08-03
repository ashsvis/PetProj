using System.Drawing;

namespace PetProj.Figures
{
    public class BindingQudrantMarker : Marker
    {
        public override void Render(Graphics graphics, Color color, float zoomScale)
        {
            var rect = new RectangleF(-MARKER_SIZE / 2f / zoomScale, -MARKER_SIZE / 2f / zoomScale, MARKER_SIZE / zoomScale, MARKER_SIZE / zoomScale);
            rect.Inflate(1f / zoomScale, 1f / zoomScale);
            rect.Offset(Position);
            using (var pen = new Pen(color, 1f / zoomScale))
            {
                graphics.DrawLine(pen, new PointF(rect.X + rect.Width / 2f, rect.Y), new PointF(rect.X + rect.Width, rect.Y + rect.Height / 2f));
                graphics.DrawLine(pen, new PointF(rect.X + rect.Width, rect.Y + rect.Height / 2f), new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height));
                graphics.DrawLine(pen, new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height), new PointF(rect.X, rect.Y + rect.Height / 2f));
                graphics.DrawLine(pen, new PointF(rect.X, rect.Y + rect.Height / 2f), new PointF(rect.X + rect.Width / 2f, rect.Y));
            }
        }
    }
}
