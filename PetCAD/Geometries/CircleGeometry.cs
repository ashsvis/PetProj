using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Makers;
using PetCAD.Renderers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml.Linq;

namespace PetCAD.Geometries
{
    public sealed class CircleGeometry : Geometry, ICircleGeometry, IMoveGeometry, IScaleGeometry, IMoveMarker
    {
        public PointF CenterPoint { get; set; }
        public float Radius { get; set; }

        public CircleGeometry(Figure figure)
        {
            Owner = figure;
            Kind = "Circle";
        }

        internal CircleGeometry(Figure figure, PointF center, float radius)
        {
            Owner = figure;
            Kind = "Circle";
            CenterPoint = center;
            Radius = radius;
        }

        public override GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                if (Radius > 0)
                {
                    var rect = new RectangleF(CenterPoint.X - Radius, CenterPoint.Y - Radius, Radius * 2f, Radius * 2f);
                    path.AddEllipse(rect);
                }
                return path;
            }
        }

        public override RectangleF Bounds
        {
            get
            {
                using (var path = (GraphicsPath)Path.Clone())
                {
                    path.Flatten();
                    return path.GetBounds();
                }
            }
        }

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedGeometryOperations AllowedOperations { get { return AllowedGeometryOperations.All; } }

        public override Geometry DeepCopy(Figure figure)
        {
            var geometry = new CircleGeometry(figure, CenterPoint, Radius)
            {
                Kind = Kind,
                Name = Name,
            };
            return geometry;
        }

        public override XElement GetXml()
        {
            var xgeometry = new XElement("Geometry");
            xgeometry.Add(new XAttribute("Kind", Kind));
            if (!string.IsNullOrEmpty(Name))
                xgeometry.Add(new XAttribute("Name", Name));
            var xarc = new XElement("Circle");
            xarc.Add(new XAttribute("Center", CenterPoint.ToString()));
            xarc.Add(new XAttribute("Radius", Radius.ToString()));
            xgeometry.Add(xarc);
            return xgeometry;
        }

        public override void SetXml(XElement xgeometry)
        {
            if (xgeometry == null || xgeometry.Name != "Geometry") return;
            var kind = xgeometry.Attribute("Kind")?.Value;
            if (string.IsNullOrWhiteSpace(kind)) return;
            Name = xgeometry.Attribute("Name")?.Value;
            var xpath = xgeometry.Element("Circle");
            if (xpath == null) return;
            var scenter = xpath.Attribute("Center")?.Value;
            var sradius = xpath.Attribute("Radius")?.Value;
            if (!string.IsNullOrWhiteSpace(scenter) && !string.IsNullOrWhiteSpace(sradius) )
            {
                CenterPoint = ParseHelper.ParsePointF(scenter, PointF.Empty);
                Radius = ParseHelper.ParseSingle(sradius, 0);
            }
        }

        public void Move(float offsetX, float offsetY)
        {
            CenterPoint = PointF.Add(CenterPoint, new SizeF(offsetX, offsetY));
        }

        public bool CanMoveMarker(int index, float offsetX, float offsetY)
        {
            return new int[] { 0, 1, 2, 3, 4 }.Contains(index);
        }

        public void MoveMarker(int index, float offsetX, float offsetY)
        {
            switch (index)
            {
                case 0:
                    CenterPoint = PointF.Add(CenterPoint, new SizeF(offsetX, offsetY));
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    var quadrants = QuadrantPoints();
                    var point = PointF.Add(quadrants[index - 1], SizeF.Empty /*new SizeF(offsetX, offsetY)*/);
                    var length = point.Vector(CenterPoint).Length();
                    Radius = length;
                    break;
            }
        }

        public void Scale(PointF basePoint, float zoom)
        {
            //var points = new PointF[] { StartPoint, MiddlePoint, EndPoint };
            //var m = new Matrix();
            //m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            //m.Scale(zoom, zoom, MatrixOrder.Append);
            //m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            //m.TransformPoints(points);
            //if (ConvertThreePointsToCenterRadiusAndAngles(points[0], points[1], points[2],
            //    out PointF center, out float radius, out float startAngle, out float sweepAngle))
            //{
            //    CenterPoint = center;
            //    Radius = radius;
            //}
        }

        public override Marker[] GetGeometryMarkers()
        {
            var markers = new List<Marker>
            {
                new CenterMarker
                    {
                        MarkerType =  MarkerType.Center,
                        Position = CenterPoint,
                        Owner = Owner,
                    },
            };
            var n = markers.Count;
            foreach (var quadrantPoint in QuadrantPoints())
            {
                markers.Add(
                    new VertexMarker
                    {
                        MarkerType = MarkerType.Vertex,
                        Position = quadrantPoint,
                        Index = ++n,
                        Owner = Owner,
                    });
            };
            return markers.ToArray();
        }

        private PointF[] QuadrantPoints()
        {
            var list = new List<PointF>();
            foreach (var angle in new float[] { 0f, 90f, 180f, 270f })
                list.Add(new PointF(CenterPoint.X + (float)(Radius * Math.Cos(angle * (Math.PI / 180.0))),
                                    CenterPoint.Y + (float)(Radius * Math.Sin(angle * (Math.PI / 180.0)))));
            return list.ToArray();
        }

        public override Marker[] GetBindingMarkers(AllowedObjectBindings allowed, PointF basePoint)
        {
            var markers = new List<Marker>();
            // поиск центра дуги
            if (allowed.HasFlag(AllowedObjectBindings.Center))
            {
                markers.Add(new BindingCenterMarker
                {
                    MarkerType = MarkerType.BindingCenter,
                    Position = CenterPoint,
                    Owner = Owner,
                });
            }
            using (var path = Path)
            using (var pen = new Pen(Color.Blue, 3f))
            {
                // поиск доступных квадрантов
                if (allowed.HasFlag(AllowedObjectBindings.Quadrant))
                {
                    foreach (var quadrantPoint in QuadrantPoints())
                    {
                        if (path.IsOutlineVisible(quadrantPoint, pen))
                        {
                            markers.Add(new BindingQuadrantMarker
                            {
                                MarkerType = MarkerType.BindingQuadrant,
                                Position = quadrantPoint,
                                Owner = Owner,
                            });
                        }
                    }
                }
                // поиск проекций базовой точки на дугу
                if (allowed.HasFlag(AllowedObjectBindings.Normal))
                {
                    // проекция точки проходит также через центр дуги
                    if (PointFExtension.NormalPointOnArc(this, basePoint, out PointF[] normals))
                    {
                        foreach (var point in normals)
                        {
                            if (path.IsOutlineVisible(point, pen))
                            {
                                markers.Add(new BindingNormalMarker
                                {
                                    MarkerType = MarkerType.BindingNormal,
                                    Position = point,
                                    Owner = Owner,
                                });
                            }
                        }
                    }
                }
                // поиск точек касания от базовой точки к дуге 
                if (allowed.HasFlag(AllowedObjectBindings.Tangent))
                {
                    if (PointFExtension.TangentPointOnArc(this, basePoint, out PointF[] tangents))
                    {
                        foreach (var point in tangents)
                        {
                            if (path.IsOutlineVisible(point, pen))
                            {
                                markers.Add(new BindingTangentMarker
                                {
                                    MarkerType = MarkerType.BindingTangent,
                                    Position = point,
                                    Owner = Owner,
                                });
                            }
                        }
                    }
                }
            }
            return markers.ToArray();
        }
    }
}
