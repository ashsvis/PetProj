using PetCAD.Figures;
using System.Drawing;

namespace PetCAD.Renderers
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

        public virtual Renderer DeepCopy()
        {
            return new DefaultRenderer();
        }

        /// <summary>
        /// Допустимые операции над геометрией
        /// </summary>
        public abstract AllowedRendererDecorators AllowedDecorators { get; }
    }
}
