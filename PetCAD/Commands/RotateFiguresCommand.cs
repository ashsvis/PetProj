using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PetCAD.Commands
{
    public class RotateFiguresCommand : ICommand
    {
        private readonly List<(Figure, PointF, float)> rotateOffsets;

        public RotateFiguresCommand(List<(Figure, PointF, float)> rotateOffsets)
        {
            this.rotateOffsets = rotateOffsets;
        }

        public string Name => "Поворот нескольких фигур";

        public void Execute()
        {
            if (rotateOffsets == null) return;
            foreach (var (figure, point, angle) in rotateOffsets)
            {
                if (figure.Geometry is IRotateGeometry geometry)
                {
                    geometry.Rotate(point, angle);
                }
            }
        }

        public void UnExecute()
        {
            if (rotateOffsets == null) return;
            foreach (var (figure, point, angle) in rotateOffsets)
            {
                if (figure.Geometry is IRotateGeometry geometry)
                {
                    geometry.Rotate(point, -angle);
                }
            }
        }
    }
}
