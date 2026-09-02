using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeArcCenterXCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastCenterX;
        private readonly float centerX;

        public ChangeArcCenterXCommand(Figure figure, float centerX)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                lastCenterX = arcGeometry.CenterPoint.X;
            this.centerX = centerX;
        }

        public string Name => "Изменить координату X центра окружности дуги";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.CenterPoint = new System.Drawing.PointF(centerX, arcGeometry.CenterPoint.Y);
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is ArcGeometry arcGeometry)
                arcGeometry.CenterPoint = new System.Drawing.PointF(lastCenterX, arcGeometry.CenterPoint.Y);
        }
    }
}
