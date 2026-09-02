using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeArcRadiusCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastRadius;
        private readonly float radius;

        public ChangeArcRadiusCommand(Figure figure, float radius)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                lastRadius = arcGeometry.Radius;
            this.radius = radius;
        }

        public string Name => "Изменить радиус окружности дуги";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.Radius = radius;
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.Radius = lastRadius;
        }
    }
}
