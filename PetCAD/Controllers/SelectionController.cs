using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;
using PetCAD.ObjectBindings;
using PetCAD.Selections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public void SelectUnselectByFrame(int width, int height, List<Figure> figures, bool shiftPressed, bool selMode, RectangleF rectangle,
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
                        Position = point,
                        Index = index,
                        Owner = owner
                    };
                case MarkerType.Middle:
                    return new MiddleMarker
                    {
                        MarkerType = markerType,
                        Position = point,
                        Owner = owner
                    };
                case MarkerType.Center:
                    return new CenterMarker
                    {
                        MarkerType = markerType,
                        Position = point,
                        Owner = owner
                    };
                case MarkerType.BindingVertex:
                    return new BindingVertexMarker
                    {
                        MarkerType = markerType,
                        Position = point,
                        Index = index,
                        Owner = owner
                    };
                case MarkerType.BindingMiddle:
                    return new BindingMiddleMarker
                    {
                        MarkerType = markerType,
                        Position = point,
                        Owner = owner
                    };
                case MarkerType.BindingCenter:
                    return new BindingCenterMarker
                    {
                        MarkerType = markerType,
                        Position = point,
                        Owner = owner
                    };
                case MarkerType.BindingQuadrant:
                    return new BindingQudrantMarker
                    {
                        MarkerType = markerType,
                        Position = point,
                        Owner = owner
                    };
                case MarkerType.BindingNormal:
                    return new BindingNormalMarker
                    {
                        MarkerType = markerType,
                        Position = point,
                        Owner = owner
                    };
                case MarkerType.BindingTangent:
                    return new BindingTangentMarker
                    {
                        MarkerType = markerType,
                        Position = point,
                        Owner = owner
                    };
                default:
                    return new Marker
                    {
                        MarkerType = markerType,
                        Cursor = Cursors.UpArrow,
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
            {
                using (var path = fig.GetRendererPath())
                {
                    if (fig.Geometry is LineGeometry segment)
                    {
                        var pt1 = segment.StartPoint;
                        var pt2 = segment.EndPoint;
                        Markers.Add(CreateMarker(fig, MarkerType.Vertex, pt1, 0));
                        Markers.Add(CreateMarker(fig, MarkerType.Vertex, pt2, 1));
                        var pt = new PointF((pt1.X + pt2.X) / 2f, (pt1.Y + pt2.Y) / 2f);
                        Markers.Add(CreateMarker(fig, MarkerType.Middle, pt));
                    }
                    else if (fig.Geometry is ArcGeometry arc)
                    {
                        Markers.Add(CreateMarker(fig, MarkerType.Center, arc.CenterPoint, 0));
                        Markers.Add(CreateMarker(fig, MarkerType.Vertex, arc.StartPoint, 1));
                        Markers.Add(CreateMarker(fig, MarkerType.Vertex, arc.MiddlePoint, 2));
                        Markers.Add(CreateMarker(fig, MarkerType.Vertex, arc.EndPoint, 3));
                    }
                    else if (fig.Geometry is BlockGeometry block)
                    {
                        Markers.Add(CreateMarker(fig, MarkerType.Vertex, block.InsertPoint, 0));
                    }
                }
            }
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
                if (fig.Geometry is LineGeometry segment)
                {
                    var pt1 = segment.StartPoint;
                    var pt2 = segment.EndPoint;
                    // поиск конечных точек
                    if (allowed.HasFlag(AllowedObjectBindings.EndPoint))
                    {
                        BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingVertex, pt1, 0));
                        BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingVertex, pt2, 1));
                    }
                    // поиск средней точки на отрезке
                    if (allowed.HasFlag(AllowedObjectBindings.Middle))
                    {
                        var mid = new PointF((pt1.X + pt2.X) / 2f, (pt1.Y + pt2.Y) / 2f);
                        BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingMiddle, mid));
                    }
                    // поиск проекции базовой точки на отрезок 
                    if (allowed.HasFlag(AllowedObjectBindings.Normal))
                    {
                        if (PointFExtension.ProjectPointOnSegment(pt1, pt2, basePoint, out PointF norm))
                            BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingNormal, norm));
                    }
                }
                else if (fig.Geometry is ArcGeometry arc)
                {
                    // поиск центра дуги
                    if (allowed.HasFlag(AllowedObjectBindings.Center))
                    {
                        var center = arc.CenterPoint;
                        BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingCenter, center));
                    }
                    var pt1 = arc.StartPoint;
                    var pt2 = arc.EndPoint;
                    // поиск конечных точек
                    if (allowed.HasFlag(AllowedObjectBindings.EndPoint))
                    {
                        BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingVertex, pt1, 0));
                        BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingVertex, pt2, 1));
                    }
                    // поиск средней точки на дуге
                    if (allowed.HasFlag(AllowedObjectBindings.Middle))
                    {
                        var mid = arc.MiddlePoint;
                        BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingMiddle, mid));
                    }
                    // поиск доступных квадрантов
                    if (allowed.HasFlag(AllowedObjectBindings.Quadrant))
                    {
                        foreach (var quadrantPoint in arc.QuadrantPoints)
                            BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingQuadrant, quadrantPoint));
                    }
                    // поиск проекций базовой точки на дугу
                    if (allowed.HasFlag(AllowedObjectBindings.Normal))
                    {
                        // проекция точки проходит также через центр дуги
                        if (PointFExtension.NormalPointOnArc(arc, basePoint, out PointF[] normals))
                        {
                            foreach (var point in normals)
                                BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingNormal, point));
                        }
                    }
                    // поиск точек касания от базовой точки к дуге 
                    if (allowed.HasFlag(AllowedObjectBindings.Tangent))
                    {
                        if (PointFExtension.TangentPointOnArc(arc, basePoint, out PointF[] tangents))
                        {
                            foreach (var point in tangents)
                                BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingTangent, point));
                        }
                    }
                }
                else if (fig.Geometry is BlockGeometry block)
                {
                    var pbase = block.InsertPoint;
                    var kf = block.ScaleFactor;
                    var angle = block.Angle;
                    foreach (var f in block.GetFigures())
                    {
                        if (f.Geometry is LineGeometry segGeometry)
                        {
                            var seg = (LineGeometry)segGeometry.DeepCopy();
                            seg.Transform(pbase, kf, angle);
                            var pt1 = seg.StartPoint;
                            var pt2 = seg.EndPoint;
                            if (allowed.HasFlag(AllowedObjectBindings.EndPoint))
                            {
                                BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingVertex, pt1, 0));
                                BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingVertex, pt2, 1));
                            }
                            if (allowed.HasFlag(AllowedObjectBindings.Middle))
                            {
                                var mid = new PointF((pt1.X + pt2.X) / 2f, (pt1.Y + pt2.Y) / 2f);
                                BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingMiddle, mid));
                            }
                        }
                        else if (f.Geometry is ArcGeometry arcGeometry)
                        {
                            var ar = (ArcGeometry)arcGeometry.DeepCopy();
                            ar.Transform(pbase, kf, angle);
                            // поиск центра дуги
                            if (allowed.HasFlag(AllowedObjectBindings.Center))
                            {
                                var center = ar.CenterPoint;
                                BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingCenter, center));
                            }
                            var pt1 = ar.StartPoint;
                            var pt2 = ar.EndPoint;
                            // поиск конечных точек
                            if (allowed.HasFlag(AllowedObjectBindings.EndPoint))
                            {
                                BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingVertex, pt1, 0));
                                BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingVertex, pt2, 1));
                            }
                            // поиск средней точки на дуге
                            if (allowed.HasFlag(AllowedObjectBindings.Middle))
                            {
                                var mid = ar.MiddlePoint;
                                BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingMiddle, mid));
                            }
                            // поиск доступных квадрантов
                            if (allowed.HasFlag(AllowedObjectBindings.Quadrant))
                            {
                                foreach (var quadrantPoint in ar.QuadrantPoints)
                                    BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingQuadrant, quadrantPoint));
                            }
                            // поиск проекций базовой точки на дугу
                            if (allowed.HasFlag(AllowedObjectBindings.Normal))
                            {
                                // проекция точки проходит также через центр дуги
                                if (PointFExtension.NormalPointOnArc(ar, basePoint, out PointF[] normals))
                                {
                                    foreach (var point in normals)
                                        BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingNormal, point));
                                }
                            }
                            // поиск точек касания от базовой точки к дуге 
                            if (allowed.HasFlag(AllowedObjectBindings.Tangent))
                            {
                                if (PointFExtension.TangentPointOnArc(ar, basePoint, out PointF[] tangents))
                                {
                                    foreach (var point in tangents)
                                        BindingMarkers.Add(CreateMarker(fig, MarkerType.BindingTangent, point));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
