using PetCAD.Common;
using PetCAD.Figures;
using System.Drawing;

namespace PetCAD.Commands
{
    public class ChangeBorderColorCommand : ICommand
    {
        private readonly Figure changedFigure;
        private readonly Color lastColor;
        private readonly Color color;

        public ChangeBorderColorCommand(Figure figure, Color color)
        {
            changedFigure = figure;
            lastColor = figure.Style.BorderStyle.Color;
            this.color = color;
        }

        public string Name => "Изменить цвет линий фигуры";

        public void Execute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.Color = color;
        }

        public void UnExecute()
        {
            if (changedFigure == null) return;
            changedFigure.Style.BorderStyle.Color = lastColor;
        }
    }
}
