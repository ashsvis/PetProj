using System.Drawing;

namespace PetCAD.Renderers
{
    public static class SegmentIntersection
    {
        static public PointF Intersection(PointF A, PointF B, PointF C, PointF D)
        {
            double xo = A.X, yo = A.Y;
            double p = B.X - A.X, q = B.Y - A.Y;

            double x1 = C.X, y1 = C.Y;
            double p1 = D.X - C.X, q1 = D.Y - C.Y;

            double x = (xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                (q * p1 - q1 * p);
            double y = (yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                (p * q1 - p1 * q);

            return new PointF((float)x, (float)y);
        }
    }
}
