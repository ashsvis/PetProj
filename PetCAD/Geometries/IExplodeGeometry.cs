using PetCAD.Figures;

namespace PetCAD.Geometries
{
    public interface IExplodeGeometry
    {
        Figure[] Explode(bool forMarkers = false);
    }
}
