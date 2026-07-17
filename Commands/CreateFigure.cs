using PetProj.Common;
using PetProj.Figures;
using System;
using System.Collections.Generic;

namespace PetProj.Commands
{
    public class CreateFigure : ICommand
    {
        private readonly List<Figure> figures;
        private readonly Figure figure;

        public CreateFigure(List<Figure> figures, Figure figure)
        {
            this.figures = figures;
            this.figure = figure;
        }

        public string Name => "Добавить новую фигуру";

        public void Execute()
        {
            if (figures == null || figure == null) return;
            figures.Add(figure);
        }

        public void UnExecute()
        {
            if (figures == null || figure == null) return;
            figures.Remove(figure);
        }
    }
}
