using PetProj.Common;
using PetProj.Figures;
using System.Collections.Generic;

namespace PetProj.Commands
{
    public class RemoveFigureCommand : ICommand
    {
        private readonly List<Figure> figures;
        private readonly Figure figure;
        private Figure prevFigure;


        public RemoveFigureCommand(List<Figure> figures, Figure figure)
        {
            this.figures = figures;
            this.figure = figure;
        }

        public string Name => "Удалить фигуру";

        public void Execute()
        {
            if (figures == null || figure == null) return;
            prevFigure = figure;
            figures.Remove(figure);
        }

        public void UnExecute()
        {
            if (figures == null || prevFigure == null) return;
            figures.Add(prevFigure);
        }
    }
}
