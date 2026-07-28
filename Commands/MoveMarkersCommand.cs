using PetProj.Common;
using PetProj.Figures;
using PetProj.Geometries;
using System.Collections.Generic;
using System.Drawing;

namespace PetProj.Commands
{
    public class MoveMarkersCommand : ICommand
    {
        private readonly List<(Figure, PointF, int)> movedOffsets;

        public MoveMarkersCommand(List<(Figure, PointF, int)> movedOffsets)
        {
            this.movedOffsets = movedOffsets;
        }

        public string Name => "Переместить несколько маркеров фигур";

        public void Execute()
        {
            if (movedOffsets == null) return;
            foreach (var (figure, offset, index) in movedOffsets)
            {
                if (figure.Geometry is IMoveMarker marker)
                    marker.Move(index, offset.X, offset.Y);
            }
        }

        public void UnExecute()
        {
            if (movedOffsets == null) return;
            foreach (var (figure, offset, index) in movedOffsets)
            {
                if (figure.Geometry is IMoveMarker marker)
                    marker.Move(index, -offset.X, -offset.Y);
            }
        }
    }
}
