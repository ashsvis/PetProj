using System.Drawing;

namespace PetProj
{
    public class Line : Primitive
    {
        public Line(PointF point1, PointF point2) 
        { 
            Origin = point1;
            Offsets.Add(new SizeF(point2.X - point1.X, point2.Y - point1.Y));
        }

        public override void DrawAt(Graphics graphics, Color forecolor)
        {
            if (Offsets.Count == 1)
            {
                var pt1 = Origin;
                var pt2 = PointF.Add(pt1, Offsets[0]);
                using (var pen = new Pen(forecolor))
                {
                    graphics.DrawLine(pen, pt1, pt2);
                }
            }
        }
    }
}
