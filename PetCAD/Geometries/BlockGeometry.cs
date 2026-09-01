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
    public sealed class BlockGeometry : Geometry, IMoveGeometry, IScaleGeometry, IRotateGeometry, IMoveMarker, IExplodeGeometry
    {
        private Figure[] zeroBasedFigures = new Figure[] { };

        public static readonly Dictionary<string, Figure[]> DefinedBlocks = new Dictionary<string, Figure[]>();

        public BlockGeometry(Figure figure, string name)
        {
            Owner = figure;
            Kind = "BlockRef";
            Name = name;
            if (DefinedBlocks.ContainsKey(name))
                zeroBasedFigures = DefinedBlocks[name];
        }

        public BlockGeometry(Figure figure) 
        { 
            Owner = figure;
            Kind = "BlockRef";
        }

        public BlockGeometry(Figure figure, string name, Figure[] zeroBasedFigures)
        {
            Owner = figure;
            Kind = "BlockRef";
            Name = name;
            this.zeroBasedFigures = zeroBasedFigures;
        }

        public Figure[] GetZeroBasedFigures()
        {
            var list = new List<Figure>();
            if (DefinedBlocks.ContainsKey(Name))
            {
                foreach (var figure in DefinedBlocks[Name])
                {
                    var fig = figure.DeepCopy();
                    list.Add(fig);
                }
            }
            return list.ToArray();
        }

        public override GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                foreach (var figure in zeroBasedFigures)
                    path.AddPath(figure.GetRendererPath(), false);
                return path;
            }
        }

        public override RectangleF Bounds
        {
            get
            {
                var m = Owner.Transformation;
                var rect = RectangleF.Empty;
                foreach (var figure in zeroBasedFigures)
                {
                    using (var path = figure.GetRendererPath())
                    {
                        path.Transform(m);
                        path.Flatten();
                        if (rect.IsEmpty)
                            rect = path.GetBounds();
                        else
                            rect = RectangleF.Union(rect, path.GetBounds());
                    }
                }
                return rect;
            }
        }

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedGeometryOperations AllowedOperations 
        { 
            get { return AllowedGeometryOperations.All ^ AllowedGeometryOperations.Vertex; } 
        }

        public override Geometry DeepCopy(Figure figure)
        {
            var geometry = new BlockGeometry(figure, Name);           
            return geometry;
        }

        public override XElement GetXml()
        {
            var xgeometry = new XElement("Geometry");
            xgeometry.Add(new XAttribute("Kind", Kind));
            xgeometry.Add(new XAttribute("Name", Name));
            return xgeometry;
        }

        public override void SetXml(XElement xgeometry)
        {
            if (xgeometry == null || xgeometry.Name != "Geometry") return;
            var kind = xgeometry.Attribute("Kind")?.Value;
            if (string.IsNullOrWhiteSpace(kind)) return;
            var name = xgeometry.Attribute("Name")?.Value;
            if (string.IsNullOrWhiteSpace(name)) return;
            Name = name;
            if (DefinedBlocks.ContainsKey(name))
                zeroBasedFigures = DefinedBlocks[name];
        }

        /// <summary>
        /// Перемещение блока безотносительно к маркерам
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void Move(float offsetX, float offsetY)
        {
            var m = Owner.Transformation;
            var scaled = m.GetScale();
            var rotated = m.GetAngle();
            // компенсация масштаба
            m.Scale(1f / scaled.Width, 1f / scaled.Height);
            // компенсация поворота
            m.Rotate(-rotated);
            // собственно перемещение
            m.Translate(offsetX, offsetY);
            // возврат компенсации поворота
            m.Rotate(rotated);
            // возврат компенсации масштаба
            m.Scale(scaled.Width, scaled.Height);
        }

        /// <summary>
        /// Перемещение блока за маркер
        /// </summary>
        /// <param name="index"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void Move(int index, float offsetX, float offsetY)
        {
            // нулевой маркер - это ORIGIN
            if (index == 0)
                Move(offsetX, offsetY);
        }

        public void Scale(PointF basePoint, float zoom)
        {
            var m = Owner.Transformation;
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Scale(zoom, zoom, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
        }

        public void Rotate(PointF baseRotatePoint, float angle)
        {
            var m = Owner.Transformation;
            m.Translate(-baseRotatePoint.X, -baseRotatePoint.Y, MatrixOrder.Append);
            m.Rotate(angle, MatrixOrder.Append);
            m.Translate(baseRotatePoint.X, baseRotatePoint.Y, MatrixOrder.Append);
        }

        public override Marker[] GetGeometryMarkers()
        {
            var pts = new PointF[] { PointF.Empty };
            var m = Owner.Transformation;
            m.TransformPoints(pts);
            var markers = new List<Marker>
            {
                new VertexMarker
                {
                    MarkerType = MarkerType.Vertex,
                    Position = pts[0],
                    Index = 0,
                    Owner = Owner,
                }
            };
            return markers.ToArray();
        }

        public override Marker[] GetBindingMarkers(AllowedObjectBindings allowed, PointF basePoint)
        {
            var markers = new List<Marker>();
            var matrix = Owner.Transformation;
            foreach (var figure in Explode())
                figure.Geometry.GetBindingMarkers(allowed, basePoint).ToList().ForEach(marker => markers.Add(marker));
            return markers.ToArray();
        }

        public Figure[] Explode()
        {
            var matrix = Owner.Transformation.Clone();
            var figures = new List<Figure>();
            Figure fig;
            PointF[] pts;
            foreach (var figure in this.GetZeroBasedFigures())
            {
                if (figure.Geometry is LineGeometry line)
                {
                    pts = new PointF[] { line.StartPoint, line.EndPoint };
                    matrix.TransformPoints(pts);
                    fig = new Figure();
                    FigureBuilder.BuildLineGeometry(fig, pts[0]);
                    (fig.Geometry as LineGeometry).AddPoint(pts[1]);
                    figures.Add(fig);
                }
                else if (figure.Geometry is ArcGeometry arc)
                {
                    pts = new PointF[] { arc.StartPoint, arc.MiddlePoint, arc.EndPoint };
                    matrix.TransformPoints(pts);
                    fig = new Figure();
                    if (ArcGeometry.ConvertThreePointsToCenterRadiusAndAngles(pts[0], pts[1], pts[2],
                        out PointF center, out float radius, out float startAngle, out float sweepAngle))
                    {
                        FigureBuilder.BuildArcGeometry(fig, center, radius, startAngle, sweepAngle);
                        figures.Add(fig);
                    }
                }
                else if (figure.Geometry is BlockGeometry block)
                {
                    fig = new BlockReference();
                    fig.Transformation = figure.Transformation.Clone();
                    var offset = matrix.GetOffset();
                    FigureBuilder.BuildBlockGeometry(block.Name, fig, PointF.Empty, block.Explode());
                    fig.Transformation.Multiply(matrix);
                    figures.Add(fig);
                }
            }
            return figures.ToArray();
        }
    }
}
