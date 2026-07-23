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
        private const int MARKER_SIZE = 8;

        /// <summary>
        /// Построение пути для ломаной линии
        /// </summary>
        /// <param name="figure">Фигура для присвоения геометрии</param>
        /// <param name="startPoint">Начальная точка</param>
        /// <param name="isClosed">Замкнутая</param>
        /// <param name="isSmoothed">Сглаживание</param>
        public static void BuildAddLineGeometry(Figure figure, PointF startPoint, bool isClosed = false, bool isSmoothed = false)
        {
            figure.Style.BorderStyle.Width = 1f;
            figure.Geometry = new AddLineGeometry(startPoint)
            {
                Name = "AddLine",
                IsClosed = isClosed,
                IsSmoothed = isSmoothed
            };
        }

        /// <summary>
        /// Построение пути для маркера
        /// </summary>
        /// <param name="marker"></param>
        public static void BuildMarkerGeometry(Figure marker)
        {
            var path = new GraphicsPath();
            marker.Style.BorderStyle.Width = 0;
            switch (((Marker)marker).MarkerType)
            {
                case MarkerType.ControlBezier:
                    // управляющие маркеры Безье рисуем круглыми
                    path.AddEllipse(new RectangleF(-MARKER_SIZE / 2f, -MARKER_SIZE / 2f, MARKER_SIZE, MARKER_SIZE));
                    break;
                default:
                    // здесь задаём размер макера в 5 единиц и смешение от центра маркера в -2 единицы
                    path.AddRectangle(new RectangleF(-MARKER_SIZE / 2f, -MARKER_SIZE / 2f, MARKER_SIZE, MARKER_SIZE));
                    break;
            }
            marker.Geometry = new PrimitiveGeometry(path, AllowedGeometryOperations.All ^
                (AllowedGeometryOperations.Size | AllowedGeometryOperations.Rotate | AllowedGeometryOperations.Select |
                 AllowedGeometryOperations.Skew | AllowedGeometryOperations.Vertex | AllowedGeometryOperations.Pathed | AllowedGeometryOperations.Warp))
            { Name = "Marker" };
        }

        public static void BuildLayerGeomentry(Figure layer)
        {
            var path = new GraphicsPath();
            path.AddLine(Point.Empty, Point.Empty);
            layer.Geometry = new PrimitiveGeometry(path, AllowedGeometryOperations.None) { Name = "Layer" };
        }
    }

}
