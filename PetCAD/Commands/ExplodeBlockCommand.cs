using PetCAD.Common;
using PetCAD.Figures;
using System.Collections.Generic;

namespace PetCAD.Commands
{
    public class ExplodeBlockCommand : ICommand
    {
        private readonly List<Figure> figures;
        private readonly List<Figure> createdFigures;
        private readonly Figure explodedFigure;

        public ExplodeBlockCommand(List<Figure> figures, List<Figure> createdFigures, Figure explodedFigure)
        {
            this.figures = figures;
            this.createdFigures = createdFigures;
            this.explodedFigure = explodedFigure;
        }

        public string Name => "Расчленить блок на примитивы";

        public void Execute()
        {
            if (figures == null || createdFigures == null || explodedFigure == null) return;
            figures.Remove(explodedFigure);
            figures.AddRange(createdFigures);
        }

        public void UnExecute()
        {
            if (figures == null || createdFigures == null || explodedFigure == null) return;
            createdFigures.ForEach(figure => figures.Remove(figure));
            figures.Add(explodedFigure);
        }
    }
}
