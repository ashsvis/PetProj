using PetCAD.Geometries;
using PetCAD.Renderers;
using PetCAD.Styles;
using System;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace PetCAD.Figures
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

        public override string ToString()
        {
            return $"{Geometry.Name} {Geometry.Bounds}";
        }

        public  XElement GetXml()
        {
            var xfigure = new XElement("Figure");
            xfigure.Add(Geometry.GetXml());
            var xStyle = Style.GetXml();
            if (xStyle != null)
                xfigure.Add(xStyle);
            return xfigure;
        }

        public void SetXml(XElement xfigure, Func<string, Geometry> geometry, Func<string, Renderer> renderer)
        {
            if (xfigure == null || xfigure.Name != "Figure") return;
            var xgeometry = xfigure.Element("Geometry");
            if (xgeometry == null) return;
            var name = xgeometry.Attribute("Name")?.Value;
            Geometry = geometry(name);
            Geometry?.SetXml(xgeometry);
            Renderer = renderer(name);
            var xstyle = xfigure.Element("Style");
            if (xstyle == null) return;
            Style.SetXml(xstyle);
        }

        public Figure DeepCopy()
        {
            var fig = new Figure
            {
                Style = Style.DeepCopy(),
                Geometry = Geometry.DeepCopy(),
                Renderer = Renderer.DeepCopy()
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
        /// Предоставление геометрии для рисования
        /// </summary>
        /// <returns>Путь для рисования</returns>
        public virtual GraphicsPath GetRendererPath()
        {
            // создаём копию геометрии фигуры
            var path = (GraphicsPath)Geometry?.Path.Clone();
            return path ?? new GraphicsPath();
        }
    }
}
