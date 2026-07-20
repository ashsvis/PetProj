using PetProj.Common;
using PetProj.Figures;
using PetProj.Geometries;
using System.Collections.Generic;
using System.Drawing;

namespace PetProj.Commands
{
    public class MoveFiguresCommand : ICommand
    {
        private readonly List<(Figure, PointF)> movedOffsets;

        public MoveFiguresCommand(List<(Figure,PointF)> movedOffsets)
        {
            this.movedOffsets = movedOffsets;
        }

        public string Name => "Переместить несколько фигур";

        public void Execute()
        {
            if (movedOffsets == null) return;
            foreach (var (figure, offset) in movedOffsets)
            {
                if (figure.Geometry is IMoveGeometry geometry)
                    geometry.Move(offset.X, offset.Y);
            }
        }

        public void UnExecute()
        {
            if (movedOffsets == null) return;
            foreach (var (figure, offset) in movedOffsets)
            {
                if (figure.Geometry is IMoveGeometry geometry)
                    geometry.Move(-offset.X, -offset.Y);
            }
        }
    }
}
