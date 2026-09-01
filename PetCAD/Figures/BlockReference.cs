using PetCAD.Geometries;
using PetCAD.Renderers;
using PetCAD.Styles;
using System.Drawing.Drawing2D;

namespace PetCAD.Figures
{
    public class BlockReference : Figure
    {
        public BlockReference()
        {
            Style = new Style();
            Renderer = new BlockRenderer();
        }

        public override Figure DeepCopy()
        {
            var fig = new BlockReference
            {
                Transformation = Transformation.Clone(),
                Style = Style.DeepCopy(),
                Renderer = Renderer.DeepCopy()
            };
            fig.Geometry = Geometry.DeepCopy(fig);
            return fig;
        }

        /// <summary>
        /// Предоставление геометрии для рисования
        /// </summary>
        /// <returns>Путь для рисования</returns>
        public override GraphicsPath GetRendererPath()
        {
            // создаём копию геометрии фигуры
            var path = new GraphicsPath();
            var blockGeometry = (BlockGeometry)Geometry;
            foreach (var f in blockGeometry.GetZeroBasedFigures())
                path.AddPath(f.GetRendererPath(), false);
            
            path.Transform(Transformation);
            
            return path;
        }

    }

}
