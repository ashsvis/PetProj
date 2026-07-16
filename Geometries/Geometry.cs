using System.Drawing.Drawing2D;

namespace PetProj.Geometries
{
    /// <summary>
    /// Класс-основа для задания геометрии фигуры
    /// </summary>
    public abstract class Geometry
    {
        public string Name { get; set; }

        /// <summary>
        /// Предоставление пути для рисования фигуры
        /// </summary>
        public abstract GraphicsPath Path { get; }

        /// <summary>
        /// Допустимые операции над геометрией
        /// </summary>
        public abstract AllowedGeometryOperations AllowedOperations { get; }

    }
}
