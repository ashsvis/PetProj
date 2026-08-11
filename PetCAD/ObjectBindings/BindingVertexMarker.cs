using PetCAD.Figures;
using System.Drawing;

namespace PetCAD.ObjectBindings
{ 
    public class BindingVertexMarker : Marker
    {
        public int Index { get; set; }

        public override void Render(Graphics graphics, Color color, float zoomScale)
        {
            var rect = new RectangleF(-MARKER_SIZE / 2f / zoomScale, -MARKER_SIZE / 2f / zoomScale, MARKER_SIZE / zoomScale, MARKER_SIZE / zoomScale);
            rect.Offset(Position);
            using (var pen = new Pen(color, 1f / zoomScale))
            {
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
            }
        }
    }
}
