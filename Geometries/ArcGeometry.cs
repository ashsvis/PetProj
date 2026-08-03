using PetProj.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace PetProj.Geometries
{
    public sealed class ArcGeometry : Geometry, IMoveGeometry, IMoveMarker
    {
        public PointF CenterPoint { get; private set; }
        public float Radius { get; private set; }
        public float StartAngle { get; private set; }
        public float SweepAngle { get; private set; }

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
            set { }
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
            var xfill = new XElement("Geometry");
            xfill.Add(new XAttribute("Name", Name));
            var xpath = new XElement("Arc");
            xpath.Add(new XAttribute("Center", CenterPoint.ToString()));
            xpath.Add(new XAttribute("Radius", Radius.ToString()));
            xpath.Add(new XAttribute("Angle", StartAngle.ToString()));
            xpath.Add(new XAttribute("Sweep", SweepAngle.ToString()));
            xfill.Add(xpath);
            return xfill;
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
            if (index == 0)
                CenterPoint = PointF.Add(CenterPoint, new SizeF(offsetX, offsetY));
            //else if (index == 1)
            //    Points[1] = PointF.Add(Points[1], new SizeF(offsetX, offsetY));
        }
    }
}
