using PetCAD.Figures;
using PetCAD.Makers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace PetCAD.Geometries
{
    /// <summary>
    /// Класс-основа для задания геометрии фигуры
    /// </summary>
    public abstract class Geometry
    {
        public string Name { get; set; }
        public Figure Owner { get; set; }

        /// <summary>
        /// Предоставление пути для рисования фигуры
        /// </summary>
        public abstract GraphicsPath Path { get; }

        public abstract RectangleF Bounds { get; }
        public string Description { get; set; }

        public abstract XElement GetXml();
        public abstract void SetXml(XElement xgeometry);
        public abstract Geometry DeepCopy(Figure figure);

        /// <summary>
        /// Допустимые операции над геометрией
        /// </summary>
        public abstract AllowedGeometryOperations AllowedOperations { get; }

        public abstract Marker[] GetGeometryMarkers();
        public abstract Marker[] GetBindingMarkers(AllowedObjectBindings allowed, PointF basePoint, float baseAngle);

    }
}
