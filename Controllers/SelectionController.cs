using PetProj.Common;
using PetProj.Figures;
using PetProj.Geometries;
using PetProj.ObjectBindings;
using PetProj.Selections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PetProj.Controllers
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

        public void SelectUnselectByFrame(int width, int height, List<Figure> figures, Keys modifierKeys, bool selMode, RectangleF rectangle,
            Action<IListManage, Figure> onSelect, Action<IListManage, Figure> onUnselect)
        {
            using (var image = new Bitmap(width, height))
            using (var g = Graphics.FromImage(image))
            {
                foreach (var fig in figures)
                {
                    using (GraphicsPath path = fig.GetRendererPath())
                    {
                        if (selMode)
                        {
                            // захватываем рамкой объекты даже частично
                            if (fig.Intersects(rectangle))
                            {
                                if (modifierKeys.HasFlag(Keys.Shift))
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
                                if (modifierKeys.HasFlag(Keys.Shift))
                                    onUnselect(this.Selection, fig);
                                else
                                    onSelect(this.Selection, fig);
                                this.BuildMarkers(this.Selection);
                            }
                        }
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

        private Marker CreateMarker(Figure owner, MarkerType markerType, PointF point, int index = 0)
        {
            switch (markerType)
            {
                case MarkerType.Vertex:
                    return new VertexMarker
                    {
                        MarkerType = markerType,
                        Cursor = Cursors.Hand,
                        Position = point,
                        Index = index,
                        Owner = owner
                    };
                case MarkerType.BindingVertex:
                    return new BindingVertexMarker
                    {
                        MarkerType = markerType,
                        Cursor = Cursors.Hand,
                        Position = point,
                        Index = index,
                        Owner = owner
                    };
                case MarkerType.Middle:
                    return new MiddleMarker
                    {
                        MarkerType = markerType,
                        Cursor = Cursors.Hand,
                        Position = point,
                        Owner = owner
                    };
                case MarkerType.BindingMiddle:
                    return new BindingMiddleMarker
                    {
                        MarkerType = markerType,
                        Cursor = Cursors.Hand,
                        Position = point,
                        Owner = owner
                    };
                case MarkerType.BindingNormal:
                    return new BindingNormalMarker
                    {
                        MarkerType = markerType,
                        Cursor = Cursors.Hand,
                        Position = point,
                        Owner = owner
                    };
                default:
                    return new Marker
                    {
                        MarkerType = markerType,
                        Cursor = Cursors.Hand,
                        Position = point,
                    };
            }
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

        public void BuildMarkers(IEnumerable<Figure> selection)
        {
            // стираем предыдущие маркеры
            Markers.Clear();
            // если ничего не выбрано, выходим
            if (selection.Count() == 0) return;
            foreach (var fig in selection)
            {
                using (var path = fig.GetRendererPath())
                {
                    var points = path.PathPoints;
                    if (fig.Geometry is LineGeometry _)
                    {
                        for (var i = 0; i < points.Length; i++)
                            Markers.Add(CreateMarker(fig, MarkerType.Vertex, points[i], i));
                        var pt1 = points[0];
                        var pt2 = points[1];
                        var pt = new PointF((pt1.X + pt2.X) / 2f, (pt1.Y + pt2.Y) / 2f);
                        Markers.Add(CreateMarker(fig, MarkerType.Middle, pt));
                    }
                }
            }
        }

        public void BuildBindingMarkers(IEnumerable<Figure> selection, AllowedObjectBindings allowed, PointF basePoint)
        {
            // стираем предыдущие маркеры
            BindingMarkers.Clear();
            // если ничего не выбрано, выходим
            if (selection.Count() == 0) return;
            foreach (var fig in selection)
            {
                using (var path = fig.GetRendererPath())
                {
                    var points = path.PathPoints;
                    for (var i = 0; i < points.Length; i++)
                    {
                        // поиск конечных точек
                        if (allowed.HasFlag(AllowedObjectBindings.EndPoint))
                        {
                            var marker = CreateMarker(fig, MarkerType.BindingVertex, points[i], i);
                            BindingMarkers.Add(marker);
                        }
                    }
                    if (fig.Geometry is LineGeometry _)
                    {
                        var pt1 = points[0];
                        var pt2 = points[1];
                        // поиск средней точки на отрезке
                        if (allowed.HasFlag(AllowedObjectBindings.Middle))
                        {
                            var mid = new PointF((pt1.X + pt2.X) / 2f, (pt1.Y + pt2.Y) / 2f);
                            var marker = CreateMarker(fig, MarkerType.BindingMiddle, mid);
                            BindingMarkers.Add(marker);
                        }
                        // поиск проекции базовой точки на отрезок 
                        if (allowed.HasFlag(AllowedObjectBindings.Normal))
                        {
                            if (PointFExtension.ProjectPointOnSegment(pt1, pt2, basePoint, out PointF norm))
                            {
                                var marker = CreateMarker(fig, MarkerType.BindingNormal, norm);
                                BindingMarkers.Add(marker);
                            }
                        }
                    }
                }
            }
        }
    }
}
