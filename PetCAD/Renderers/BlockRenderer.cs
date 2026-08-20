using PetCAD.Figures;
using PetCAD.Geometries;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PetCAD.Renderers
{
    /// <summary>
    /// Класс рисовальщика блока
    /// </summary>
    public class BlockRenderer : Renderer
    {
        /// <summary>
        /// Метод отрисовки блока на канве
        /// </summary>
        /// <param name="graphics">Канва для рисования</param>
        /// <param name="block">Фигура со свойствами для рисования</param>
        public override void Render(Graphics graphics, Figure block)
        {
            if (block.Geometry is BlockGeometry geometry)
            {
                foreach (var fig in geometry.GetPreparedFigures())
                {
                    var bounds = fig.Geometry?.Bounds ?? RectangleF.Empty;
                    if (bounds.Width != 0 || bounds.Height != 0)
                    {
                        using (var path = fig.GetRendererPath())
                        {
                            path.Transform(((BlockReference)block).Transformation);
                            using (var pen = fig.Style.BorderStyle.GetPen(fig))
                                graphics.DrawPath(pen, path);
                        }
                    }
                }
            }
        }

        public override Renderer DeepCopy()
        {
            return new BlockRenderer();
        }

        /// <summary>
        /// Свойство возвращает ограничения для подключения декораторов
        /// </summary>
        public override AllowedRendererDecorators AllowedDecorators
        {
            get { return AllowedRendererDecorators.All; }
        }
    }
}
