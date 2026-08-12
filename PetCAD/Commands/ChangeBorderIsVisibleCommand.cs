using PetCAD.Common;
using PetCAD.Figures;

namespace PetCAD.Commands
{
    public class ChangeBorderIsVisibleCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly bool lastIsVisible;
        private readonly bool isVisible;

        public ChangeBorderIsVisibleCommand(Figure figure, bool isVisible)
        {
            changedFigure = figure;
            lastIsVisible = figure.Style.BorderStyle.IsVisible;
            this.isVisible = isVisible;
        }

        public string Name => "Изменить видимость линий фигуры";

        public void Execute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.IsVisible = isVisible;
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.IsVisible = lastIsVisible;
        }
    }
}
