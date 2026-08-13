using PetCAD.Common;
using PetCAD.Figures;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace PetCAD.Geometries
{
    public sealed class BlockGeometry : Geometry, IMoveGeometry, IMoveMarker
    {
        private Figure[] zeroBasedFigures = new Figure[] { };

        public static readonly Dictionary<string, Figure[]> DefinedBlocks = new Dictionary<string, Figure[]>();

        public PointF InsertPoint { get; set; }

        public BlockGeometry(string name, PointF insertPoint)
        {
            Name = name;
            InsertPoint = insertPoint;
            if (DefinedBlocks.ContainsKey(name))
            {
                this.zeroBasedFigures = DefinedBlocks[name];
            }
        }

        public BlockGeometry() { }

        public BlockGeometry(string name, PointF insertPoint, Figure[] zeroBasedFigures)
        {
            Name = name;
            InsertPoint = insertPoint;
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
                    if (fig.Geometry is IMoveGeometry mover)
                    {
                        mover.Move(InsertPoint.X, InsertPoint.Y);
                        list.Add(fig);
                    }
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
                var m = new Matrix();
                m.Translate(InsertPoint.X, InsertPoint.Y);
                path.Transform(m);
                return path;
            }
            set { }

        }

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedGeometryOperations AllowedOperations 
        { 
            get { return AllowedGeometryOperations.All ^ AllowedGeometryOperations.Vertex; } 
        }

        public override Geometry DeepCopy()
        {
            var geometry = new BlockGeometry(Name, InsertPoint);           
            return geometry;
        }

        public override XElement GetXml()
        {
            var xgeometry = new XElement("Geometry");
            xgeometry.Add(new XAttribute("Name", Name));
            xgeometry.Add(new XAttribute("Insert", InsertPoint.ToString()));
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
    }
}
