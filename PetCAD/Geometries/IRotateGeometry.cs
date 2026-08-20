using System.Drawing;

namespace PetCAD.Geometries
{
    public interface IRotateGeometry
    {
        void Rotate(PointF centerRotatePoint, float angel);
    }
}
