using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeArcCenterYCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastCenterY;
        private readonly float centerY;

        public ChangeArcCenterYCommand(Figure figure, float centerY)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                lastCenterY = arcGeometry.CenterPoint.Y;
            this.centerY = centerY;
        }

        public string Name => "Изменить координату Y центра окружности дуги";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.CenterPoint = new System.Drawing.PointF(arcGeometry.CenterPoint.X, centerY);
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.CenterPoint = new System.Drawing.PointF(arcGeometry.CenterPoint.X, lastCenterY);
        }
    }
}
