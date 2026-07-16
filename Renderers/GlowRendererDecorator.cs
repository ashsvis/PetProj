using PetProj.Figures;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PetProj.Renderers
{
    /// <summary>
    /// Класс рисовальщика фигуры со "свечением"
    /// </summary>
    public class GlowRendererDecorator : RendererDecorator
    {
        private readonly Renderer renderer;

        /// <summary>
        /// Цвет для свечения
        /// </summary>
        public Color Color { get; set; }

        public GlowRendererDecorator(Renderer renderer)
            : base(renderer)
        {
            this.renderer = renderer;
            Color = Color.White;
        }

        public override void Render(Graphics graphics, Figure figure)
        {
            var baseRenderer = GetBaseRenderer(figure.Renderer) as IRendererPath;
            // получаем путь для рисования, трансформированный методом фигуры
            using (var path = baseRenderer != null
            ? baseRenderer.GetRendererPath(graphics, figure) : figure.GetRendererPath())
            {
                // то получаем карандаш из стиля рисования фигуры
                using (var pen = new Pen(Color))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    var color = Color;
                    for (var i = 0; i < 8; i++)
                    {
                        pen.Color = color;
                        pen.Width += 1; //4;
                        graphics.DrawPath(pen, path);
                        color = Color.FromArgb(color.A / 2, color);
                    }
                }
            }
            renderer.Render(graphics, figure);
        }

        /// <summary>
        /// Свойство возвращает ограничения для подключения декораторов
        /// </summary>
        public override AllowedRendererDecorators AllowedDecorators
        {
            get { return AllowedRendererDecorators.All ^ (AllowedRendererDecorators.Glow); }
        }

        public override string ToString()
        {
            return "Glow";
        }

    }
}
