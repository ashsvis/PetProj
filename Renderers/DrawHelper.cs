using PetProj.Controllers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace PetProj.Renderers
{
    public static class DrawHelper
    {
        /// <summary>
        /// Рисуем курсор-перекрестье на всё окно
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="mousePosition"></param>
        public static void DrawDefaultCursor(this DrawControl drawControl, Graphics graphics, PointF mousePosition)
        {
            var pt1 = drawControl.PrepareMousePosition(new PointF(0, mousePosition.Y));
            var pt2 = drawControl.PrepareMousePosition(new PointF(drawControl.ClientSize.Width, mousePosition.Y));
            var pt3 = drawControl.PrepareMousePosition(new PointF(mousePosition.X, 0));
            var pt4 = drawControl.PrepareMousePosition(new PointF(mousePosition.X, drawControl.ClientSize.Height));
            using (var pen = new Pen(Color.FromArgb(50, Color.Black), 0))
            {
                graphics.DrawLine(pen, pt1, pt2);
                graphics.DrawLine(pen, pt3, pt4);
            }
        }

        /// <summary>
        /// Рисуем глобальную точку нуля (левый верхний угол)
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="color"></param>
        /// <param name="zoomScale"></param>
        public static void DrawZeroOrigin(this DrawControl drawControl, Graphics graphics, Color color)
        {
            float zoom = drawControl.Zoom;
            using (var pen = new Pen(color, (float)(2f / zoom)))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                var gs = graphics.Save();
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                using (var penX = new Pen(Color.FromArgb(80, Color.LightSalmon), (float)(2f / zoom)))
                {
                    graphics.DrawLine(penX, new PointF(0f, 0f), drawControl.PrepareMousePositionX(new PointF(drawControl.ClientSize.Width, 0f)));
                }
                using (var penY = new Pen(Color.FromArgb(80, Color.LightGreen), (float)(2f / zoom)))
                {
                    graphics.DrawLine(penY, new PointF(0f, 0f), drawControl.PrepareMousePositionY(new PointF(0f, drawControl.ClientSize.Height)));
                }
                graphics.DrawLine(pen, new PointF(0f, 0f), new PointF(50f / zoom, 0f));
                graphics.DrawLine(pen, new PointF(0f, 0f), new PointF(0f, 50f / zoom));
                var rect = new RectangleF(-4f / zoom, -4f / zoom, 8f / zoom, 8f / zoom);
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
                using (var font = new Font("Arial", 10f / zoom))
                using (var brush = new SolidBrush(color))
                {
                    graphics.DrawString("X", font, brush, new PointF(50f / zoom, 0f));
                    graphics.DrawString("Y", font, brush, new PointF(0f, 50f / zoom));
                }
                graphics.Restore(gs);
            }
        }

        public static PointF PrepareMousePositionX(this DrawControl drawControl, PointF p)
        {
            float zoom = drawControl.Zoom;
            var origin = drawControl.Origin;
            PointF[] arr = new PointF[] { p };
            Matrix matrix = new Matrix();

            matrix.Translate(origin.X, 0);
            matrix.Scale(1 / zoom, 1);
            matrix.TransformPoints(arr);
            matrix.Dispose();
            return new PointF(arr[0].X, arr[0].Y);
        }

        public static PointF PrepareMousePositionY(this DrawControl drawControl, PointF p)
        {
            float zoom = drawControl.Zoom;
            var origin = drawControl.Origin;
            PointF[] arr = new PointF[] { p };
            Matrix matrix = new Matrix();

            matrix.Translate(0, origin.Y);
            matrix.Scale(1, 1 / zoom);
            matrix.TransformPoints(arr);
            matrix.Dispose();
            return new PointF(arr[0].X, arr[0].Y);
        }

        /// <summary>
        /// Рисуем линию, соединющую точки начала и конца перемещения
        /// Также рисуются перемещаемые фигуры
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="firstMouseDown"></param>
        /// <param name="mousePosition"></param>
        public static void DrawRibbonMoved(this DrawControl drawControl, Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            float zoom = drawControl.Zoom;
            var pt1 = firstMouseDown;
            var pt2 = drawControl.PrepareMousePosition(mousePosition);
            //поиск ортогональной точки, если включен режим ортогонального построения
            pt2 = drawControl.FindOrthoPoint(pt2);
            using (var pen = new Pen(Color.Gray, (float)(2.6f / zoom)) { DashStyle = DashStyle.Dash })
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                var state = graphics.Save();

                graphics.TranslateTransform(pt2.X - pt1.X, pt2.Y - pt1.Y);
                // отрисовка выделения
                drawControl.SelectionController.Selection.Render(graphics, Color.LightPink, (float)zoom);
                graphics.TranslateTransform(-pt2.X + pt1.X, -pt2.Y + pt1.Y);

                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                graphics.DrawLine(pen, pt1, pt2);
                graphics.Restore(state);
            }
        }

        public static PointF FindOrthoPoint(this DrawControl drawControl, PointF pt2)
        {
            if (drawControl.IsDrawOrthoMode)
            {
                pt2 = drawControl.PrepareMousePosition(drawControl.CurrentMousePosition);
                var dx = Math.Abs(drawControl.FirstMouseDown.X - pt2.X);
                var dy = Math.Abs(drawControl.FirstMouseDown.Y - pt2.Y);
                if (dx < dy)
                    pt2.X = drawControl.FirstMouseDown.X;
                else
                    pt2.Y = drawControl.FirstMouseDown.Y;
            }
            return pt2;
        }

        public static PointF FindBindingPoint(this DrawControl drawControl, PointF point)
        {
            if (drawControl.IsObjectBinding)
            {
                var query = drawControl.SelectionController.BindingMarkers.Select(marker => (marker,
                             $"{Math.Abs(marker.Position.X - point.X):00000}{Math.Abs(marker.Position.Y - point.Y):00000}")).OrderBy(x => x.Item2);
                if (query.Count() > 0)
                    // принимаем позицию ближайшего маркера привязки к текущему курсору
                    point = query.First().marker.Position;
            }
            return point;
        }

        /// <summary>
        /// Рисуем резиновый прямоугольник выбора
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="firstMouseDown"></param>
        /// <param name="mousePosition"></param>
        public static void DrawRibbonSelectionRect(this DrawControl drawControl, Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = drawControl.PrepareMousePosition(mousePosition);
            var rect = new RectangleF(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y),
                Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));
            var color = pt1.X > pt2.X ? Color.Green : Color.Blue;
            using (var brush = new SolidBrush(Color.FromArgb(50, color)))
                graphics.FillRectangle(brush, rect);
            using (var pen = new Pen(Color.Black, 0))
            {
                if (pt1.X > pt2.X) pen.DashStyle = DashStyle.Dash;
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
            }
        }

        /// <summary>
        /// Рисуем прямоугольник при построении
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="firstMouseDown"></param>
        /// <param name="mousePosition"></param>
        public static void DrawRibbonRectangle(this DrawControl drawControl, Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            float zoom = drawControl.Zoom;
            var pt1 = firstMouseDown;
            var pt2 = drawControl.PrepareMousePosition(mousePosition);
            var rect = new RectangleF(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y),
                Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));
            using (var pen = new Pen(Color.LightPink, (float)(2.6f / zoom)))
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
            if (drawControl.MouseClickCount == 1)
            {
                if (drawControl.IsDynamicalEnter)
                {
                    using (var pen = new Pen(Color.Gray, 0) { DashStyle = DashStyle.Dot })
                    {
                        var kf = (float)(50f / zoom);
                        var p1 = pt1;
                        var p2 = pt2;
                        if (pt2.X > pt1.X)
                        {
                            drawControl.DrawSizeLine(graphics, pen, new PointF(p1.X, p2.Y), new PointF(p2.X, p2.Y), kf, pt2.Y > pt1.Y);
                            drawControl.DrawSizeLine(graphics, pen, new PointF(p2.X, p1.Y), new PointF(p2.X, p2.Y), kf, pt2.Y < pt1.Y);
                        }
                        else
                        {
                            drawControl.DrawSizeLine(graphics, pen, new PointF(p1.X, p2.Y), new PointF(p2.X, p2.Y), kf, pt2.Y < pt1.Y);
                            drawControl.DrawSizeLine(graphics, pen, new PointF(p2.X, p1.Y), new PointF(p2.X, p2.Y), kf, pt2.Y > pt1.Y);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Рисуем отрезок при построении
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="firstMouseDown"></param>
        /// <param name="mousePosition"></param>
        public static void DrawRibbonLine(this DrawControl drawControl, Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            float zoom = drawControl.Zoom;
            var pt1 = firstMouseDown;
            var pt2 = drawControl.PrepareMousePosition(mousePosition);
            //поиск ортогональной точки, если включен режим ортогонального построения
            pt2 = drawControl.FindOrthoPoint(pt2);
            using (var pen = new Pen(Color.LightPink, (float)(2.6f / zoom)))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                graphics.DrawLine(pen, pt1, pt2);
            }
            if (drawControl.MouseClickCount == 1)
            {
                if (drawControl.IsDynamicalEnter)
                {
                    using (var pen = new Pen(Color.Gray, 0) { DashStyle = DashStyle.Dot })
                    {
                        drawControl.DrawSizeLine(graphics, pen, pt1, pt2, (float)(50f / zoom)); // Выноска размера 50 пикселей
                        drawControl.DrawAngleLine(graphics, pen, pt1, pt2);
                    }
                }
            }
        }

        /// <summary>
        /// Рисуем дугу, показывающую угол наклона отрезка к горизонтали
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void DrawAngleLine(this DrawControl drawControl, Graphics graphics, Pen pen, PointF start, PointF end)
        {
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            if (length == 0)
            {
                // Отрезок вырожден в точку
                return;
            }
            // выносная линия, горизонтальная
            var b1 = PointF.Add(start, new SizeF(length, 0));
            graphics.DrawLine(pen, start, b1);
            var arcrect = new RectangleF(start.X - length, start.Y - length, length * 2, length * 2);
            var angle = Math.Atan2(dy, dx);
            var cx = start.X + length * Math.Cos(angle / 2);
            var cy = start.Y + length * Math.Sin(angle / 2);
            var mid = new PointF((float)cx, (float)cy);
            var angleDegree = (float)(angle * 180 / Math.PI);
            var L = Math.PI * length * Math.Abs(angleDegree) / 180;
            if (L < 35)
                mid = new PointF(end.X + 5, end.Y + 25);
            var sarc = $"{angleDegree}°";
            try
            {
                graphics.DrawArc(pen, arcrect, 0, angleDegree);
                drawControl.DrawTextAtCenter(graphics, pen, Brushes.White, mid, sarc);
            }
            catch { }
        }

        /// <summary>
        /// Рисуем размерную линию для отрезка
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="halfLength"></param>
        public static void DrawSizeLine(this DrawControl drawControl, Graphics graphics, Pen pen, PointF start, PointF end, float halfLength, bool? side = null)
        {
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float px = dy;
            float py = -dx;
            float length = (float)Math.Sqrt(px * px + py * py);
            if (length == 0)
            {
                // Отрезок вырожден в точку
                return;
            }
            px /= length;
            py /= length;

            bool kind = side ?? px > 0;
            // перпендикуляр в начале отрезка
            PointF df = kind ? new PointF(start.X, start.Y) : new PointF(start.X + px * halfLength, start.Y + py * halfLength);
            PointF ef = !kind ? new PointF(start.X, start.Y) : new PointF(start.X - px * halfLength, start.Y - py * halfLength);
            graphics.DrawLine(pen, df, ef);
            // перпендикуляр в конце отрезка
            PointF de = kind ? new PointF(end.X, end.Y) : new PointF(end.X + px * halfLength, end.Y + py * halfLength);
            PointF ee = !kind ? new PointF(end.X, end.Y) : new PointF(end.X - px * halfLength, end.Y - py * halfLength);
            graphics.DrawLine(pen, de, ee);
            // выносная линия, соединяющая два перпендикуляра
            graphics.DrawLine(pen, kind ? ef : df, kind ? ee : de);
            PointF mid = kind ? new PointF((ef.X + ee.X) / 2, (ef.Y + ee.Y) / 2) : new PointF((df.X + de.X) / 2, (df.Y + de.Y) / 2);
            var slength = $"{length}";
            drawControl.DrawTextAtCenter(graphics, pen, Brushes.White, mid, slength);
        }

        /// <summary>
        /// Рисуем текст с привязкой к середине
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="background"></param>
        /// <param name="mid"></param>
        /// <param name="text"></param>
        public static void DrawTextAtCenter(this DrawControl drawControl, Graphics graphics, Pen pen, Brush background, PointF mid, string text)
        {
            float zoom = drawControl.Zoom;
            using (var font = new Font("Segoe UI", (float)(10f / zoom)))
            {
                var ms = graphics.MeasureString(text, font);
                var rect = new RectangleF(mid.X - ms.Width / 2, mid.Y - ms.Height / 2, ms.Width, ms.Height);
                graphics.FillRectangles(background, new RectangleF[] { rect });
                graphics.DrawRectangles(pen, new RectangleF[] { rect });
                using (var brush = new SolidBrush(Color.Black))
                    graphics.DrawString(text, font, brush, rect);
            }
        }
    }
}
