using System.Drawing;

namespace PetCAD.Geometries
{
    public interface ICircleGeometry
    {
        PointF CenterPoint { get; set; }
        float Radius { get; set; }
    }
}
