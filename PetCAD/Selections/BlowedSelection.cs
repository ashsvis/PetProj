using PetCAD.Figures;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PetCAD.Selections
{
    public class BlowedSelection : IEnumerable<Figure>
    {
        // внутренний набор для хранения списка выделенных фигур
        private readonly HashSet<Figure> selected = new HashSet<Figure>();

        public IEnumerator<Figure> GetEnumerator()
        {
            return selected.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Очистка списка выделенных фигур
        /// </summary>
        public void Clear()
        {
            selected.Clear();
        }

        /// <summary>
        /// Добавление фигуры к списку выделенных фигур
        /// </summary>
        /// <param name="fig">Добавляемая фигура</param>
        public void Add(Figure fig)
        {
            selected.Add(fig);
        }

        /// <summary>
        /// Исключение фигуры из списка выделенных фигур
        /// </summary>
        /// <param name="fig">Исключаемая фигура</param>
        public void Remove(Figure fig)
        {
            selected.Remove(fig);
        }

        /// <summary>
        /// Наличие фигуры в списке выбранных
        /// </summary>
        /// <param name="figure">Проверяемая фигура</param>
        /// <returns>True - фигура в списке</returns>
        public bool Contains(Figure figure)
        {
            return selected.Contains(figure);
        }

        /// <summary>
        /// Количество фигур в списке
        /// </summary>
        public int Count { get { return selected.Count; } }

        /// <summary>
        /// Метод отрисовки фигуры на канве
        /// </summary>
        /// <param name="graphics">Канва для рисования</param>
        /// <param name="figure">Фигура со свойствами для рисования</param>
        public void Render(Graphics graphics, Color color, float zoomScale, Selection selection)
        {
            foreach (var figure in selected.Where(x => !selection.Contains(x)))
            {
                var blowedColor = color;
                // получаем путь для рисования методом фигуры
                using (var path = figure.GetRendererPath())
                {
                    using (var pen = new Pen(blowedColor, (figure.Style.BorderStyle.Width + 2f) / zoomScale))
                        graphics.DrawPath(pen, path);
                }
            }
        }

    }
}
