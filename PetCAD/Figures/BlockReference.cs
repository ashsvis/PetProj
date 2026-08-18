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
            Geometry = new BlockGeometry();
            Renderer = new BlockRenderer();
        }

        public override Figure DeepCopy()
        {
            var fig = new BlockReference
            {
                Style = Style.DeepCopy(),
                Geometry = Geometry.DeepCopy(),
                Renderer = Renderer.DeepCopy()
            };
            return fig;
        }

        /// <summary>
        /// Предоставление геометрии для рисования
        /// </summary>
        /// <returns>Путь для рисования</returns>
        public override GraphicsPath GetRendererPath()
        {
            // создаём копию геометрии фигуры
            var path = new GraphicsPath(); //(GraphicsPath)Geometry?.Path.Clone();
            var m = new Matrix();
            var basePoint = ((BlockGeometry)Geometry).InsertPoint;
            var kf = ((BlockGeometry)Geometry).ScaleFactor;
            var angle = ((BlockGeometry)Geometry).Angle;
            m.Translate(-basePoint.X, -basePoint.Y, MatrixOrder.Append);
            m.Rotate(angle, MatrixOrder.Append);
            m.Scale(kf, kf, MatrixOrder.Append);
            m.Translate(basePoint.X, basePoint.Y, MatrixOrder.Append);
            path.Transform(m);
            return path;
        }

    }

}
