using System;
using System.Drawing;

namespace PetCAD.Renderers
{
    public static class SegmentIntersection
    {
        static public bool Intersection(PointF A, PointF B, PointF C, PointF D, out PointF found)
        {
            double xo = A.X, yo = A.Y;
            double p = B.X - A.X, q = B.Y - A.Y;

            double x1 = C.X, y1 = C.Y;
            double p1 = D.X - C.X, q1 = D.Y - C.Y;

            double x = (xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                (q * p1 - q1 * p);
            double y = (yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                (p * q1 - p1 * q);

            found = new PointF((float)x, (float)y);

            if (double.IsInfinity(x) || double.IsNaN(x) ||
                double.IsInfinity(y) || double.IsNaN(y)) return false;
            //var point = FindIntersection(
            //    new Segment(new Point(A.X, A.Y), new Point(B.X, B.Y)), 
            //    new Segment(new Point(C.X, C.Y), new Point(D.X, D.Y)));
            //if (point != null )
            //{
            //    found = new PointF((float)point.X, (float)point.Y);
            //    return true;
            //}
            return true;
        }
        
        class Point
        {
            public double X { get; }
            public double Y { get; }

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        class Segment
        {
            public Point Start { get; }
            public Point End { get; }

            public Segment(Point start, Point end)
            {
                Start = start;
                End = end;
            }
        }
        
        private static Point FindIntersection(Segment s1, Segment s2)
        {
            // Проверка на параллельность (угловые коэффициенты)
            double m1 = (s1.End.Y - s1.Start.Y) / (s1.End.X - s1.Start.X);
            double m2 = (s2.End.Y - s2.Start.Y) / (s2.End.X - s2.Start.X);

            if (Math.Abs(m1 - m2) < double.Epsilon) // Прямые параллельны
            {
                // Проверка на совпадение отрезков
                if (IsCollinear(s1, s2))
                {
                    // Пересечение — это проекция на ось X (или Y, так как они коллинеарны)
                    double maxStart = Math.Max(s1.Start.X, s2.Start.X);
                    double minEnd = Math.Min(s1.End.X, s2.End.X);
                    if (maxStart <= minEnd)
                    {
                        Point intersection = new Point(maxStart, s1.Start.Y);
                        return intersection;
                    }
                }
                // Отрезки не пересекаются
                return null;
            }

            // Нахождение точки пересечения прямых
            double x = (m2 - m1) == 0 ? 0 : (s2.Start.Y - s1.Start.Y) / (m1 - m2);
            double y = m1 * x + s1.Start.Y;

            // Проверка принадлежности точки обоим отрезкам
            bool onSegment1 = (x >= Math.Min(s1.Start.X, s1.End.X) && x <= Math.Max(s1.Start.X, s1.End.X)) &&
                              (y >= Math.Min(s1.Start.Y, s1.End.Y) && y <= Math.Max(s1.Start.Y, s1.End.Y));

            if (onSegment1)
            {
                // Проверка для второго отрезка (можно оптимизировать, проверив проекцию на X)
                x = (m2 - m1) == 0 ? 0 : (s2.Start.Y - s1.Start.Y) / (m1 - m2);
                y = m1 * x + s1.Start.Y;
                bool onSegment2 = (x >= Math.Min(s2.Start.X, s2.End.X) && x <= Math.Max(s2.Start.X, s2.End.X)) &&
                                  (y >= Math.Min(s2.Start.Y, s2.End.Y) && y <= Math.Max(s2.Start.Y, s2.End.Y));

                if (onSegment2)
                {
                    return new Point(x, y);
                }
            }

            return null; // Точка пересечения не принадлежит обоим отрезкам
        }

        // Вспомогательная функция для проверки коллинеарности
        private static bool IsCollinear(Segment s1, Segment s2)
        {
            // Простая проверка на совпадение направляющих векторов
            return Math.Abs(s1.End.X - s2.Start.X) < double.Epsilon &&
                   Math.Abs(s1.End.Y - s2.Start.Y) < double.Epsilon;
        }
    }
}
