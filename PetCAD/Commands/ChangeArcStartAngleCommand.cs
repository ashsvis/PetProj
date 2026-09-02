using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeArcStartAngleCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastStartAngle;
        private readonly float startAngle;

        public ChangeArcStartAngleCommand(Figure figure, float startAngle)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                lastStartAngle = arcGeometry.StartAngle;
            this.startAngle = startAngle;
        }

        public string Name => "Изменить начальный угол окружности дуги";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.StartAngle = startAngle;
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.StartAngle = lastStartAngle;
        }
    }
}
