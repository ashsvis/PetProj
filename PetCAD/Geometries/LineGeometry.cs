using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Makers;
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
                    path.Flatten();
                }
                return path;
            }
        }

        public override RectangleF Bounds
        {
            get
            {
                using (var path = Path)
                {
                    var rect = path.GetBounds();
                    if (rect.Width == 0) rect.Inflate(0.1f, 0f);
                    if (rect.Height == 0) rect.Inflate(0f, 0.1f);
                    return rect;
                }
            }
        }

        public LineGeometry(Figure figure)
        {
            Owner = figure;
            Kind = "Segment";
        }

        /// <summary>
        /// Конструктор, недоступный вне проекта EditorModel
        /// (только для внутреннего использования)
        /// </summary>
        internal LineGeometry(Figure figure, PointF point)
        {
            Owner = figure;
            Kind = "Segment";
            Points.Add(point);
        }

        public override Geometry DeepCopy(Figure figure)
        {
            var geometry = new LineGeometry(figure, StartPoint)
            {
                Kind = Kind,
                Name = Name,
            };
            geometry.Points.AddRange(Points.Skip(1));
            return geometry;
        }

        public override XElement GetXml()
        {
            var xgeometry = new XElement("Geometry");
            xgeometry.Add(new XAttribute("Kind", Kind));
            if (!string.IsNullOrEmpty(Name))
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
            var kind = xgeometry.Attribute("Kind")?.Value;
            if (string.IsNullOrWhiteSpace(kind)) return;
            Name = xgeometry.Attribute("Name")?.Value;
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

        public void Scale(PointF baseScalePoint, float zoom)
        {
            var points = new PointF[] { Points[0], Points[1] };
            using (var m = new Matrix())
            {
                m.Translate(-baseScalePoint.X, -baseScalePoint.Y, MatrixOrder.Append);
                m.Scale(zoom, zoom, MatrixOrder.Append);
                m.Translate(baseScalePoint.X, baseScalePoint.Y, MatrixOrder.Append);
                m.TransformPoints(points);
            }
            Points[0] = points[0];
            Points[1] = points[1];
        }

        public void Rotate(PointF baseRotatePoint, float angle)
        {
            var points = new PointF[] { Points[0], Points[1] };
            using (var m = new Matrix())
            {
                m.Translate(-baseRotatePoint.X, -baseRotatePoint.Y, MatrixOrder.Append);
                m.Rotate(angle, MatrixOrder.Append);
                m.Translate(baseRotatePoint.X, baseRotatePoint.Y, MatrixOrder.Append);
                m.TransformPoints(points);
            }
            Points[0] = points[0];
            Points[1] = points[1];
        }

        public override Marker[] GetGeometryMarkers()
        {
            return new Marker[] 
            {
                new VertexMarker
                    {
                        MarkerType =  MarkerType.Vertex,
                        Position = StartPoint,
                        Index = 0,
                        Owner = Owner,
                    },
                new VertexMarker
                    {
                        MarkerType =  MarkerType.Vertex,
                        Position = EndPoint,
                        Index = 1,
                        Owner = Owner,
                    },
                new MiddleMarker
                    {
                        MarkerType =  MarkerType.Middle,
                        Position = new PointF((StartPoint.X + EndPoint.X) / 2f, (StartPoint.Y + EndPoint.Y) / 2f),
                        Owner = Owner,
                    },
            };
        }

        public override Marker[] GetBindingMarkers(AllowedObjectBindings allowed, PointF basePoint)
        {
            var markers = new List<Marker>();
            // поиск конечных точек
            if (allowed.HasFlag(AllowedObjectBindings.EndPoint))
            {
                markers.Add(new BindingVertexMarker
                {
                    MarkerType = MarkerType.BindingVertex,
                    Position = StartPoint,
                    Index = 0,
                    Owner = Owner,
                });
                markers.Add(new BindingVertexMarker
                {
                    MarkerType = MarkerType.BindingVertex,
                    Position = EndPoint,
                    Index = 1,
                    Owner = Owner,
                });
            }
            // поиск средней точки на отрезке
            if (allowed.HasFlag(AllowedObjectBindings.Middle))
            {
                var mid = new PointF((StartPoint.X + EndPoint.X) / 2f, (StartPoint.Y + EndPoint.Y) / 2f);
                markers.Add(new BindingMiddleMarker
                {
                    MarkerType = MarkerType.BindingMiddle,
                    Position = mid,
                    Owner = Owner,
                });
            }
            // поиск проекции базовой точки на отрезок 
            if (allowed.HasFlag(AllowedObjectBindings.Normal) &&
                PointFExtension.ProjectPointOnSegment(StartPoint, EndPoint, basePoint, out PointF norm))
            {
                markers.Add(new BindingNormalMarker
                {
                    MarkerType = MarkerType.BindingNormal,
                    Position = norm,
                    Owner = Owner,
                });
            }
            return markers.ToArray();
        }
    }
}
