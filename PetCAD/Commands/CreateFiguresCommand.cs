using PetCAD.Common;
using PetCAD.Figures;
using System.Collections.Generic;

namespace PetCAD.Commands
{
    public class CreateFiguresCommand : ICommand
    {
        private readonly List<Figure> figures;
        private readonly List<Figure> createdFigures;

        public CreateFiguresCommand(List<Figure> figures, List<Figure> createdFigures)
        {
            this.figures = figures;
            this.createdFigures = createdFigures;
        }

        public string Name => "Добавить несколько новых фигур";

        public void Execute()
        {
            if (figures == null || createdFigures == null) return;
            figures.AddRange(createdFigures);
        }

        public void UnExecute()
        {
            if (figures == null || createdFigures == null) return;
            createdFigures.ForEach(figure => figures.Remove(figure));
        }
    }
}
