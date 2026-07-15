using System.Drawing;
using System.Reflection;

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
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    graphics.DrawLine(pen, pt1, pt2);
                }
            }
        }

        public PointF StartPoint => Origin;
        public PointF EndPoint => Offsets.Count == 1 ? PointF.Add(Origin, Offsets[0]) : Origin;

        public void MoveStartPointTo(PointF point)
        {
            var endPoint = EndPoint;
            Origin = point;
            var sz = new SizeF(endPoint.X - Origin.X, endPoint.Y - Origin.Y);
            if (Offsets.Count == 1)
                Offsets[0] = sz;
            else
                Offsets.Add(sz);
        }

        public void MoveEndPointTo(PointF point)
        {
            var sz = new SizeF(point.X - Origin.X, point.Y - Origin.Y);
            if (Offsets.Count == 1)
                Offsets[0] = sz;
            else 
                Offsets.Add(sz);
        }
    }
}
