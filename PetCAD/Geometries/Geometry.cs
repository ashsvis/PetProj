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

        //public virtual RectangleF Bounds => Path.GetBounds();
        public abstract RectangleF Bounds { get; }

        public string Description { get; set; }

        public abstract XElement GetXml();

        //public virtual XElement GetXml()
        //{
        //    var xfill = new XElement("Geometry");
        //    xfill.Add(new XAttribute("Name", Name));
        //    var xpath = new XElement("Path");
        //    xpath.Add(new XAttribute("Points", string.Join(";", Path.PathPoints.Select(p => p.ToString()))));
        //    xpath.Add(new XAttribute("Types", string.Join(";", Path.PathTypes.Select(t => t.ToString()))));
        //    xfill.Add(xpath);
        //    return xfill;
        //}

        public abstract void SetXml(XElement xgeometry);
        //public virtual void SetXml(XElement xgeometry)
        //{
        //    if (xgeometry == null || xgeometry.Name != "Geometry") return;
        //    var name = xgeometry.Attribute("Name")?.Value;
        //    if (string.IsNullOrWhiteSpace(name)) return;
        //    Name = name;
        //    var xpath = xgeometry.Element("Path");
        //    if (xpath == null) return;
        //    var spoints = xpath.Attribute("Points")?.Value;
        //    var stypes = xpath.Attribute("Types")?.Value;
        //    if (!string.IsNullOrWhiteSpace(spoints) && !string.IsNullOrWhiteSpace(stypes))
        //    {
        //        var apoints = spoints.Split(';');
        //        var atypes = spoints.Split(';');
        //        if (apoints.Length > 0 && apoints.Length == atypes.Length)
        //        {
        //            var points = new List<PointF>();
        //            var types = new List<byte>();
        //            for (var i = 0; i < apoints.Length; i++)
        //            {
        //                points.Add(ParseHelper.ParsePointF(apoints[i], PointF.Empty));
        //                types.Add(ParseHelper.ParseByte(atypes[i], 0));
        //            }
        //            Path = new GraphicsPath(points.ToArray(), types.ToArray());
        //        }
        //    }
        //}

        //public abstract void Transform(PointF point, float scale, float rotation);
        public abstract Geometry DeepCopy();

        /// <summary>
        /// Допустимые операции над геометрией
        /// </summary>
        public abstract AllowedGeometryOperations AllowedOperations { get; }

        public abstract Marker[] GetGeometryMarkers();
        public abstract Marker[] GetBindingMarkers(AllowedObjectBindings allowed, PointF basePoint);

    }
}
