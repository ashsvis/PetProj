using PetCAD.Geometries;
using System.Collections.Generic;
using System.Drawing;

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
            figure.Geometry = new LineGeometry(figure, startPoint)
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
            figure.Geometry = new ArcGeometry(figure, center, radius, startAngle, sweepAngle)
            {
                Name = "Arc",
            };
        }

        /// <summary>
        /// Создание геометрии блока из набора фигур и базовой точки вставки
        /// </summary>
        /// <param name="name"></param>
        /// <param name="block"></param>
        /// <param name="basePoint"></param>
        /// <param name="figures"></param>
        public static void BuildBlockGeometry(string name, Figure block, PointF basePoint, Figure[] figures)
        {
            var zeroBasedFigures = new List<Figure>();
            foreach (var figure in figures)
            {
                var zbf = figure.DeepCopy();
                if (zbf.Geometry is IMoveGeometry mover)
                {
                    mover.Move(-basePoint.X, -basePoint.Y);
                    zeroBasedFigures.Add(zbf);
                }
            }
            block.Geometry = new BlockGeometry(block, name, basePoint, 1f, 0f, zeroBasedFigures.ToArray());
        }
    }

}
