using PetCAD.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml.Linq;

namespace PetCAD.Geometries
{
    public sealed class LineGeometry : Geometry, IMoveGeometry, IScaleGeometry, IRotateGeometry, IMoveMarker
    {
        public readonly List<PointF> Points = new List<PointF>();

        public PointF StartPoint => Points.Count > 0 ? Points.First() : PointF.Empty;
        public PointF EndPoint => Points.Count > 0 ? Points.Last() : PointF.Empty;
        public override RectangleF Bounds => 
            Points.Count == 2 
               ? new RectangleF(Math.Min(Points[0].X, Points[1].X), Math.Min(Points[0].Y, Points[1].Y), 
                   Math.Abs(Points[0].X - Points[1].X), Math.Abs(Points[0].Y - Points[1].Y)) : RectangleF.Empty;

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedGeometryOperations AllowedOperations { get { return AllowedGeometryOperations.None; } }

        public override GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                if (Points.Count == 2)
                {
                    var points = Points.ToArray();
                    path.AddLines(points);
                }
                return path;
            }
        }

        public LineGeometry()
        {
        }

        /// <summary>
        /// Конструктор, недоступный вне проекта EditorModel
        /// (только для внутреннего использования)
        /// </summary>
        internal LineGeometry(PointF point)
        {
            Points.Add(point);
        }

        public override Geometry DeepCopy()
        {
            var geometry = new LineGeometry(StartPoint)
            {
                Name = Name,
            };
            geometry.Points.AddRange(Points.Skip(1));
            return geometry;
        }

        public override XElement GetXml()
        {
            var xgeometry = new XElement("Geometry");
            xgeometry.Add(new XAttribute("Name", Name));
            var xline = new XElement("Line");
            xline.Add(new XAttribute("Start", StartPoint.ToString()));
            xline.Add(new XAttribute("End", EndPoint.ToString()));
            xgeometry.Add(xline);
            return xgeometry;
        }

        public override void SetXml(XElement xgeometry)
        {
            if (xgeometry == null || xgeometry.Name != "Geometry") return;
            var name = xgeometry.Attribute("Name")?.Value;
            if (string.IsNullOrWhiteSpace(name)) return;
            Name = name;
            var xpath = xgeometry.Element("Line");
            if (xpath == null) return;
            var sstart = xpath.Attribute("Start")?.Value;
            var send = xpath.Attribute("End")?.Value;
            if (!string.IsNullOrWhiteSpace(sstart) && !string.IsNullOrWhiteSpace(send))
            {
                Points.Clear();
                Points.Add(ParseHelper.ParsePointF(sstart, PointF.Empty));
                Points.Add(ParseHelper.ParsePointF(send, PointF.Empty));
            }
        }

        public void AddPoint(PointF point)
        {
            if (!Points.Contains(point)) Points.Add(point);
        }

        public void Move(float offsetX, float offsetY)
        {
            for (var i = 0; i < Points.Count; i++)
                Points[i] = PointF.Add(Points[i], new SizeF(offsetX, offsetY));
        }

        public void Move(int index, float offsetX, float offsetY)
        {
            if (index == 0)
                Points[0] = PointF.Add(Points[0], new SizeF(offsetX, offsetY));
            else if (index == 1)
                Points[1] = PointF.Add(Points[1], new SizeF(offsetX, offsetY));
        }

        public void Scale(PointF basePoint, float zoom)
        {
            var points = new PointF[] { Points[0], Points[1] };
            var m = new Matrix();
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Scale(zoom, zoom, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            m.TransformPoints(points);
            Points[0] = points[0];
            Points[1] = points[1];
        }

        public void Rotate(PointF basePoint, float angle)
        {
            var points = new PointF[] { Points[0], Points[1] };
            var m = new Matrix();
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Rotate(angle, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            m.TransformPoints(points);
            Points[0] = points[0];
            Points[1] = points[1];
        }

        public override void Transform(PointF basePoint, float zoom, float angle)
        {
            var points = new PointF[] { Points[0], Points[1] };
            var m = new Matrix();
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Rotate(angle, MatrixOrder.Append);
            m.Scale(zoom, zoom, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            m.TransformPoints(points);
            Points[0] = points[0];
            Points[1] = points[1];
        }
    }
}
