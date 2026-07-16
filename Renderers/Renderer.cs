using PetProj.Figures;
using System.Drawing;

namespace PetProj.Renderers
{
    /// <summary>
    /// Класс рисовальщика фигуры
    /// </summary>
    public abstract class Renderer
    {
        /// <summary>
        /// Метод отрисовки фигуры на канве
        /// </summary>
        /// <param name="graphics">Канва для рисования</param>
        /// <param name="figure">Фигура со свойствами для рисования</param>
        public abstract void Render(Graphics graphics, Figure figure);

        /// <summary>
        /// Допустимые операции над геометрией
        /// </summary>
        public abstract AllowedRendererDecorators AllowedDecorators { get; }
    }
}
