using System.Windows.Forms;

namespace PetProj.Figures
{
    public class VertexMarker : Marker
    {
        public override Cursor Cursor { get; set; } = Cursors.Hand;
        public int Index { get; set; }
        public override AllowedMarkerOperations AllowedOperations { get { return AllowedMarkerOperations.None ^ AllowedMarkerOperations.MoveVertex; } }
    }
}
