using System.Drawing;

namespace PetCAD.Geometries
{
    public interface IRotateGeometry
    {
        void Rotate(PointF baseRotatePoint, float angle);
    }
}
