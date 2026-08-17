using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;
using System.Collections.Generic;
using System.Drawing;

namespace PetCAD.Commands
{
    public class ScaleFiguresCommand : ICommand
    {
        private readonly List<(Figure, PointF, float)> scaleOffsets;

        public ScaleFiguresCommand(List<(Figure, PointF, float)> scaleOffsets)
        {
            this.scaleOffsets = scaleOffsets;
        }

        public string Name => "Изменить масштаб нескольким фигурам";

        public void Execute()
        {
            if (scaleOffsets == null) return;
            foreach (var (figure, point, zoom) in scaleOffsets)
            {
                if (figure.Geometry is IScaleGeometry geometry)
                {
                    geometry.Scale(point, zoom); 
                }
            }
        }

        public void UnExecute()
        {
            if (scaleOffsets == null) return;
            foreach (var (figure, point, zoom) in scaleOffsets)
            {
                if (figure.Geometry is IScaleGeometry geometry)
                { 
                    geometry.Scale(point, 1 / zoom); 
                }
            }
        }
    }
}
