using System.Drawing;

namespace PetProj
{
    public class AnchorPoint : Primitive
    {
        public AnchorPoint(PointF point)
        {
            Origin = point;
        }

        public override void DrawAt(Graphics graphics, Color forecolor)
        {
            // stub
        }
    }
}
