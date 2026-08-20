using System.Windows.Forms;

namespace PetCAD.Controllers
{
    public interface IBuildFigure
    {
        void Container_Paint(object sender, PaintEventArgs e);
        void Container_MouseDown(object sender, MouseEventArgs e);
        void Container_MouseMove(object sender, MouseEventArgs e);

        void SetParameters(string[] strings);
    }
}