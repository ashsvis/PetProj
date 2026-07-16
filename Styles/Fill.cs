using PetProj.Figures;
using System.Drawing;

namespace PetProj.Styles
{
    /// <summary>
    /// Класс рисовальщика фона фигуры
    /// </summary>
    public abstract class Fill
    {
        /// <summary>
        /// Величина прозрачности цвета заливки
        /// </summary>
        public virtual int Opacity { get; set; }

        /// <summary>
        /// Цвет для заполнения фона (цвет заливки)
        /// </summary>
        public virtual Color Color { get; set; }

        /// <summary>
        /// Признак возможности заливки фигуры
        /// </summary>
        public virtual bool IsVisible { get; set; }

        public abstract Brush GetBrush(Figure figure);

        /// <summary>
        /// Допустимые операции над заливкой
        /// </summary>
        public abstract AllowedFillDecorators AllowedDecorators { get; }
    }
}
