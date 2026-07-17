using PetProj.Figures;
using System.Drawing;

namespace PetProj.Selections
{
    public interface IListManage
    {
        void Clear();
        void Add(Figure fig);
        void Remove(Figure fig);
        bool Contains(Figure figure);
        int Count { get; }
        void Render(Graphics graphics, Color color);
    }
}
