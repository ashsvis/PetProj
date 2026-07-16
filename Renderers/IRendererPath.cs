using PetProj.Figures;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PetProj.Renderers
{
    public interface IRendererPath
    {
        GraphicsPath GetRendererPath(Graphics graphics, Figure figure);
    }
}
