using System.Drawing;

namespace PetCAD.Geometries
{
    public interface IRotateGeometry
    {
        void Rotate(PointF basePoint, float angel);
    }
}
