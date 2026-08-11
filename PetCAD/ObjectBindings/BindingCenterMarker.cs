using PetCAD.Figures;
using System.Drawing;

namespace PetCAD.ObjectBindings
{
    public class BindingCenterMarker : Marker
    {
        public override void Render(Graphics graphics, Color color, float zoomScale)
        {
            var rect = new RectangleF(-MARKER_SIZE / 2f / zoomScale, -MARKER_SIZE / 2f / zoomScale, MARKER_SIZE / zoomScale, MARKER_SIZE / zoomScale);
            rect.Inflate(1f / zoomScale, 1f / zoomScale);
            rect.Offset(Position);
            using (var pen = new Pen(color, 1f / zoomScale))
            {
                graphics.DrawEllipse(pen, rect);
            }
        }
    }
}
