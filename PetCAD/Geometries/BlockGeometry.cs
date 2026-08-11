using PetCAD.Figures;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace PetCAD.Geometries
{
    public sealed class BlockGeometry : Geometry, IMoveGeometry, IMoveMarker
    {
        private readonly Figure[] zeroBasedFigures = new Figure[] { };

        public static readonly Dictionary<string, Figure[]> Blocks = new Dictionary<string, Figure[]>();

        public PointF InsertPoint { get; set; }

        public BlockGeometry(string name, PointF insertPoint)
        {
            Name = name;
            InsertPoint = insertPoint;
            if (Blocks.ContainsKey(name))
            {
                this.zeroBasedFigures = Blocks[name];
            }
        }

        public BlockGeometry(string name, PointF insertPoint, Figure[] zeroBasedFigures)
        {
            Name = name;
            InsertPoint = insertPoint;
            if (!Blocks.ContainsKey(name))
            {
                Blocks.Add(name, zeroBasedFigures);
            }
            this.zeroBasedFigures = zeroBasedFigures;
        }

        public Figure[] GetFigures()
        {
            var list = new List<Figure>();
            if (Blocks.ContainsKey(Name))
            {
                foreach (var figure in Blocks[Name])
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
