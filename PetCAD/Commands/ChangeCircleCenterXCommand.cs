using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeCircleCenterXCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastCenterX;
        private readonly float centerX;

        public ChangeCircleCenterXCommand(Figure figure, float centerX)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is CircleGeometry circGeometry)
                lastCenterX = circGeometry.CenterPoint.X;
            this.centerX = centerX;
        }

        public string Name => "Изменить координату X центра окружности круга";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is CircleGeometry circGeometry)
                circGeometry.CenterPoint = new System.Drawing.PointF(centerX, circGeometry.CenterPoint.Y);
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is CircleGeometry circGeometry)
                circGeometry.CenterPoint = new System.Drawing.PointF(lastCenterX, circGeometry.CenterPoint.Y);
        }
    }
}
