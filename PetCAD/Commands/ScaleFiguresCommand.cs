using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;
using System.Collections.Generic;
using System.Drawing;

namespace PetCAD.Commands
{
    public class ScaleFiguresCommand : ICommand
    {
        private readonly List<(Figure, PointF)> movedOffsets;

        public ScaleFiguresCommand(List<(Figure, PointF)> movedOffsets)
        {
            this.movedOffsets = movedOffsets;
        }

        public string Name => "Изменить масштаб нескольким фигурам";

        public void Execute()
        {
            if (movedOffsets == null) return;
            foreach (var (figure, offset) in movedOffsets)
            {
                if (figure.Geometry is IScaleGeometry geometry)
                    geometry.Scale(offset.X, offset.Y);
            }
        }

        public void UnExecute()
        {
            if (movedOffsets == null) return;
            foreach (var (figure, offset) in movedOffsets)
            {
                if (figure.Geometry is IScaleGeometry geometry)
                    geometry.Scale(-offset.X, -offset.Y);
            }
        }
    }
}
