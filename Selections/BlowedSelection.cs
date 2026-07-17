using PetProj.Figures;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PetProj.Selections
{
    public class BlowedSelection : IEnumerable<Figure>, IListManage
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
        public void Render(Graphics graphics)
        {
            foreach (var figure in selected)
            {
                var blowedColor = Color.Gray;
                // получаем путь для рисования методом фигуры
                using (var path = figure.Geometry.Path)
                {
                    using (var pen = new Pen(figure.Style.BorderStyle.Color, figure.Style.BorderStyle.Width))
                    {
                        graphics.DrawPath(pen, path);
                        using (var blowedPen = new Pen(blowedColor, pen.Width))
                        {
                            blowedPen.StartCap = LineCap.Round;
                            blowedPen.EndCap = LineCap.Round;
                            var width = 10;

                            for (var i = 0; i < width; i++)
                            {
                                blowedPen.Color = blowedColor;
                                blowedPen.Width += 0.5f; //4;
                                graphics.DrawPath(blowedPen, path);
                                blowedColor = Color.FromArgb(blowedColor.A / 2, blowedColor);
                            }
                        }
                    }
                }
            }
        }

    }
}
