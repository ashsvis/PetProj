using PetCAD.Geometries;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PetCAD.Figures
{
    /// <summary>
    /// Строит компоненты фигуры
    /// </summary>
    public static class FigureBuilder
    {
        /// <summary>
        /// Построение пути для отрезка линии
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        /// <param name="startPoint">Начальная точка</param>
        public static void BuildLineGeometry(Figure figure, PointF startPoint)
        {
            figure.Geometry = new LineGeometry(startPoint)
            {
                Name = "Segment",
            };
        }

        /// <summary>
        /// Построение пути для дуги
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        /// <param name="center">Центральная точка оружности дуги</param>
        /// <param name="radius">Радиус окружности</param>
        /// <param name="startAngle">Начальный угол</param>
        /// <param name="sweepAngle">Прибавочный угол</param>
        public static void BuildArcGeometry(Figure figure, PointF center, float radius, float startAngle, float sweepAngle)
        {
            figure.Geometry = new ArcGeometry(center, radius, startAngle, sweepAngle)
            {
                Name = "Arc",
            };
        }

        public static void BuildLayerGeomentry(Figure layer)
        {
            var path = new GraphicsPath();
            layer.Geometry = new PrimitiveGeometry(path, AllowedGeometryOperations.None) { Name = "Layer" };
        }
    }

}
