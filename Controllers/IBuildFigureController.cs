using System.Windows.Forms;

namespace PetProj.Controllers
{
    public interface IBuildFigureController
    {
        void Container_Paint(object sender, PaintEventArgs e);
        void Container_MouseDown(object sender, MouseEventArgs e);
        void Container_MouseMove(object sender, MouseEventArgs e);

        void SetParameters(string[] strings);
    }
}