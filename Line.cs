using System.Drawing;
using System.Drawing.Drawing2D;

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
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.DrawLine(pen, pt1, pt2);
                }
            }
        }

        public override void DrawHighlightAt(Graphics graphics, Color forecolor)
        {
            if (Offsets.Count == 1)
            {
                var pt1 = Origin;
                var pt2 = PointF.Add(pt1, Offsets[0]);
                using (var pen = new Pen(Color.FromArgb(80, forecolor), 2))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.DrawLine(pen, pt1, pt2);
                }
                using (var pen = new Pen(forecolor))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.DrawLine(pen, pt1, pt2);
                }
            }
        }

        public override bool Contains(PointF point)
        {
            if (Offsets.Count == 1)
            {
                var pt1 = Origin;
                var pt2 = PointF.Add(pt1, Offsets[0]);
                using (var path = new GraphicsPath())
                {
                    path.AddLine(pt1, pt2);
                    using (var pen = new Pen(Color.Black, 3))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        return path.IsOutlineVisible(point, pen);
                    }
                }
            }
            return false;
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
