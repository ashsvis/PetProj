using PetProj.Geometries;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PetProj.Figures
{
    /// <summary>
    /// Строит компоненты фигуры
    /// </summary>
    public static class FigureBuilder
    {
        //private const int MARKER_SIZE = 8;

        /// <summary>
        /// Построение пути для ломаной линии
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        /// <param name="startPoint">Начальная точка</param>
        public static void BuildLineGeometry(Figure figure, PointF startPoint)
        {
            figure.Geometry = new LineGeometry(startPoint)
            {
                Name = "Отрезок",
            };
        }

        public static void BuildLayerGeomentry(Figure layer)
        {
            var path = new GraphicsPath();
            layer.Geometry = new PrimitiveGeometry(path, AllowedGeometryOperations.None) { Name = "Слой" };
        }
    }

}
