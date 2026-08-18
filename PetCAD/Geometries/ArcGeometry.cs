using PetCAD.Common;
using PetCAD.Renderers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace PetCAD.Geometries
{
    public sealed class ArcGeometry : Geometry, IMoveGeometry, IScaleGeometry, IRotateGeometry, IMoveMarker
    {
        public PointF CenterPoint { get; set; }
        public float Radius { get; set; }
        public float StartAngle { get; set; }
        public float SweepAngle { get; set; }

        public PointF StartPoint => Radius > 0
            ? new PointF(CenterPoint.X + (float)(Radius * Math.Cos(StartAngle * (Math.PI / 180))), 
                         CenterPoint.Y + (float)(Radius * Math.Sin(StartAngle * (Math.PI / 180))))
            : PointF.Empty;

        public PointF MiddlePoint => Radius > 0
            ? new PointF(CenterPoint.X + (float)(Radius * Math.Cos((StartAngle + SweepAngle / 2) * (Math.PI / 180))),
                         CenterPoint.Y + (float)(Radius * Math.Sin((StartAngle + SweepAngle / 2) * (Math.PI / 180))))
            : PointF.Empty;

        public PointF EndPoint => Radius > 0
            ? new PointF(CenterPoint.X + (float)(Radius * Math.Cos((StartAngle + SweepAngle) * (Math.PI / 180))),
                         CenterPoint.Y + (float)(Radius * Math.Sin((StartAngle + SweepAngle) * (Math.PI / 180))))
            : PointF.Empty;

        public PointF[] QuadrantPoints
        {
            get 
            {
                var list = new List<PointF>();
                foreach(var angle in new float[] { -360f, -270f, -180f, -90, 0f, 90f, 180f, 270f, 360f })
                {
                    if (angle >= StartAngle && angle <= StartAngle + SweepAngle)
                    {
                        list.Add(new PointF(CenterPoint.X + (float)(Radius * Math.Cos(angle * (Math.PI / 180))),
                                            CenterPoint.Y + (float)(Radius * Math.Sin(angle * (Math.PI / 180)))));
                    }
                }
                return list.ToArray();
            }
        }

        public ArcGeometry() { }    

        internal ArcGeometry(PointF center, float radius, float startAngle, float sweepAngle)
        {
            CenterPoint = center;
            Radius = radius;
            StartAngle = startAngle;
            SweepAngle = sweepAngle;
        }

        public override GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                if (Radius > 0 && Math.Abs(SweepAngle) > 0)
                {
                    var rect = new RectangleF(CenterPoint.X - Radius, CenterPoint.Y - Radius, Radius * 2f, Radius * 2f);
                    path.AddArc(rect, StartAngle, SweepAngle);
                }
                return path;
            }
        }

        public override RectangleF Bounds
        {
            get
            {
                // Переводим угол из градусов в радианы
                double angle1 =StartAngle * Math.PI / 180.0;
                double angle2 = (StartAngle + SweepAngle) * Math.PI / 180.0;

                // Вычисляем координаты
                float x1 = (float)(CenterPoint.X + Radius * Math.Cos(angle1));
                float y1 = (float)(CenterPoint.Y + Radius * Math.Sin(angle1));
                float x2 = (float)(CenterPoint.X + Radius * Math.Cos(angle2));
                float y2 = (float)(CenterPoint.Y + Radius * Math.Sin(angle2));
                
                return new RectangleF(Math.Min(x1, x2), Math.Min(y1, y2), Math.Abs(x2 - x1), Math.Abs(y2 - y1));
            }
        }

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedGeometryOperations AllowedOperations { get { return AllowedGeometryOperations.None; } }

        public override Geometry DeepCopy()
        {
            var geometry = new ArcGeometry(CenterPoint, Radius, StartAngle, SweepAngle)
            {
                Name = Name,
            };
            return geometry;
        }

        public override XElement GetXml()
        {
            var xgeometry = new XElement("Geometry");
            xgeometry.Add(new XAttribute("Name", Name));
            var xarc = new XElement("Arc");
            xarc.Add(new XAttribute("Center", CenterPoint.ToString()));
            xarc.Add(new XAttribute("Radius", Radius.ToString()));
            xarc.Add(new XAttribute("Angle", StartAngle.ToString()));
            xarc.Add(new XAttribute("Sweep", SweepAngle.ToString()));
            xgeometry.Add(xarc);
            return xgeometry;
        }

        public override void SetXml(XElement xgeometry)
        {
            if (xgeometry == null || xgeometry.Name != "Geometry") return;
            var name = xgeometry.Attribute("Name")?.Value;
            if (string.IsNullOrWhiteSpace(name)) return;
            Name = name;
            var xpath = xgeometry.Element("Arc");
            if (xpath == null) return;
            var scenter = xpath.Attribute("Center")?.Value;
            var sradius = xpath.Attribute("Radius")?.Value;
            var sangle = xpath.Attribute("Angle")?.Value;
            var ssweep = xpath.Attribute("Sweep")?.Value;
            if (!string.IsNullOrWhiteSpace(scenter) && !string.IsNullOrWhiteSpace(sradius) &&
                !string.IsNullOrWhiteSpace(sangle) && !string.IsNullOrWhiteSpace(ssweep))
            {
                CenterPoint = ParseHelper.ParsePointF(scenter, PointF.Empty);
                Radius = ParseHelper.ParseSingle(sradius, 0);
                StartAngle = ParseHelper.ParseSingle(sangle, 0);
                SweepAngle = ParseHelper.ParseSingle(ssweep, 0);
            }
        }

        public void Move(float offsetX, float offsetY)
        {
            CenterPoint = PointF.Add(CenterPoint, new SizeF(offsetX, offsetY));
        }

        public void Move(int index, float offsetX, float offsetY)
        {
            PointF center;
            float radius, angle, sweep;
            switch (index)
            {
                case 0:
                    CenterPoint = PointF.Add(CenterPoint, new SizeF(offsetX, offsetY));
                    break;
                case 1:
                    var startPoint = PointF.Add(StartPoint, new SizeF(offsetX, offsetY));
                    if (DrawHelper.GetCenterRadiusAngleSweep(startPoint, MiddlePoint, EndPoint, out center, out radius, out angle, out sweep))
                    {
                        CenterPoint = center;
                        Radius = radius;
                        StartAngle = angle;
                        SweepAngle = sweep;
                    }
                    break;
                case 2:
                    var middlePoint = PointF.Add(MiddlePoint, new SizeF(offsetX, offsetY));
                    if (DrawHelper.GetCenterRadiusAngleSweep(StartPoint, middlePoint, EndPoint, out center, out radius, out angle, out sweep))
                    {
                        CenterPoint = center;
                        Radius = radius;
                        StartAngle = angle;
                        SweepAngle = sweep;
                    }
                    break;
                case 3:
                    var endPoint = PointF.Add(EndPoint, new SizeF(offsetX, offsetY));
                    if (DrawHelper.GetCenterRadiusAngleSweep(StartPoint, MiddlePoint, endPoint, out center, out radius, out angle, out sweep))
                    {
                        CenterPoint = center;
                        Radius = radius;
                        StartAngle = angle;
                        SweepAngle = sweep;
                    }
                    break;
            }
        }

        public void Scale(PointF basePoint, float zoom)
        {
            var points = new PointF[] { StartPoint, MiddlePoint, EndPoint };
            var m = new Matrix();
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Scale(zoom, zoom, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            m.TransformPoints(points);
            if (ConvertThreePointsToCenterRadiusAndAngles(points[0], points[1], points[2],
                out PointF center, out float radius, out float startAngle, out float sweepAngle))
            {
                CenterPoint = center;
                Radius = radius;
                StartAngle = startAngle;
                SweepAngle = sweepAngle;
            }
        }

        public void Rotate(PointF basePoint, float angle)
        {
            var points = new PointF[] { StartPoint, MiddlePoint, EndPoint };
            var m = new Matrix();
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Rotate(angle, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            m.TransformPoints(points);
            if (ConvertThreePointsToCenterRadiusAndAngles(points[0], points[1], points[2],
                out PointF center, out float radius, out float startAngle, out float sweepAngle))
            {
                CenterPoint = center;
                Radius = radius;
                StartAngle = startAngle;
                SweepAngle = sweepAngle;
            }
        }

        public static bool ConvertThreePointsToCenterRadiusAndAngles(PointF pt1, PointF pt2, PointF pt3,
            out PointF center, out float radius, out float startAngle, out float sweepAngle)
        {
            center = PointF.Empty;
            radius = 0;
            startAngle = 0;
            sweepAngle = 0;

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
            sweepAngle = angle3 - angle1; if (sweepAngle < 0) sweepAngle = 360f + sweepAngle;
            if (angle2 > angle3) sweepAngle = -360f + sweepAngle;

            #endregion блок коррекции углов дуги
            startAngle = angle1;
            return true;
        }

        public override void Transform(PointF basePoint, float zoom, float angle)
        {
            var points = new PointF[] { StartPoint, MiddlePoint, EndPoint };
            var m = new Matrix();
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Rotate(angle, MatrixOrder.Append);
            m.Scale(zoom, zoom, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            m.TransformPoints(points);
            if (ConvertThreePointsToCenterRadiusAndAngles(points[0], points[1], points[2],
                out PointF center, out float radius, out float startAngle, out float sweepAngle))
            {
                CenterPoint = center;
                Radius = radius;
                StartAngle = startAngle;
                SweepAngle = sweepAngle;
            }
        }
    }
}
