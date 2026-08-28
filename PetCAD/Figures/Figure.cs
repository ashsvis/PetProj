using PetCAD.Common;
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
            Transformation = new Matrix();
            Renderer = new DefaultRenderer();
        }

        /// <summary>
        /// Свойство стиля рисования фигуры
        /// </summary>
        public Style Style { get; protected set; }

        /// <summary>
        /// Свойство источника геометрии фигуры
        /// </summary>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Трансформация (масштаб, поворот, смещение)
        /// </summary>
        public Matrix Transformation { get; set; }

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
            if (!Transformation.IsIdentity)
            {
                var elements = string.Join(", ", Transformation.Elements);
                xfigure.Add(new XAttribute("Matrix", elements));
            }
            return xfigure;
        }

        public static void SetXml(XElement xfigure, 
            Func<string, Figure> figureFunc, Func<string, Geometry> geometryFunc, Func<string, Renderer> rendererFunc)
        {
            if (xfigure == null || xfigure.Name != "Figure") return;
            var xgeometry = xfigure.Element("Geometry");
            if (xgeometry == null) return;
            var name = xgeometry.Attribute("Name")?.Value;
            var figure = figureFunc(name);
            if (figure == null) return;
            figure.Geometry = geometryFunc(name);
            figure.Geometry?.SetXml(xgeometry);
            var xtransform = xfigure.Attribute("Matrix")?.Value;
            if (xtransform != null)
            {
                var xelements = xtransform.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                if (xelements.Length == 6)
                {
                    float[] m = new float[6];
                    for (var i = 0; i < xelements.Length; i++ )
                        m[i] = ParseHelper.ParseSingle(xelements[i], 0f);
                    figure.Transformation = new Matrix(m[0], m[1], m[2], m[3], m[4], m[5]);
                }
            }
            figure.Renderer = rendererFunc(name);
            var xstyle = xfigure.Element("Style");
            if (xstyle == null) return;
            figure.Style.SetXml(xstyle);
        }

        public virtual Figure DeepCopy()
        {
            var fig = new Figure
            {
                Style = Style.DeepCopy(),
                Transformation = Transformation.Clone(),
                Renderer = Renderer.DeepCopy()
            };
            fig.Geometry = Geometry.DeepCopy(fig);
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
