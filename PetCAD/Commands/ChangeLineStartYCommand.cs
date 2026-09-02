using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeLineStartYCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastStartY;
        private readonly float startY;

        public ChangeLineStartYCommand(Figure figure, float startY)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lastStartY = lineGeometry.StartPoint.Y;
            this.startY = startY;
        }

        public string Name => "Изменить начальную координату Y отрезка";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lineGeometry.Points[0] = new System.Drawing.PointF(lineGeometry.Points[0].X, startY);
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lineGeometry.Points[0] = new System.Drawing.PointF(lineGeometry.Points[0].X, lastStartY);
        }
    }
}
