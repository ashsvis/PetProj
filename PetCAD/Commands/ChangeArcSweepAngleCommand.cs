using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeArcSweepAngleCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastSweepAngle;
        private readonly float sweepAngle;

        public ChangeArcSweepAngleCommand(Figure figure, float sweepAngle)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                lastSweepAngle = arcGeometry.SweepAngle;
            this.sweepAngle = sweepAngle;
        }

        public string Name => "Изменить полный угол окружности дуги";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.SweepAngle = sweepAngle;
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.SweepAngle = lastSweepAngle;
        }
    }
}
