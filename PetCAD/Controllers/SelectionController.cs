using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Makers;
using PetCAD.Selections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PetCAD.Controllers
{
    /// <summary>
    /// Обрабатывает движения мышки, строит маркеры, управляет выделением,
    /// выполняет преобразования над фигурами
    /// </summary>
    public class SelectionController
    {
        private readonly Selection selection;
        private readonly List<Marker> markers;
        private readonly List<Marker> bindingMarkers;

        public SelectionController()
        {
            selection = new Selection();
            markers = new List<Marker>();
            bindingMarkers = new List<Marker>();
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
        /// Маркеры привязок
        /// </summary>
        public List<Marker> BindingMarkers { get { return bindingMarkers; } }

        /// <summary>
        /// Изменилась выделенная фигура/фигуры
        /// </summary>
        public event Action SelectedFigureChanged = delegate { };

        /// <summary>
        /// Очистить список выбранных фигур
        /// </summary>
        public void Clear()
        {
            markers.Clear();
            bindingMarkers.Clear();
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
        public void OnMouseDown(List<Figure> figures, PointF location, Keys modifierKeys, float kf = 1f)
        {
            // попробуем найти фигуру...
            if (selection.FindFigureAt(figures, location, kf, out Figure fig))
            {
                // фигура найдена.
                // если этой фигуры не было в списке
                if (!selection.Contains(fig))
                {
                    fig.DrawGlowed(false);
                    selection.Add(fig);
                    OnSelectedFigureChanged();
                }
                else
                {
                    // при нажатой клавише Shift удаляем эту фигуру из списка выбора
                    if (selection.Count > 0 && modifierKeys.HasFlag(Keys.Shift))
                    {
                        selection.Remove(fig);
                        OnSelectedFigureChanged();
                    }
                }
                BuildMarkers(Selection);
            }
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

        public void SelectUnselectByFrame(List<Figure> figures, bool shiftPressed, bool selMode, RectangleF rectangle,
            Action<IListManage, Figure> onSelect, Action<IListManage, Figure> onUnselect)
        {
            foreach (var fig in figures)
            {
                if (selMode)
                {
                    // захватываем рамкой объекты даже частично
                    if (fig.Intersects(rectangle))
                    {
                        if (shiftPressed)
                            onUnselect(this.Selection, fig);
                        else
                            onSelect(this.Selection, fig);
                        this.BuildMarkers(this.Selection);
                    }
                }
                else
                {
                    // захватываем рамкой объекты целиком
                    if (fig.Contains(rectangle))
                    {
                        if (shiftPressed)
                            onUnselect(this.Selection, fig);
                        else
                            onSelect(this.Selection, fig);
                        this.BuildMarkers(this.Selection);
                    }
                }
            }
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

        public void ClearMarkers()
        {
            // стираем предыдущие маркеры
            Markers.Clear();
        }

        public void ClearBindingMarkers()
        {
            // стираем предыдущие маркеры
            BindingMarkers.Clear();
        }

        /// <summary>
        /// Строим маркеры перемещения
        /// </summary>
        /// <param name="selection"></param>
        public void BuildMarkers(IEnumerable<Figure> selection)
        {
            // стираем предыдущие маркеры
            Markers.Clear();
            // если ничего не выбрано, выходим
            if (selection.Count() == 0) return;
            foreach (var fig in selection)
                Markers.AddRange(fig.Geometry.GetGeometryMarkers());
        }

        /// <summary>
        /// Строим маркеры объектной привязки
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="allowed"></param>
        /// <param name="basePoint"></param>
        public void BuildBindingMarkers(IEnumerable<Figure> selection, AllowedObjectBindings allowed, PointF basePoint)
        {
            // стираем предыдущие маркеры
            BindingMarkers.Clear();
            // если ничего не выбрано, выходим
            if (selection.Count() == 0) return;
            foreach (var fig in selection)
            {
                BindingMarkers.AddRange(fig.Geometry.GetBindingMarkers(allowed, basePoint));
            }
        }
    }
}
