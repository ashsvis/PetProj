using PetCAD.Figures;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PetCAD.Renderers
{
    public interface IRendererPath
    {
        GraphicsPath GetRendererPath(Graphics graphics, Figure figure);
    }
}
