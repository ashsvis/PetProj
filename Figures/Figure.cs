using PetProj.Geometries;
using PetProj.Renderers;
using PetProj.Styles;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        /// Свойство источника геометрии фигуры
        /// </summary>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Свойство рисовальщика фигуры
        /// </summary>
        public Renderer Renderer { get; set; }

        public XElement GetXml()
        {
            var xfigure = new XElement("Figure");
            xfigure.Add(Geometry.GetXml());
            xfigure.Add(Style.GetXml());
            return xfigure;
        }

        public void SetXml(XElement xfigure)
        {
            if (xfigure == null || xfigure.Name != "Figure") return;
            var xgeometry = xfigure.Element("Geometry");
            if (xgeometry == null) return;
            Geometry.SetXml(xgeometry);
            var xstyle = xfigure.Element("Style");
            if (xstyle == null) return;
            Style.SetXml(xstyle);
        }

        public Figure DeepCopy()
        {
            var fig = new Figure
            {
                Style = Style.DeepCopy(),
                Geometry = Geometry.DeepCopy()
            };
            return fig;
        }

        /// <summary>
        /// Подсветка фигуры
        /// </summary>
        /// <param name="state"></param>
        public virtual void DrawGlowed(bool state = true)
        {
            if (state)
            {
                if (RendererDecorator.IsNotContainsDecorator(this, typeof(GlowRendererDecorator)))
                {
                    if (Renderer.AllowedDecorators.HasFlag(AllowedRendererDecorators.Glow))
                        Renderer = new GlowRendererDecorator(Renderer) { Color = Style.BorderStyle.Color };
                }
            }
            else
            {
                if (RendererDecorator.IsContainsDecorator(this, typeof(GlowRendererDecorator)))
                {
                    if (RendererDecorator.ContainsAnyDecorator(Renderer))
                        Renderer = RendererDecorator.GetBaseRenderer(Renderer);
                }
            }
        }

        /// <summary>
        /// Контрур фигуры содержит искомую точку
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool Contains(PointF point, float kf) 
        {
            using (var path = GetRendererPath())
            {
                using (var pen = new Pen(Color.Black, 2f * kf))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    return path.IsOutlineVisible(point, pen);
                }
            }
        }

        /// <summary>
        /// Предоставление геометрии для рисования
        /// </summary>
        /// <returns>Путь для рисования</returns>
        public virtual GraphicsPath GetRendererPath()
        {
            // создаём копию геометрии фигуры
            var path = (GraphicsPath)Geometry.Path.Clone();
            return path;
        }
    }

    public class VertexMarker : Marker
    {
        public int Index { get; set; }
        public Figure Owner { get; set; }
    }

    public enum MarkerType
    {
        Scale,
        SizeX,
        SizeY,
        Rotate,
        SkewX,
        SkewY,
        Vertex,
        Gradient,
        ControlBezier,
        Warp
    }
}
