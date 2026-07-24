using PetProj.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace PetProj.Common
{
    public static class IntersectHelper
    {
        //public static bool Contains(this Figure figure, RectangleF rect)
        //{
        //    return figure.Contains(rect);
        //}

        /// <summary>
        /// Контрур фигуры содержит искомую точку
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Contains(this Figure figure, PointF point, float kf)
        {
            using (var path = figure.GetRendererPath())
            {
                using (var pen = new Pen(Color.Black, 2f * kf))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    return path.IsOutlineVisible(point, pen);
                }
            }
        }

        /// <summary>
        /// Фигура полностью содержится внутри прямоугольной области
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="captured">Область выбора</param>
        /// <returns></returns>
        public static bool Contains(this Figure figure, RectangleF captured)
        {
            using (var path = figure.GetRendererPath())
            {
                var figrect = path.GetBounds();
                figrect.Intersect(captured);
                return figrect.Equals(path.GetBounds());
            }
        }

        /// <summary>
        /// Фигура полностью или частично содержится внутри прямоугольной области
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="captured">Рамка выбора</param>
        /// <returns></returns>
        public static bool Intersects(this Figure figure, RectangleF captured)
        {
            using (var path = figure.GetRendererPath())
            {
                var rect = path.GetBounds();
                rect.Intersect(captured);
                if (rect.Equals(path.GetBounds()))
                    return true; // содержится целиком
                if (path.PathPoints.Any(p => captured.Contains(p)))
                    return true; // любая точка пути фигуры содержится в рамке
                // проверка частичного попадания, контур фигуры должен пересекаться с контуром рамки выбора
                rect = path.GetBounds();
                // если фигура не имеет ширины или длины, то задаём отсутсвующее измерение
                if (rect.Width > 0 && rect.Height == 0) rect.Height = 1;
                else if (rect.Width == 0 && rect.Height > 0) rect.Width = 1;
                rect.Intersect(captured);
                if (rect.IsEmpty)
                    return false; // не будет пересечений, т.к. области рамки и фигуры не пересекаются
                List<Point> framePoints = GetFramePoints(rect, captured);
                List<Point> figPoints = GetPathPoints(rect, path);
                var commonPoints = framePoints.Intersect(figPoints).ToList();
                return commonPoints.Count() > 0;
            }
        }

        /// <summary>
        /// Получение списка точек графического пути
        /// </summary>
        /// <param name="area">Область отсечения</param>
        /// <param name="path">Графический путь</param>
        /// <returns></returns>
        private static List<Point> GetPathPoints(RectangleF area, GraphicsPath path)
        {
            List<Point> points = new List<Point>();
            using (var image = new Bitmap((int)Math.Ceiling(area.Width + 1), (int)Math.Ceiling(area.Height + 1)))
            {
                using (var gr = Graphics.FromImage(image))
                {
                    gr.TranslateTransform(-area.X, -area.Y);
                    gr.DrawPath(Pens.Red, path);
                }
                using (var wr = new ImageWrapper(image))
                {
                    foreach (var point in wr)
                    {
                        var px = wr[point.X, point.Y];
                        if (px.R > 0)
                            points.Add(point);
                    }
                }
            }
            return points;
        }

        /// <summary>
        /// Получение списка точек рамки выбора (фрейма)
        /// </summary>
        /// <param name="area">Область отсечения</param>
        /// <param name="frame">Рамка выбора</param>
        /// <returns></returns>
        private static List<Point> GetFramePoints(RectangleF area, RectangleF frame)
        {
            var points = new List<Point>();
            using (var image = new Bitmap((int)Math.Ceiling(area.Width + 1), (int)Math.Ceiling(area.Height + 1)))
            {
                using (var gr = Graphics.FromImage(image))
                {
                    gr.TranslateTransform(-area.X, -area.Y);
                    gr.DrawRectangles(Pens.Blue, new RectangleF[] { frame });
                }
                using (var wr = new ImageWrapper(image))
                {
                    foreach (var point in wr)
                    {
                        var px = wr[point.X, point.Y];
                        if (px.B > 0)
                            points.Add(point);
                    }
                }
            }
            return points;
        }
    }
}
