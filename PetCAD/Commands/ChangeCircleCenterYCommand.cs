using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeCircleCenterYCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastCenterY;
        private readonly float centerY;

        public ChangeCircleCenterYCommand(Figure figure, float centerX)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is CircleGeometry circGeometry)
                lastCenterY = circGeometry.CenterPoint.Y;
            this.centerY = centerX;
        }

        public string Name => "Изменить координату Y центра окружности круга";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is CircleGeometry circGeometry)
                circGeometry.CenterPoint = new System.Drawing.PointF(circGeometry.CenterPoint.X, centerY);
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is CircleGeometry circGeometry)
                circGeometry.CenterPoint = new System.Drawing.PointF(circGeometry.CenterPoint.X, lastCenterY);
        }
    }
}
