using PetCAD.Common;
using PetCAD.Figures;

namespace PetCAD.Commands
{
    public class ChangeBorderOpacityCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly int lastOpacity;
        private readonly int opacity;

        public ChangeBorderOpacityCommand(Figure figure, int opacity)
        {
            changedFigure = figure;
            lastOpacity = figure.Style.BorderStyle.Opacity;
            this.opacity = opacity;
        }

        public string Name => "Изменить прозрачность линий фигуры";

        public void Execute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.Opacity = opacity;
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.Opacity = lastOpacity;
        }
    }
}
