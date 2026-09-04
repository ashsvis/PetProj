using PetCAD.Makers;

namespace PetCAD.Geometries
{
    public interface IMoveMarker
    {
        void MoveMarker(Marker maker, int index, float offsetX, float offsetY);
        bool CanMoveMarker(Marker maker, int index, float offsetX, float offsetY);
    }
}
