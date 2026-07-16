using PetProj.Geometries;
using PetProj.Renderers;
using PetProj.Styles;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace PetProj.Figures
{
    public class Figure
    {
        public Figure() 
        {
            Style = new Style();
            Renderer = new DefaultRenderer();
        }

        /// <summary>
        /// Свойство стиля рисования фигуры
        /// </summary>
        public Style Style { get; private set; }

        /// <summary>
        /// Свойство рисовальщика фигуры
        /// </summary>
        public Renderer Renderer { get; set; }

        /// <summary>
        /// Свойство источника геометрии фигуры
        /// </summary>
        public Geometry Geometry { get; set; }

        public PointF Origin { get; set; } = new PointF(0, 0);

        public List<SizeF> Offsets { get; set; } = new List<SizeF>();

        public virtual void DrawAt(Graphics graphics, Color forecolor) { }
        public virtual void DrawHighlightAt(Graphics graphics, Color forecolor) { }
        public virtual bool Contains(PointF point) { return false; }
        public virtual XElement GetData() { return null; }
        public virtual void DrawSelectedAt(Graphics graphics, Color forecolor) { }
        public virtual RectangleF[] GetMarkers() { return new RectangleF[] { }; }
    }

}
