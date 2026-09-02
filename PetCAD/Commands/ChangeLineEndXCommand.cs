using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;

namespace PetCAD.Commands
{
    public class ChangeLineEndXCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastEndX;
        private readonly float endX;

        public ChangeLineEndXCommand(Figure figure, float endX)
        {
            changedFigure = figure;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lastEndX = lineGeometry.EndPoint.X;
            this.endX = endX;
        }

        public string Name => "Изменить конечную координату X отрезка";

        public void Execute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lineGeometry.Points[1] = new System.Drawing.PointF(endX, lineGeometry.Points[1].Y);
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            if (changedFigure.Geometry is LineGeometry lineGeometry)
                lineGeometry.Points[1] = new System.Drawing.PointF(lastEndX, lineGeometry.Points[1].Y);
        }
    }
}
