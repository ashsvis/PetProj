using PetProj.Figures;
using PetProj.Geometries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PetProj.Selections
{
    /// <summary>
    /// Набор выделенных фигур и операции над ними
    /// </summary>
    public class Selection : IEnumerable<Figure>,  IListManage
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
        /// Конструктор с инициализацией по умолчанию
        /// </summary>
        public Selection()
        {

        }

        //public Color Color { get; set; } = Color.Magenta;

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
        /// Ищем попадание в контур фигуры
        /// </summary>
        /// <param name="point">Положение курсора</param>
        /// <returns>Контур фигуры найден</returns>
        public bool IsHit(List<Figure> figures, Point point)
        {
            var found = false;
            using (var pen = new Pen(Color.Black, 5))
            {
                // просмотр начинаем с конца списка - там самые "верхние" фигуры
                for (var i = figures.Count - 1; i >= 0; i--)
                {
                    var fig = figures[i];
                    var path = fig.GetRendererPath();
                    // проверяем также попадание на контур фигуры
                    if (!path.IsOutlineVisible(point, pen)) continue;
                    found = true;
                    break;
                }
            }
            return found;
        }

        /// <summary>
        /// Ищем фигуру в данной точке
        /// </summary>
        /// <param name="layer">Ссылка на слой</param>
        /// <param name="point">Положение курсора</param>
        /// <param name="figure">Найденная фигура или null</param>
        /// <returns>True - фигура найдена</returns>
        public bool FindFigureAt(List<Figure> figures, PointF point, out Figure figure)
        {
            figure = null;
            var found = false;
            using (var pen = new Pen(Color.Black, 5))
            {
                // просмотр начинаем с конца списка - там самые "верхние" фигуры
                for (var i = figures.Count - 1; i >= 0; i--)
                {
                    var fig = figures[i];
                    var isClosed = !(fig.Geometry is IGeometry closedGeometry) ||
                        closedGeometry != null && closedGeometry.IsClosed;
                    var path = fig.GetRendererPath();
                    if (isClosed && path.IsVisible(point))
                    {
                        figure = fig;
                        found = true;
                        break;
                    }
                    // проверяем также попадание на контур фигуры
                    if (!path.IsOutlineVisible(point, pen)) continue;
                    figure = fig;
                    found = true;
                    break;
                }
            }
            return found;
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
        public void Render(Graphics graphics, Color color)
        {
            foreach (var figure in selected)
            {
                // получаем путь для рисования методом фигуры
                using (var path = figure.Geometry.Path)
                {
                    // то получаем карандаш из стиля рисования фигуры
                    using (var pen = new Pen(color /*Color*/))
                    { 
                        graphics.DrawPath(pen, path); 
                    }
                }
            }
        }

        /// <summary>
        /// Перемещение выделенных(ой) фигур(ы)
        /// </summary>
        /// <param name="offsetX">Смещение по горизонтали</param>
        /// <param name="offsetY">Смещение по вертикали</param>
        /// <param name="moveAction">Ссылка на метод фактического перемещения</param>
        public void Translate(float offsetX, float offsetY, Action<List<(Figure, PointF)>> moveAction)
        {
            // список фигур и смещений
            List<(Figure, PointF)> offsets = new List<(Figure, PointF)>();
            // для всех выделенных фигур
            foreach(var figure in selected)
            {
                // если перемещение поддерживается
                if (figure.Geometry is IMoveGeometry _)
                {
                    // добавляем в список
                    offsets.Add((figure, new PointF(offsetX, offsetY)));
                }
            }
            // если список не пуст, выполняем метод перемещения
            if (offsets.Count > 0)
                moveAction(offsets);
        }

        /// <summary>
        /// Копирование с перещением
        /// </summary>
        /// <param name="offsetX">Смещение по горизонтали</param>
        /// <param name="offsetY">Смещение по вертикали</param>
        /// <param name="addCopyAction">Ссылка на метод фактического копирования и перемещения</param>
        public void TranslateCopy(float offsetX, float offsetY, Action<List<Figure>> addCopyAction)
        {
            // список фигур
            List<Figure> added = new List<Figure>();
            // для всех выделенных фигур
            foreach (var figure in selected)
            {
                // получаем полную копию фигуры
                var fig = figure.DeepCopy();
                // если перемещение поддерживается
                if (fig.Geometry is IMoveGeometry geometry)
                {
                    // перемещаем созданную копию
                    geometry.Move(offsetX, offsetY);
                    // добавляем копию в список
                    added.Add(fig);
                }
            }
            // если список не пуст, выполняем метод копирования и перемещения
            if (added.Count > 0)
                addCopyAction(added);
        }
    }
}
