using PetCAD.Common;
using PetCAD.Controllers;
using PetCAD.Figures;
using PetCAD.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace PetCAD.Renderers
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
                    graphics.DrawLine(penX, new PointF(0f, 0f), drawControl.PrepareMousePositionX(new PointF(drawControl.ClientSize.Width, 0f)));
                using (var penY = new Pen(Color.FromArgb(80, Color.LightGreen), (float)(2f / zoom)))
                    graphics.DrawLine(penY, new PointF(0f, 0f), drawControl.PrepareMousePositionY(new PointF(0f, drawControl.ClientSize.Height)));
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

        public static void DrawRibbonRotated(this DrawControl drawControl, Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            float zoom = drawControl.Zoom;
            var pt1 = firstMouseDown;
            var pt2 = drawControl.PrepareMousePosition(mousePosition);
            var dx = pt1.X - pt2.X;
            var dy = pt1.Y - pt2.Y;
            var d = dx * dx + dy * dy;
            if (d > 0.01)
            {
                using (var pen = new Pen(Color.Gray, (float)(2.6f / zoom)) { DashStyle = DashStyle.Dash })
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    //var max = drawControl.SelectionController.Selection.Select(x =>
                    //    Math.Max(x.Geometry.Bounds.Width, x.Geometry.Bounds.Height)).Max();
                    //var kf = d / max;
                    using (var penA = new Pen(Color.LightPink, 2.6f / zoom))
                    using (var penB = new Pen(Color.Silver, 2.6f / zoom))
                    {
                        foreach (var figure in drawControl.SelectionController.Selection)
                        {
                            var fig = figure.DeepCopy();
                            // рисуем текущее представление цветом Silver
                            if (figure.Geometry is IRotateGeometry prevGeometry &&
                                figure.Geometry is BlockGeometry blockGeometry)
                            {
                                prevGeometry.Rotate(blockGeometry.InsertPoint, blockGeometry.Angle);
                                using (var path = figure.GetRendererPath())
                                {
                                    try
                                    {
                                        graphics.DrawPath(penB, path);
                                    }
                                    catch { }
                                }
                            }
                            // рисуем масштабируемое представление цветом LightPink
                            if (fig.Geometry is IRotateGeometry rotateGeometry)
                            {
                                //if (figure.Geometry is BlockGeometry blkGeometry)
                                //{
                                //    var arr = new PointF[] { pt1 };
                                //    var m = new Matrix();
                                //    var mx = blkGeometry.InsertPoint.X;
                                //    var my = blkGeometry.InsertPoint.Y;
                                //    m.Translate(-mx, -my, MatrixOrder.Append);
                                //    m.Scale(1f / blkGeometry.ScaleFactor, 1f / blkGeometry.ScaleFactor, MatrixOrder.Append);
                                //    m.Translate(mx, my, MatrixOrder.Append);
                                //    m.TransformPoints(arr);
                                //    pt1 = arr[0];
                                //}

                                rotateGeometry.Rotate(pt1, 0f);

                                //if (figure.Geometry is BlockGeometry blkGeom)
                                //    blkGeom.InsertPoint = PointF.Add(blkGeom.InsertPoint,
                                //        new SizeF(firstMouseDown.X - pt1.X, firstMouseDown.Y - pt1.Y));

                                using (var path = fig.GetRendererPath())
                                {
                                    try
                                    {
                                        graphics.DrawPath(penA, path);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    graphics.SmoothingMode = SmoothingMode.HighSpeed;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    graphics.DrawLine(pen, firstMouseDown, pt2);
                }
            }
        }

        public static void DrawRibbonScaled(this DrawControl drawControl, Graphics graphics, PointF firstMouseDown, PointF mousePosition)
        {
            float zoom = drawControl.Zoom;
            var pt1 = firstMouseDown;
            var pt2 = drawControl.PrepareMousePosition(mousePosition);
            var dx = pt1.X - pt2.X;
            var dy = pt1.Y - pt2.Y;
            var d = dx * dx + dy * dy;
            if (d > 0.01) 
            {
                using (var pen = new Pen(Color.Gray, (float)(2.6f / zoom)) { DashStyle = DashStyle.Dash })
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    var max = drawControl.SelectionController.Selection.Select(x => 
                        Math.Max(x.Geometry.Bounds.Width, x.Geometry.Bounds.Height)).Max();
                    var kf = d / max;
                    using (var penA = new Pen(Color.LightPink, 2.6f / zoom))
                    using (var penB = new Pen(Color.Silver, 2.6f / zoom))
                    {
                        foreach (var figure in drawControl.SelectionController.Selection)
                        {
                            var fig = figure.DeepCopy();
                            // рисуем текущее представление цветом Silver
                            if (figure.Geometry is IScaleGeometry prevGeometry &&
                                figure.Geometry is BlockGeometry blockGeometry)
                            {
                                prevGeometry.Scale(blockGeometry.InsertPoint, blockGeometry.ScaleFactor);
                                using (var path = figure.GetRendererPath())
                                {
                                    try
                                    {
                                        graphics.DrawPath(penB, path);
                                    }
                                    catch { }
                                }
                            }
                            // рисуем масштабируемое представление цветом LightPink
                            if (fig.Geometry is IScaleGeometry scaleGeometry)
                            {
                                if (figure.Geometry is BlockGeometry blkGeometry)
                                {
                                    var arr = new PointF[] { pt1 };
                                    var m = new Matrix();
                                    var mx = blkGeometry.InsertPoint.X;
                                    var my = blkGeometry.InsertPoint.Y;
                                    m.Translate(-mx, -my, MatrixOrder.Append);
                                    m.Scale(1f / blkGeometry.ScaleFactor, 1f / blkGeometry.ScaleFactor, MatrixOrder.Append);
                                    m.Translate(mx, my, MatrixOrder.Append);
                                    m.TransformPoints(arr);
                                    pt1 = arr[0];
                                }

                                scaleGeometry.Scale(pt1, kf);

                                if (figure.Geometry is BlockGeometry blkGeom)
                                    blkGeom.InsertPoint = PointF.Add(blkGeom.InsertPoint, 
                                        new SizeF(firstMouseDown.X - pt1.X, firstMouseDown.Y - pt1.Y));

                                using (var path = fig.GetRendererPath())
                                {
                                    try
                                    {
                                        graphics.DrawPath(penA, path);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    graphics.SmoothingMode = SmoothingMode.HighSpeed;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    graphics.DrawLine(pen, firstMouseDown, pt2);
                }
            }
        }

        public static bool CalcRibbonScaled(this DrawControl drawControl, PointF firstMouseDown, PointF mousePosition,
            out PointF baseScalePoint, out float scaleFactor)
        {
            var pt1 = firstMouseDown;
            baseScalePoint = pt1;
            var pt2 = drawControl.PrepareMousePosition(mousePosition);
            var dx = pt1.X - pt2.X;
            var dy = pt1.Y - pt2.Y;
            var d = dx * dx + dy * dy;
            if (d > 0.01)
            {
                var max = drawControl.SelectionController.Selection.Select(x =>
                    Math.Max(x.Geometry.Bounds.Width, x.Geometry.Bounds.Height)).Max();
                var kf = d / max;
                scaleFactor = kf;
                foreach (var figure in drawControl.SelectionController.Selection)
                {
                    var fig = figure.DeepCopy();
                    if (fig.Geometry is IScaleGeometry scaleGeometry)
                    {
                        if (figure.Geometry is BlockGeometry blkGeometry)
                        {
                            var arr = new PointF[] { pt1 };
                            var m = new Matrix();
                            var mx = blkGeometry.InsertPoint.X;
                            var my = blkGeometry.InsertPoint.Y;
                            m.Translate(-mx, -my, MatrixOrder.Append);
                            m.Scale(1f / blkGeometry.ScaleFactor, 1f / blkGeometry.ScaleFactor, MatrixOrder.Append);
                            m.Translate(mx, my, MatrixOrder.Append);
                            m.TransformPoints(arr);
                            pt1 = arr[0];
                            //baseScalePoint = pt1;
                            baseScalePoint = PointF.Add(blkGeometry.InsertPoint, new SizeF(firstMouseDown.X - pt1.X, firstMouseDown.Y - pt1.Y));
                        }
                        return true;
                    }
                }
            }
            baseScalePoint = firstMouseDown;
            scaleFactor = 1f;
            return false;
        }

        /// <summary>
        /// Рисование перемещения отрезка за середину
        /// </summary>
        /// <param name="drawControl"></param>
        /// <param name="graphics"></param>
        /// <param name="figures"></param>
        /// <param name="firstMouseDown"></param>
        /// <param name="mousePosition"></param>
        public static void DrawRibbonMovedFigures(this DrawControl drawControl, Graphics graphics, 
            IList<Figure> figures, PointF firstMouseDown, PointF mousePosition)
        {
            float zoom = drawControl.Zoom;
            var pt1 = firstMouseDown;
            var pt2 = drawControl.PrepareMousePosition(mousePosition);
            //поиск ортогональной точки, если включен режим ортогонального построения
            pt2 = drawControl.FindOrthoPoint(pt2);
            using (var pen = new Pen(Color.Gray, (float)(2.6f / zoom)) { DashStyle = DashStyle.Dash })
            using (var penA = new Pen(Color.LightPink, 2.6f / zoom))
            using (var penB = new Pen(Color.Silver, 2.6f / zoom))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                foreach (var figure in figures)
                {
                    // получаем путь для рисования методом фигуры
                    using (var path = figure.Geometry.Path)
                        graphics.DrawPath(penB, path);
                }

                var state = graphics.Save();

                graphics.TranslateTransform(pt2.X - pt1.X, pt2.Y - pt1.Y);
                // отрисовка выделения
                foreach (var figure in figures)
                {
                    // получаем путь для рисования методом фигуры
                    using (var path = figure.Geometry.Path)
                        graphics.DrawPath(penA, path);
                }
                graphics.TranslateTransform(-pt2.X + pt1.X, -pt2.Y + pt1.Y);

                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                graphics.DrawLine(pen, pt1, pt2);
                graphics.Restore(state);
            }
        }

        /// <summary>
        /// Рисование перемещения линии с маркером на конце
        /// </summary>
        /// <param name="drawControl"></param>
        /// <param name="graphics"></param>
        /// <param name="markers"></param>
        /// <param name="mousePosition"></param>
        /// 
        public static void DrawRibbonMovedMarkers(this DrawControl drawControl, Graphics graphics,
            IList<Marker> markers, PointF mousePosition)
        {
            float zoom = drawControl.Zoom;
            var pt = drawControl.PrepareMousePosition(mousePosition);
            //поиск ортогональной точки, если включен режим ортогонального построения
            pt = drawControl.FindOrthoPoint(pt);
            using (var pen = new Pen(Color.Silver, (float)(2.6f / zoom)))
            using (var penA = new Pen(Color.LightPink, 2.6f / zoom))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                // отрисовка выделения
                foreach (var marker in markers)
                {
                    if (marker.AllowedOperations.HasFlag(AllowedMarkerOperations.MoveVertex) &&
                        marker is VertexMarker vertex)
                    {
                        switch (marker.Owner.Geometry)
                        {
                            case LineGeometry segment:
                                {
                                    graphics.DrawLine(pen, segment.StartPoint, segment.EndPoint);
                                    var startPoint = vertex.Index == 0 ? segment.EndPoint : segment.StartPoint;
                                    graphics.DrawLine(penA, startPoint, pt);
                                    break;
                                }
                            case ArcGeometry arc:
                                drawControl.DrawArcByThreePoints(graphics, pen, arc.StartPoint, arc.MiddlePoint, arc.EndPoint);
                                drawControl.DrawArcByThreePoints(graphics, penA,
                                    vertex.Index == 1 ? pt : arc.StartPoint,
                                    vertex.Index == 2 ? pt : arc.MiddlePoint,
                                    vertex.Index == 3 ? pt : arc.EndPoint);
                                break;
                            case BlockGeometry block:
                                using (var path = marker.Owner.GetRendererPath())
                                {
                                    graphics.DrawPath(pen, path);
                                    var m = new Matrix();
                                    m.Translate(pt.X - block.InsertPoint.X, pt.Y - block.InsertPoint.Y);
                                    path.Transform(m);
                                    graphics.DrawPath(penA, path);
                                }
                                break;
                        }
                    }
                }
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

        public static PointF FindOrthoPoint(this DrawControl drawControl, PointF pt1, PointF pt2)
        {
            if (drawControl.IsDrawOrthoMode)
            {
                pt2 = drawControl.PrepareMousePosition(drawControl.CurrentMousePosition);
                var dx = Math.Abs(pt1.X - pt2.X);
                var dy = Math.Abs(pt1.Y - pt2.Y);
                if (dx < dy)
                    pt2.X = pt1.X;
                else
                    pt2.Y = pt1.Y;
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
            try
            {
                using (var brush = new SolidBrush(Color.FromArgb(50, color)))
                    graphics.FillRectangle(brush, rect);
                using (var pen = new Pen(Color.Black, 0))
                {
                    if (pt1.X > pt2.X) pen.DashStyle = DashStyle.Dash;
                    graphics.DrawRectangles(pen, new RectangleF[] { rect });
                }
            }
            catch { }
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
        public static void DrawRibbonLine(this DrawControl drawControl, Graphics graphics, Pen pen, 
            PointF firstMouseDown, PointF mousePosition, bool ortho = true)
        {
            float zoom = drawControl.Zoom;
            var pt1 = firstMouseDown;
            var pt2 = drawControl.PrepareMousePosition(mousePosition);
            if (ortho)
            {
                //поиск ортогональной точки, если включен режим ортогонального построения
                pt2 = drawControl.FindOrthoPoint(pt2);
            }
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            graphics.DrawLine(pen, pt1, pt2);
            if (drawControl.MouseClickCount == 1)
            {
                if (drawControl.IsDynamicalEnter)
                {
                    using (var dynpen = new Pen(Color.Gray, 0) { DashStyle = DashStyle.Dot })
                    {
                        drawControl.DrawSizeLine(graphics, dynpen, pt1, pt2, (float)(50f / zoom)); // Выноска размера 50 пикселей
                        drawControl.DrawAngleLine(graphics, dynpen, pt1, pt2);
                    }
                }
            }
            drawControl.ToolTipChanged($"Количество нажатий: {drawControl.MouseClickCount}");
        }

        /// <summary>
        /// Рисуем дугу при построении
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="firstMouseDown"></param>
        /// <param name="mousePosition"></param>
        public static void DrawRibbonArc(this DrawControl drawControl, Graphics graphics, Pen pen, 
            PointF firstMouseDown, PointF secondMouseDown, PointF mousePosition)
        {
            var pt1 = firstMouseDown;
            var pt2 = secondMouseDown;
            var pt3 = drawControl.PrepareMousePosition(mousePosition);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;

            switch (drawControl.EditorMode)
            {
                case EditorMode.BuildArcThreePoints:
                    drawControl.DrawArcByThreePoints(graphics, pen, pt1, pt2, pt3);
                    break;
                case EditorMode.BuildArcStartCenterEnd:
                    drawControl.DrawArcByStartCenterEnd(graphics, pen, pt1, pt2, pt3);
                    break;
                case EditorMode.BuildArcCenterStartEnd:
                    drawControl.DrawArcByCenterStartEnd(graphics, pen, pt1, pt2, pt3);
                    break;
            }
            if (drawControl.MouseClickCount == 2)
            {
                if (drawControl.IsDynamicalEnter)
                {
                    using (var dynpen = new Pen(Color.Gray, 0) { DashStyle = DashStyle.Dot })
                    {
                        if (drawControl.EditorMode == EditorMode.BuildArcCenterStartEnd)
                            drawControl.DrawAngleLine(graphics, dynpen, pt1, pt3);
                        else
                            drawControl.DrawAngleLine(graphics, dynpen, pt2, pt3);
                    }
                }
            }
            drawControl.ToolTipChanged($"Количество нажатий: {drawControl.MouseClickCount}");
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
        /// Рисуем дугу, проходящую через начало, с центром, к концу
        /// </summary>
        /// <param name="drawControl"></param>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="start"></param>
        /// <param name="center"></param>
        /// <param name="end"></param>
        public static void DrawArcByStartCenterEnd(this DrawControl drawControl, Graphics graphics, Pen pen, PointF start, PointF center, PointF end)
        {
            var r = center.Vector(start).Length();
            var rect = new RectangleF(center.X - r, center.Y - r, r * 2f, r * 2f);
            try
            {
                #region блок коррекции углов дуги
                var angle1 = start.Vector(center).AngleDegree(); if (angle1 < 0) angle1 = 360f + angle1;
                var angle2 = end.Vector(center).AngleDegree(); if (angle2 < 0) angle2 = 360f + angle2;
                if (angle2 < angle1) angle2 += 360f;
                var sweepAngle = angle2 - angle1; if (sweepAngle < 0) sweepAngle = 360f + sweepAngle;
                if (angle1 > angle2) sweepAngle = -360f + sweepAngle;
                #endregion блок коррекции углов дуги
                
                if (drawControl.CtrlPressed)    // разворот направления рисования
                    graphics.DrawArc(pen, rect, angle2, 360 - sweepAngle);
                else
                    graphics.DrawArc(pen, rect, angle1, sweepAngle);
            }
            catch { }
        }

        /// <summary>
        /// Рисуем дугу, с центром, проходящую из начала, к концу
        /// </summary>
        /// <param name="drawControl"></param>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="center"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void DrawArcByCenterStartEnd(this DrawControl drawControl, Graphics graphics, Pen pen, PointF center, PointF start, PointF end)
        {
            var r = center.Vector(start).Length();
            var rect = new RectangleF(center.X - r, center.Y - r, r * 2f, r * 2f);
            try
            {
                #region блок коррекции углов дуги
                var angle1 = start.Vector(center).AngleDegree(); if (angle1 < 0) angle1 = 360f + angle1;
                var angle2 = end.Vector(center).AngleDegree(); if (angle2 < 0) angle2 = 360f + angle2;
                if (angle2 < angle1) angle2 += 360f;
                var sweepAngle = angle2 - angle1; if (sweepAngle < 0) sweepAngle = 360f + sweepAngle;
                if (angle1 > angle2) sweepAngle = -360f + sweepAngle;
                #endregion блок коррекции углов дуги

                if (drawControl.CtrlPressed)    // разворот направления рисования
                    graphics.DrawArc(pen, rect, angle2, 360 - sweepAngle);
                else
                    graphics.DrawArc(pen, rect, angle1, sweepAngle);
            }
            catch { }
        }

        /// <summary>
        /// Рисуем дугу, проходящую через три точки
        /// </summary>
        /// <param name="drawControl"></param>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        public static void DrawArcByThreePoints(this DrawControl drawControl, Graphics graphics, Pen pen, PointF pt1, PointF pt2, PointF pt3)
        {
            float mx1 = (pt1.X + pt2.X) / 2f;
            float my1 = (pt1.Y + pt2.Y) / 2f;
            PointF mid1 = new PointF(mx1, my1);
            float dx1 = pt2.X - pt1.X;
            float dy1 = pt2.Y - pt1.Y;
            float px1 = dy1;
            float py1 = -dx1;
            float length1 = (float)Math.Sqrt(px1 * px1 + py1 * py1);
            if (length1 == 0) return; // отрезок вырожден в точку
            px1 /= length1;
            py1 /= length1;

            float mx2 = (pt3.X + pt2.X) / 2f;
            float my2 = (pt3.Y + pt2.Y) / 2f;
            PointF mid2 = new PointF(mx2, my2);
            float dx2 = pt3.X - pt2.X;
            float dy2 = pt3.Y - pt2.Y;
            float px2 = dy2;
            float py2 = -dx2;
            float length2 = (float)Math.Sqrt(px2 * px2 + py2 * py2);
            if (length2 == 0) return; // отрезок вырожден в точку
            px2 /= length2;
            py2 /= length2;

            // перпендикуляр в середине 1 отрезка
            float halfLength = Math.Max(length1, length2); //50f / zoom;
            PointF df1 = new PointF(mid1.X + px1 * halfLength, mid1.Y + py1 * halfLength);
            PointF ef1 = new PointF(mid1.X - px1 * halfLength, mid1.Y - py1 * halfLength);
            // перпендикуляр в середине 2 отрезка
            PointF df2 = new PointF(mid2.X + px2 * halfLength, mid2.Y + py2 * halfLength);
            PointF ef2 = new PointF(mid2.X - px2 * halfLength, mid2.Y - py2 * halfLength);
            // точка пересечения двух перпендикуляров
            PointF center = SegmentIntersection.Intersection(df1, ef1, df2, ef2);
            var r = center.Vector(pt1).Length();
            var rect = new RectangleF(center.X - r, center.Y - r, r * 2f, r * 2f);
            try
            { 
                #region блок коррекции углов дуги
                var angle1 = pt1.Vector(center).AngleDegree(); if (angle1 < 0) angle1 = 360f + angle1;
                var angle2 = pt2.Vector(center).AngleDegree(); if (angle2 < 0) angle2 = 360f + angle2;
                var angle3 = pt3.Vector(center).AngleDegree(); if (angle3 < 0) angle3 = 360f + angle3;
                if (angle2 < angle1) angle2 += 360f;
                if (angle3 < angle1) angle3 += 360f;
                var sweepAngle = angle3 - angle1; if (sweepAngle < 0) sweepAngle = 360f + sweepAngle;
                if (angle2 > angle3) sweepAngle = -360f + sweepAngle;
                #endregion блок коррекции углов дуги

                graphics.DrawArc(pen, rect, angle1, sweepAngle);
            }
            catch { } 
        }

        public static bool GetCenterRadiusAngleSweep(PointF pt1, PointF pt2, PointF pt3,
                                                     out PointF center, out float radius, out float angle, out float sweep)
        {
            center = PointF.Empty;
            radius = 0.0f;
            angle = 0.0f;
            sweep = 0.0f;

            float mx1 = (pt1.X + pt2.X) / 2f;
            float my1 = (pt1.Y + pt2.Y) / 2f;
            PointF mid1 = new PointF(mx1, my1);
            float dx1 = pt2.X - pt1.X;
            float dy1 = pt2.Y - pt1.Y;
            float px1 = dy1;
            float py1 = -dx1;
            float length1 = (float)Math.Sqrt(px1 * px1 + py1 * py1);
            if (length1 == 0) return false; // отрезок вырожден в точку
            px1 /= length1;
            py1 /= length1;

            float mx2 = (pt3.X + pt2.X) / 2f;
            float my2 = (pt3.Y + pt2.Y) / 2f;
            PointF mid2 = new PointF(mx2, my2);
            float dx2 = pt3.X - pt2.X;
            float dy2 = pt3.Y - pt2.Y;
            float px2 = dy2;
            float py2 = -dx2;
            float length2 = (float)Math.Sqrt(px2 * px2 + py2 * py2);
            if (length2 == 0) return false; // отрезок вырожден в точку
            px2 /= length2;
            py2 /= length2;

            // перпендикуляр в середине 1 отрезка
            float halfLength = Math.Max(length1, length2); //50f / zoom;
            PointF df1 = new PointF(mid1.X + px1 * halfLength, mid1.Y + py1 * halfLength);
            PointF ef1 = new PointF(mid1.X - px1 * halfLength, mid1.Y - py1 * halfLength);
            // перпендикуляр в середине 2 отрезка
            PointF df2 = new PointF(mid2.X + px2 * halfLength, mid2.Y + py2 * halfLength);
            PointF ef2 = new PointF(mid2.X - px2 * halfLength, mid2.Y - py2 * halfLength);
            // точка пересечения двух перпендикуляров
            center = SegmentIntersection.Intersection(df1, ef1, df2, ef2);
            radius = center.Vector(pt1).Length();

            #region блок коррекции углов дуги

            var angle1 = pt1.Vector(center).AngleDegree(); if (angle1 < 0) angle1 = 360f + angle1;
            var angle2 = pt2.Vector(center).AngleDegree(); if (angle2 < 0) angle2 = 360f + angle2;
            var angle3 = pt3.Vector(center).AngleDegree(); if (angle3 < 0) angle3 = 360f + angle3;
            if (angle2 < angle1) angle2 += 360f;
            if (angle3 < angle1) angle3 += 360f;
            var sweepAngle = angle3 - angle1; if (sweepAngle < 0) sweepAngle = 360f + sweepAngle;
            if (angle2 > angle3) sweepAngle = -360f + sweepAngle;
            #endregion блок коррекции углов дуги

            angle = angle1;
            sweep = sweepAngle;
            return true;
        }

        public static float GetAngle(float x1, float y1, float x2, float y2)
        {
            float radians = (float)Math.Atan((y2 - y1) / (x2 - x1));
            return radians;
        }

        public static float GetAngleToDergrees(float x1, float y1, float x2, float y2)
        {
            return (float)(180 * GetAngle(x1, y1, x2, y2) / Math.PI);
        }

        /// <summary>
        /// Рисуем размерную линию для отрезка
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="halfLength"></param>
        public static void DrawSizeLine(this DrawControl drawControl, Graphics graphics, Pen pen, 
            PointF start, PointF end, float halfLength, bool? side = null)
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

        public static void DrawRibbonBlock(this DrawControl drawControl, Graphics graphics, Pen pen, string name, PointF point)
        {
            float zoom = drawControl.Zoom;
            if (BlockGeometry.DefinedBlocks.ContainsKey(name))
            {
                Figure[] zeroBasedFigures = BlockGeometry.DefinedBlocks[name];
                using (var path = new GraphicsPath())
                {
                    foreach (var figure in zeroBasedFigures)
                        path.AddPath(figure.GetRendererPath(), false);
                    var m = new Matrix();
                    m.Translate(point.X, point.Y);
                    path.Transform(m);
                    graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
