using PetProj.Geometries;
using PetProj.Renderers;
using PetProj.Styles;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

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
        public virtual bool Contains(PointF point) 
        {
            using (var path = GetRendererPath())
            {
                using (var pen = new Pen(Color.Black, 3))
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
