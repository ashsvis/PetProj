using System.Drawing;

namespace PetCAD.Geometries
{
    public interface IScaleGeometry
    {
        void Scale(PointF basePoint, float zoom);
    }
}
