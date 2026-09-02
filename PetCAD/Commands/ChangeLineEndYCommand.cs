using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeLineEndYCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastEndY;
        private readonly float endY;

        public ChangeLineEndYCommand(Figure figure, float endY)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lastEndY = lineGeometry.EndPoint.Y;
            this.endY = endY;
        }

        public string Name => "Изменить конечную координату Y отрезка";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lineGeometry.Points[1] = new System.Drawing.PointF(lineGeometry.Points[1].X, endY);
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lineGeometry.Points[1] = new System.Drawing.PointF(lineGeometry.Points[1].X, lastEndY);
        }
    }
}
