using System.Windows.Forms;

namespace PetCAD.Figures
{
    public class CenterMarker : Marker
    {
        public override Cursor Cursor { get; set; } = Cursors.SizeAll;
        public override AllowedMarkerOperations AllowedOperations { get { return AllowedMarkerOperations.None ^ AllowedMarkerOperations.MoveOwner; } }
    }
}
