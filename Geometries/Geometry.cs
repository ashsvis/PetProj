using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml.Linq;

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

        public XElement GetXml()
        {
            var xfill = new XElement("Geometry");
            xfill.Add(new XAttribute("Name", Name));
            var xpath = new XElement("Path");
            xpath.Add(new XAttribute("Points", string.Join(",", Path.PathPoints.Select(p => p.ToString()))));
            xpath.Add(new XAttribute("Types", string.Join(",", Path.PathTypes.Select(t => t.ToString()))));
            xfill.Add(xpath);
            return xfill;
        }

        /// <summary>
        /// Допустимые операции над геометрией
        /// </summary>
        public abstract AllowedGeometryOperations AllowedOperations { get; }

    }
}
