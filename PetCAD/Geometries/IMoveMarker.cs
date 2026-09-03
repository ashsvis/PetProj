namespace PetCAD.Geometries
{
    public interface IMoveMarker
    {
        void MoveMarker(int index, float offsetX, float offsetY);
        bool CanMoveMarker(int index, float offsetX, float offsetY);
    }
}
