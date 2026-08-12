using PetCAD.Common;
using PetCAD.Figures;

namespace PetCAD.Commands
{
    public class ChangeBorderWidthCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly float lastWidth;
        private readonly float width;

        public ChangeBorderWidthCommand(Figure figure, float width)
        {
            changedFigure = figure;
            lastWidth = figure.Style.BorderStyle.Width;
            this.width = width;
        }

        public string Name => "Изменить толщину границы фигуры";

        public void Execute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.Width = width;
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.Width = lastWidth;
        }
    }
}
