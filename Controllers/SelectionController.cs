using PetProj.Figures;
using PetProj.Selections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PetProj.Controllers
{
    public enum EditorMode
    {
        Select,
        FrameSelect,
        AddLine,
        ChangeGeometry,
        Drag,
        CreateFigure,
        Skew,
        Verticies,
        Warp
    }

    /// <summary>
    /// Обрабатывает движения мышки, строит маркеры, управляет выделением,
    /// выполняет преобразования над фигурами
    /// </summary>
    public class SelectionController
    {
        private readonly Selection selection;
        private readonly List<Marker> markers;

        public SelectionController()
        {
            selection = new Selection();
            markers = new List<Marker>();
        }

        /// <summary>
        /// Выделенные фигуры
        /// </summary>
        public Selection Selection { get { return selection; } }

        /// <summary>
        /// Маркеры
        /// </summary>
        public List<Marker> Markers { get { return markers; } }

        /// <summary>
        /// Изменилась выделенная фигура/фигуры
        /// </summary>
        public event Action SelectedFigureChanged = delegate { };

        /// <summary>
        /// Изменился режим работы редактора
        /// </summary>
        public event Action<EditorMode> EditorModeChanged = delegate { };

        /// <summary>
        /// Очистить список выбранных фигур
        /// </summary>
        public void Clear()
        {
            markers.Clear();
            selection.Clear();
            OnSelectedFigureChanged();
        }

        /// <summary>
        /// Вызываем привязанный к событию метод при выборе фигур
        /// </summary>
        private void OnSelectedFigureChanged()
        {
            SelectedFigureChanged();
        }

        /// <summary>
        /// Действия по двойному клику на фигурах
        /// Обычно, это действие по умолчанию
        /// </summary>
        /// <param name="location"></param>
        /// <param name="modifierKeys"></param>
        public void OnDblClick(Point location, Keys modifierKeys)
        {

        }

        /// <summary>
        /// Обработчик нажатия левой кнопки мышки
        /// </summary>
        /// <param name="location">Координаты курсора</param>
        /// <param name="modifierKeys">Какие клавиши были ещё нажаты в этот момент</param>
        public void OnMouseDown(List<Figure> figures, PointF location, Keys modifierKeys)
        {
            // попробуем найти фигуру...
            if (selection.FindFigureAt(figures, location, out Figure fig))
            {
                // фигура найдена.
                // если этой фигуры не было в списке
                if (!selection.Contains(fig))
                {
                    /*
                    // если не нажата управляющая клавиша Shift
                    // в режиме изменения вершим может быть выбрана только одна фигура
                    if (!modifierKeys.HasFlag(Keys.Shift))
                        selection.Clear(); // очистим список выбранных
                    // то добавим её в список
                    */
                    fig.DrawGlowed(false);
                    selection.Add(fig);
                    OnSelectedFigureChanged();
                }
                else
                {
                    // при нажатой клавише Shift удаляем эту фигуру из списка выбора
                    // если она не последняя
                    if (selection.Count > 1 && modifierKeys.HasFlag(Keys.Shift))
                    {
                        selection.Remove(fig);
                        OnSelectedFigureChanged();
                    }
                }
                //строим маркеры
                //UpdateMarkers();
            }
        }

        /// <summary>
        /// Метод строит маркеры у объекта Selection
        /// </summary>
        public void UpdateMarkers()
        {
            var list = new List<Figure>(selection);
            selection.Clear();
            foreach (var figure in list) selection.Add(figure);
            //строим маркеры
            BuildMarkers();
        }

        private void BuildMarkers()
        {
            // стираем предыдущие маркеры
            Markers.Clear();
            // если ничего не выбрано, выходим
            if (Selection.Count == 0) return;

            // задаём геометрию маркеров по умолчанию 
            foreach (var marker in Markers)
                FigureBuilder.BuildMarkerGeometry(marker);
        }

        /// <summary>
        /// Обработчик перемещения мышки при нажатой левой кнопке мышки 
        /// </summary>
        /// <param name="location">Координаты курсора</param>
        /// <param name="modifierKeys">Какие клавиши были ещё нажаты в этот момент</param>
        public void OnMouseMove(PointF location, Keys modifierKeys)
        {

        }

        /// <summary>
        /// Обработчик отпускания левой кнопки мышки
        /// </summary>
        /// <param name="location">Координаты курсора</param>
        /// <param name="modifierKeys">Какие клавиши были ещё нажаты в этот момент</param>
        public void OnMouseUp(PointF location, Keys modifierKeys)
        {

        }

        /// <summary>
        /// Форма курсора в зависимости от контекста
        /// </summary>
        /// <param name="location">Позиция курсора</param>
        /// <param name="modifierKeys">Какие клавиши были ещё нажаты в этот момент</param>
        /// <param name="button">Нажатая кнопка мышки</param>
        /// <returns>Настроенный курсор</returns>
        public Cursor GetCursor(Point location, Keys modifierKeys, MouseButtons button)
        {
            return Cursors.Default;
        }
    }
}
