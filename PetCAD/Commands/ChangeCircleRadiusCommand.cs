using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeCircleRadiusCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastRadius;
        private readonly float radius;

        public ChangeCircleRadiusCommand(Figure figure, float radius)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is CircleGeometry circGeometry)
                lastRadius = circGeometry.Radius;
            this.radius = radius;
        }

        public string Name => "Изменить радиус окружности круга";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is CircleGeometry circGeometry)
                circGeometry.Radius = radius;
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is CircleGeometry circGeometry)
                circGeometry.Radius = lastRadius;
        }
    }
}
