using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeLineStartXCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastStartX;
        private readonly float startX;

        public ChangeLineStartXCommand(Figure figure, float startX)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lastStartX = lineGeometry.StartPoint.X;
            this.startX = startX;
        }

        public string Name => "Изменить начальную координату X отрезка";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lineGeometry.Points[0] = new System.Drawing.PointF(startX, lineGeometry.Points[0].Y);
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lineGeometry.Points[0] = new System.Drawing.PointF(lastStartX, lineGeometry.Points[0].Y);
        }
    }
}
