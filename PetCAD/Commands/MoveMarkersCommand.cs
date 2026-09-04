using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;
using PetCAD.Makers;
using System.Collections.Generic;
using System.Drawing;

namespace PetCAD.Commands
{
    public class MoveMarkersCommand : ICommand
    {
        private readonly List<(Figure, PointF, Marker, int)> movedOffsets;

        public MoveMarkersCommand(List<(Figure, PointF, Marker, int)> movedOffsets)
        {
            this.movedOffsets = movedOffsets;
        }

        public string Name => "Переместить несколько маркеров фигур";

        public void Execute()
        {
            if (movedOffsets == null) return;
            foreach (var (figure, offset, marker, index) in movedOffsets)
            {
                if (figure.Geometry is IMoveMarker mover)
                    mover.MoveMarker(marker, index, offset.X, offset.Y);
            }
        }

        public void UnExecute()
        {
            if (movedOffsets == null) return;
            foreach (var (figure, offset, marker, index) in movedOffsets)
            {
                if (figure.Geometry is IMoveMarker mover)
                    mover.MoveMarker(marker, index, -offset.X, -offset.Y);
            }
        }
    }
}
