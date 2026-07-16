using System.Drawing;
using System.Windows.Forms;

namespace PetProj.Figures
{
    public class Marker : Figure
    {
        public Cursor Cursor;
        public PointF Position { get; set; }
        public PointF AnchorPosition { get; set; }
        public MarkerType MarkerType { get; set; }
    }
}
