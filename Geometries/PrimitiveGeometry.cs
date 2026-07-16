using System;
using System.Drawing.Drawing2D;

namespace PetProj.Geometries
{
    /*
    /// <summary>
    /// Содержит геометрию фиксированной формы
    /// </summary>
    public class PrimitiveGeometry : Geometry, IDisposable
    {
        /// <summary>
        /// Локальное поле для хранения пути
        /// </summary>
        readonly GraphicsPath path = new GraphicsPath();

        /// <summary>
        /// Локальное поле для хранения ограничений для операций
        /// </summary>
        readonly AllowedGeometryOperations allowedOperations;

        /// <summary>
        /// Свойство возвращает путь, указанный в конструкторе
        /// </summary>
        public override GraphicsPath Path { get { return path; } }

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedGeometryOperations AllowedOperations { get { return allowedOperations; } }

        /// <summary>
        /// Конструктор, недоступный вне проекта EditorModel
        /// (только для внутреннего использования)
        /// </summary>
        /// <param name="path">Закрепляемый путь для примитивной геометрии</param>
        /// <param name="allowed">набор прав для операций</param>
        internal PrimitiveGeometry(GraphicsPath path, AllowedGeometryOperations allowed)
        {
            Name = "Primitive";
            // запоминаем переданный в конструкторе путь в локальном поле
            this.path = path;
            // запоминаем ограничения для операций в локальном поле
            allowedOperations = allowed;
        }

        public void Dispose()
        {
            path?.Dispose();
        }

        public override string ToString()
        {
            return Name;
        }
    }
    */
}
