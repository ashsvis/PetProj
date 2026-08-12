using PetCAD.Common;
using PetCAD.Figures;
using System.Drawing.Drawing2D;

namespace PetCAD.Commands
{
    public class ChangeBorderDashStyleCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly DashStyle lastDashStyle;
        private readonly DashStyle dashStyle;

        public ChangeBorderDashStyleCommand(Figure figure, DashStyle dashStyle)
        {
            changedFigure = figure;
            lastDashStyle = figure.Style.BorderStyle.DashStyle;
            this.dashStyle = dashStyle;
        }

        public string Name => "Изменить стиль начертания линий фигуры";

        public void Execute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.DashStyle = dashStyle;
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.DashStyle = lastDashStyle;
        }
    }
}
