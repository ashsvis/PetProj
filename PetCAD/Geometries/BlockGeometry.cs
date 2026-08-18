using PetCAD.Common;
using PetCAD.Figures;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace PetCAD.Geometries
{
    public sealed class BlockGeometry : Geometry, IMoveGeometry, IScaleGeometry, IRotateGeometry, IMoveMarker
    {
        private Figure[] zeroBasedFigures = new Figure[] { };

        public static readonly Dictionary<string, Figure[]> DefinedBlocks = new Dictionary<string, Figure[]>();

        public PointF InsertPoint { get; set; }
        public float ScaleFactor { get; set; } = 1f;
        public float Angle { get; set; }

        public BlockGeometry(string name, PointF insertPoint, float scaleFactor, float angle)
        {
            Name = name;
            InsertPoint = insertPoint;
            ScaleFactor = scaleFactor;
            Angle = angle;
            if (DefinedBlocks.ContainsKey(name))
            {
                this.zeroBasedFigures = DefinedBlocks[name];
            }
        }

        public BlockGeometry() { }

        public BlockGeometry(string name, PointF insertPoint, float scaleFactor, float angle, Figure[] zeroBasedFigures)
        {
            Name = name;
            InsertPoint = insertPoint;
            ScaleFactor = scaleFactor;
            Angle = angle;
            if (!DefinedBlocks.ContainsKey(name))
            {
                DefinedBlocks.Add(name, zeroBasedFigures);
            }
            this.zeroBasedFigures = zeroBasedFigures;
        }

        public Figure[] GetFigures()
        {
            var list = new List<Figure>();
            if (DefinedBlocks.ContainsKey(Name))
            {
                foreach (var figure in DefinedBlocks[Name])
                {
                    var fig = figure.DeepCopy();
                    if (fig.Geometry is IScaleGeometry scaleGeometry)
                        scaleGeometry.Scale(PointF.Empty, ScaleFactor);
                    if (fig.Geometry is IMoveGeometry moveGeometry)
                        moveGeometry.Move(InsertPoint.X, InsertPoint.Y);
                    list.Add(fig);
                }
            }
            return list.ToArray();
        }

        public override GraphicsPath Path
        {
            get
            {
                //var path = new GraphicsPath();
                //foreach (var figure in zeroBasedFigures)
                //    path.AddPath(figure.GetRendererPath(), false);
                //var m = new Matrix();
                //m.Translate(-InsertPoint.X, -InsertPoint.Y, MatrixOrder.Append);
                //m.Rotate(Angle, MatrixOrder.Append);
                //m.Scale(ScaleFactor, ScaleFactor, MatrixOrder.Append);
                //m.Translate(InsertPoint.X, InsertPoint.Y, MatrixOrder.Append);
                //path.Transform(m);
                var path = new GraphicsPath(); // (GraphicsPath)this.Path.Clone();
                //foreach (var figure in zeroBasedFigures)
                //    path.AddPath(figure.GetRendererPath(), false);
                //var m = new Matrix();
                //var basePoint = this.InsertPoint;
                //var kf = this.ScaleFactor;
                //var angle = this.Angle;
                //m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
                //m.Rotate(angle, MatrixOrder.Append);
                //m.Scale(kf, kf, MatrixOrder.Append);
                //m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
                //path.Transform(m);
                return path ?? new GraphicsPath();
            }
            //set { }

        }

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedGeometryOperations AllowedOperations 
        { 
            get { return AllowedGeometryOperations.All ^ AllowedGeometryOperations.Vertex; } 
        }

        public override RectangleF Bounds => RectangleF.Empty;

        public override Geometry DeepCopy()
        {
            var geometry = new BlockGeometry(Name, InsertPoint, ScaleFactor, Angle);           
            return geometry;
        }

        public override XElement GetXml()
        {
            var xgeometry = new XElement("Geometry");
            xgeometry.Add(new XAttribute("Name", Name));
            xgeometry.Add(new XAttribute("Insert", InsertPoint.ToString()));
            if (ScaleFactor != 1)
                xgeometry.Add(new XAttribute("Scale", ScaleFactor.ToString()));
            if (Angle != 0)
                xgeometry.Add(new XAttribute("Angle", Angle.ToString()));
            return xgeometry;
        }

        public override void SetXml(XElement xgeometry)
        {
            if (xgeometry == null || xgeometry.Name != "Geometry") return;
            var name = xgeometry.Attribute("Name")?.Value;
            if (string.IsNullOrWhiteSpace(name)) return;
            Name = name;
            if (DefinedBlocks.ContainsKey(name))
                this.zeroBasedFigures = DefinedBlocks[name];
            var sinsert = xgeometry.Attribute("Insert")?.Value;
            if (!string.IsNullOrWhiteSpace(sinsert))
                InsertPoint = ParseHelper.ParsePointF(sinsert, PointF.Empty);
            var sscale = xgeometry.Attribute("Scale")?.Value;
            if (!string.IsNullOrWhiteSpace(sscale))
                ScaleFactor = ParseHelper.ParseSingle(sscale, 1f);
            var sangle = xgeometry.Attribute("Angle")?.Value;
            if (!string.IsNullOrWhiteSpace(sangle))
                Angle = ParseHelper.ParseSingle(sangle, 0f);
        }

        public void Move(float offsetX, float offsetY)
        {
            InsertPoint = PointF.Add(InsertPoint, new SizeF(offsetX, offsetY));
        }

        public void Move(int index, float offsetX, float offsetY)
        {
            if (index == 0)
                InsertPoint = PointF.Add(InsertPoint, new SizeF(offsetX, offsetY));
        }

        public void Scale(PointF basePoint, float zoom)
        {
            var points = new PointF[] { InsertPoint };
            var m = new Matrix();
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Scale(zoom, zoom, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            m.TransformPoints(points);
            InsertPoint = points[0];
            ScaleFactor = zoom;
        }

        public void Rotate(PointF basePoint, float angle)
        {
            var points = new PointF[] { InsertPoint };
            var m = new Matrix();
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Rotate(angle, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            m.TransformPoints(points);
            InsertPoint = points[0];
            Angle = angle;
        }

        public override void Transform(PointF basePoint, float zoom, float angle)
        {
            var points = new PointF[] { InsertPoint };
            var m = new Matrix();
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Rotate(angle, MatrixOrder.Append);
            m.Scale(zoom, zoom, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            m.TransformPoints(points);
            InsertPoint = points[0];
            Angle = angle;
        }
    }
}
