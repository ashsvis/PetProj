using PetProj.Commands;
using PetProj.Common;
using PetProj.Controllers;
using PetProj.Figures;
using PetProj.Geometries;
using PetProj.ObjectBindings;
using PetProj.Renderers;
using PetProj.Selections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PetProj
{
    public partial class DrawControl : UserControl, IUndoRedoSupport
    {
        private int mouseClickCount;
        private PointF firstMouseDown;
        private PointF secondMouseDown;
        private PointF mousePosition;
        private EditorMode editorMode;

        public bool IsDynamicalEnter { get; set; } = true;
        public bool IsDrawOrthoMode { get; set; } = false;
        public bool IsObjectBinding { get; set; } = false;

        public EditorMode EditorMode => editorMode;
        public int MouseClickCount { get => mouseClickCount; set => mouseClickCount = value; }
        public PointF FirstMouseDown { get => firstMouseDown; set => firstMouseDown = value; }
        public PointF SecondMouseDown { get => secondMouseDown; set => secondMouseDown = value; }
        public PointF CurrentMousePosition => mousePosition;

        private readonly BlowedSelection underCursor = new BlowedSelection();

        private readonly List<Figure> figures = new List<Figure>();
        private readonly SelectionController selectionController;
        private readonly List<Marker> markers = new List<Marker>();

        private readonly UndoRedoManager undoRedoManager;

        public bool Changed { get; set; }
        public SelectionController SelectionController => selectionController;

        public event EventHandler OnSelectionMode;
        public event EventHandler<string> OnToolTipChanged;
        public event EventHandler<EditorMode> OnChangeMode;
        public event EventHandler<object[]> OnChangeParams;
        public event EventHandler<(int, PointF, Point)> OnCursorMoved;
        public event EventHandler<Selection> OnSelected;

        public Figure Layer { get; set; }

        /// <summary>
        /// Текущие режимы объектных привязок
        /// </summary>
        public  AllowedObjectBindings AllowedObjectBindings { get; set; }

        private readonly BuildLineController buildLineController;
        private readonly BuildRectangleController buildRectangleController;
        private readonly BuildArcByThreePointsController buildArcByThreePointsController;

        public DrawControl()
        {
            InitializeComponent();
            Layer = new Figure();
            FigureBuilder.BuildLayerGeomentry(Layer);
            undoRedoManager = new UndoRedoManager();
            undoRedoManager.OnStateChaned += (o, e) => zoomPad.Invalidate();
            selectionController = new SelectionController();
            // подключение обработчиков событий для контроллера выбора
            selectionController.SelectedFigureChanged += BuildInterface;
            buildLineController = new BuildLineController(this, zoomPad);
            buildRectangleController = new BuildRectangleController(this, zoomPad);
            buildArcByThreePointsController = new BuildArcByThreePointsController(this, zoomPad);
        }

        private void BuildInterface()
        {
            mouseClickCount = 0;
            UpdateInterface();
        }

        public void UpdateInterface()
        {
            Invalidate();
            zoomPad.Invalidate();
        }

        private void zoomPad_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            if (graphics == null) return;
            // рисуем начало координат и направление осей
            this.DrawZeroOrigin(graphics, Color.LightGray);
            var zoom = (float)zoomPad.ZoomScale;
            // отрисовка созданных фигур
            foreach (var fig in figures)
            {
                var bounds = fig.Geometry?.Bounds ?? RectangleF.Empty;
                // если фигура вырождена в точку, то охватываюший прямоугольник пуст
                if (bounds.Width == 0 && bounds.Height == 0)
                {
                    bounds.Width += 1.6f / zoom;
                    bounds.Height += 1.6f / zoom;
                    // рисуем точку в позиции этой фигуры
                    using (var brush = new SolidBrush(fig.Style.BorderStyle.Color))
                        graphics.FillRectangle(brush, bounds);
                }
                else
                    // рисуем стандартно
                    fig.Renderer.Render(graphics, fig);
            }
            // отрисовка выделения
            selectionController.Selection.Render(graphics,
                    editorMode == EditorMode.MoveSelected && mouseClickCount == 1 ? Color.Silver : Color.Pink, zoom);
            // отрисовка маркеров на выбранных фигурах
            foreach (var marker in selectionController.Markers)
                marker.Render(graphics, markers.Contains(marker) ? Color.Red : Color.Blue, zoom);
            // отрисовка маркеров привязки на фигурах под курсором, при построении линий
            if (IsObjectBinding && (editorMode != EditorMode.Selection))
            {
                var location = PrepareMousePosition(PointToClient(MousePosition));
                var query = selectionController.BindingMarkers.Select(marker => (marker,
                             $"{Math.Abs(marker.Position.X - location.X):00000}{Math.Abs(marker.Position.Y - location.Y):00000}")).OrderBy(x => x.Item2);
                // рисуем ближайший маркер привязки к текущему курсору
                using (var pen = new Pen(Color.Black, 1f / zoom))
                    foreach (var item in query.Take(1))
                    {
                        // рисование перекрестья в центре дуги
                        if (item.marker.Owner is Figure fig && fig.Geometry is ArcGeometry arc)
                        {
                            graphics.DrawLine(pen,
                                new PointF(arc.CenterPoint.X - 4f / zoom, arc.CenterPoint.Y),
                                new PointF(arc.CenterPoint.X + 4f / zoom, arc.CenterPoint.Y));
                            graphics.DrawLine(pen,
                                new PointF(arc.CenterPoint.X, arc.CenterPoint.Y - 4f / zoom),
                                new PointF(arc.CenterPoint.X, arc.CenterPoint.Y + 4f / zoom));
                        }
                        item.marker.Render(graphics, Color.White, zoom);
                    }
            }
            else
            { 
                // отрисовка временно подсвеченных под курсором или рамкой выделения
                underCursor.Render(graphics, Color.White, zoom, selectionController.Selection);
            }
            this.DrawDefaultCursor(graphics, mousePosition);
            float kf = (float)(1f / zoom);
            PointF pt;
            string text;
            switch (editorMode)
            {
                case EditorMode.Selection:
                    if (mouseClickCount == 1)
                        this.DrawRibbonSelectionRect(graphics, firstMouseDown, mousePosition);
                    break;
                case EditorMode.MoveSelected:
                case EditorMode.MoveCopySelected:
                    if (IsDynamicalEnter)
                    {
                        pt = PrepareMousePosition(mousePosition);
                        text = (mouseClickCount == 0 ? $"Базовая точка " : $"Вторая точка ") + $" X:{pt.X} Y:{pt.Y}";
                        using (var pen = new Pen(Color.Black, kf))
                        using (var font = new Font("Arial", (float)(10f * kf)))
                            graphics.DrawString(text, font, Brushes.Black, PrepareMousePosition(PointF.Add(mousePosition, new SizeF(1f, 1f))));
                    }
                    if (mouseClickCount == 1)
                        this.DrawRibbonMoved(graphics, firstMouseDown, mousePosition);
                    break;
                case EditorMode.MoveMarkers:
                    if (mouseClickCount == 1)
                    {
                        this.DrawRibbonMovedFigures(graphics, markers
                            .Where(m => m.AllowedOperations.HasFlag(AllowedMarkerOperations.MoveOwner))
                            .Select(m => m.Owner).ToList(), firstMouseDown, mousePosition);
                        this.DrawRibbonMovedMarkers(graphics,
                            markers.Where(m => m.AllowedOperations.HasFlag(AllowedMarkerOperations.MoveVertex)).ToList(),
                            mousePosition);
                    }
                    break;
            }
        }

        public PointF Origin => zoomPad.Origin;
        public float Zoom => (float)zoomPad.ZoomScale;
        public Graphics ZoomPadGraphics => zoomPad.CreateGraphics();

        /// <summary>
        /// Перерасчёт позиции мыши при масштабировании и панарамировании
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PointF PrepareMousePosition(PointF p)
        {
            PointF[] arr = new PointF[] { p };
            Matrix matrix = new Matrix();

            var zoom = (float)zoomPad.ZoomScale;
            var origin = zoomPad.Origin;

            matrix.Translate(origin.X, origin.Y);
            matrix.Scale(1 / zoom, 1 / zoom);
            matrix.TransformPoints(arr);
            matrix.Dispose();
            return new PointF(arr[0].X, arr[0].Y);
        }

        /// <summary>
        /// Нажатие кнопки указателя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomPad_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PressLeftMouseButton(e.Location);
            else if (e.Button == MouseButtons.Right)
                PressRightMouseButton(e.Location);
        }

        private void PressLeftMouseButton(PointF point, bool calledByCode = false)
        {
            underCursor.Clear();
            mousePosition = point;
            if (mouseClickCount == 0)
            {
                // при первом нажатии запоминаем точку нажатия
                firstMouseDown = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
                if (!calledByCode)
                {
                    if (editorMode == EditorMode.Selection && selectionController.Markers.Count > 0)
                    {
                        // определение нажатия на маркере
                        var items = selectionController.Markers.Where(m => m.Target(Zoom).Contains(firstMouseDown));
                        if (items.Count() > 0)
                        {
                            markers.Clear();
                            foreach (var marker in items)
                            {
                                // отсечение маркеров, принадлежащих одной фигуре и имеющих одинаковую позицию
                                if (markers.Any(m => m.Owner == marker.Owner && m.Position == marker.Position)) continue;
                                markers.Add(marker);
                            }
                            SetMode(EditorMode.MoveMarkers);
                            // прибавление чиста нажатий указателя через событие таймера
                            timerAddMouseCount.Enabled = true;
                            return;
                        }
                    }
                    //поиск ближайшей точки привязки, если включен режим объектной привязки
                    if (editorMode != EditorMode.Selection)
                        firstMouseDown = this.FindBindingPoint(firstMouseDown);
                }
                if (editorMode == EditorMode.Selection)
                {
                    var count = selectionController.Selection.Count;
                    selectionController.OnMouseDown(figures, firstMouseDown, ModifierKeys);
                    OnSelected?.Invoke(this, selectionController.Selection);
                    if (count != selectionController.Selection.Count) 
                        return;
                }
                // прибавление числа нажатий указателя через событие таймера
                timerAddMouseCount.Enabled = true;
            }
            else if (mouseClickCount == 1) // это второе нажатие
            {
                secondMouseDown = PrepareMousePosition(mousePosition);
                PointF pt1, pt2;
                switch (editorMode)
                {
                    case EditorMode.Selection:
                        pt1 = firstMouseDown;
                        pt2 = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
                        var selMode = pt1.X > pt2.X;
                        var rectangle = new RectangleF(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y),
                            Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));
                        selectionController.SelectUnselectByFrame(Width, Height, figures, ModifierKeys, selMode, rectangle,
                                (manager, fig) =>
                                {
                                    if (!selectionController.Selection.Contains(fig))
                                    {
                                        fig.DrawGlowed(false);
                                        selectionController.Selection.Add(fig);
                                        OnSelected?.Invoke(this, selectionController.Selection);
                                    }
                                },
                                (manager, fig) =>
                                {
                                    if (selectionController.Selection.Contains(fig))
                                    {
                                        selectionController.Selection.Remove(fig);
                                        OnSelected?.Invoke(this, selectionController.Selection);
                                    }
                                }
                            );
                        // при отсутствии других режимов - режим выбора, и второе нажатие
                        // сбрасывает количество нажатий
                        timerClearMouseCount.Enabled = true;
                        break;
                    case EditorMode.MoveSelected:
                        pt1 = firstMouseDown;
                        pt2 = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
                        //поиск ортогональной точки, если включен режим ортогонального построения
                        pt2 = this.FindOrthoPoint(pt2);
                        //поиск ближайшей точки привязки, если включен режим объектной привязки
                        pt2 = this.FindBindingPoint(pt2);
                        selectionController.Selection.Translate(pt2.X - pt1.X, pt2.Y - pt1.Y,
                            (movedoffsets) =>
                            {
                                undoRedoManager.Execute(new MoveFiguresCommand(movedoffsets));
                            });
                        // предыдущий выбор стирается, т.к. перемещение - однократная операция
                        selectionController.Selection.Clear();
                        timerClearMouseCount.Enabled = true;
                        SetMode(EditorMode.Selection);
                        Changed = true;
                        break;
                    case EditorMode.MoveCopySelected:
                        pt1 = firstMouseDown;
                        pt2 = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
                        //поиск ортогональной точки, если включен режим ортогонального построения
                        pt2 = this.FindOrthoPoint(pt2);
                        //поиск ближайшей точки привязки, если включен режим объектной привязки
                        pt2 = this.FindBindingPoint(pt2);
                        selectionController.Selection.TranslateCopy(pt2.X - pt1.X, pt2.Y - pt1.Y,
                            (addedfigs) =>
                            {
                                undoRedoManager.Execute(new CreateFiguresCommand(figures, addedfigs));
                            });
                        Changed = true;
                        break;
                    case EditorMode.MoveMarkers:
                        if (markers.Count > 0)
                        {
                            var lastPosition = markers.First().Position;
                            var currentPosition = calledByCode ? mousePosition : PrepareMousePosition(mousePosition);
                            //поиск ортогональной точки, если включен режим ортогонального построения
                            currentPosition = this.FindOrthoPoint(currentPosition);
                            //поиск ближайшей точки привязки, если включен режим объектной привязки
                            currentPosition = this.FindBindingPoint(currentPosition);
                            // перемещение отрезков за середину
                            MoveFigureByMarker(lastPosition, currentPosition);
                            MoveVertexByMarker(lastPosition, currentPosition);
                            firstMouseDown = currentPosition;
                            Changed = true;
                        }
                        timerClearMouseCount.Enabled = true;
                        SetMode(EditorMode.Selection);
                        break;
                }
            }
            zoomPad.Invalidate();
        }

        /// <summary>
        /// Перемещение отрезка за середину
        /// Перемещение дуги, окружности за центральную точку
        /// Перемещение блока за базовую точку (Origin)
        /// </summary>
        /// <param name="last"></param>
        /// <param name="current"></param>
        private void MoveFigureByMarker(PointF last, PointF current)
        {
            var offsets = new List<(Figure, PointF)>();
            foreach (var figure in markers.Where(m => m.AllowedOperations.HasFlag(AllowedMarkerOperations.MoveOwner)).Select(m => m.Owner))
            {
                // добавляем в список
                offsets.Add((figure, new PointF(current.X - last.X, current.Y - last.Y)));
            }
            // если список не пуст, выполняем метод перемещения
            if (offsets.Count > 0)
            {
                undoRedoManager.Execute(new MoveFiguresCommand(offsets));
                selectionController.BuildMarkers(selectionController.Selection);
            }
        }

        /// <summary>
        /// Изменение размера отрезка при перемещении маркера конца
        /// </summary>
        /// <param name="last"></param>
        /// <param name="current"></param>
        private void MoveVertexByMarker(PointF last, PointF current)
        {
            var offsets = new List<(Figure, PointF, int)>();
            foreach (var marker in markers.Where(m => m.AllowedOperations.HasFlag(AllowedMarkerOperations.MoveVertex)))
            {
                if (marker is VertexMarker vertex)
                {
                    //if (marker.Owner.Geometry is LineGeometry segment)
                        // добавляем в список
                        offsets.Add((marker.Owner, new PointF(current.X - last.X, current.Y - last.Y), vertex.Index));
                }
            }
            // если список не пуст, выполняем метод перемещения
            if (offsets.Count > 0)
            {
                undoRedoManager.Execute(new MoveMarkersCommand(offsets));
                selectionController.BuildMarkers(selectionController.Selection);
            }
        }

        private void PressRightMouseButton(Point screenMouseLocation, bool calledByCode = false)
        {
            OnToolTipChanged?.Invoke(this, string.Empty);
            if (editorMode == EditorMode.MoveCopySelected)
            {
                timerClearMouseCount.Enabled = true;
                selectionController.Clear();
                OnSelected?.Invoke(this, selectionController.Selection);
                SetMode(EditorMode.Selection);
            }
            else if (editorMode == EditorMode.Selection)
            {
                timerClearMouseCount.Enabled = true;
                selectionController.Clear();
                OnSelected?.Invoke(this, selectionController.Selection);
            }
            else if (editorMode != EditorMode.Selection)
                SetMode(EditorMode.Selection);
        }

        public void SendParamsOnChange(params object[] args)
        {
            OnChangeParams?.Invoke(this, args);
        }

        private void zoomPad_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            var pt = PrepareMousePosition(mousePosition);
            OnCursorMoved?.Invoke(this, (mouseClickCount, firstMouseDown, Point.Ceiling(pt)));

            if (e.Button == MouseButtons.None)
            {
                switch (editorMode)
                {
                    case EditorMode.Selection:
                        if (mouseClickCount == 0 && selectionController.Markers.Count > 0)
                        {
                            var marker = selectionController.Markers.FirstOrDefault(m => m.Target(Zoom).Contains(pt));
                            if (marker is Marker _)
                                Cursor = marker.Cursor;
                        }
                        break;
                    case EditorMode.MoveSelected:
                    case EditorMode.MoveCopySelected:
                        if (mouseClickCount == 0)
                            OnChangeParams?.Invoke(this, new object[] { pt });
                        else if (mouseClickCount == 1)
                        {
                            selectionController.ClearMarkers();
                            var pt1 = firstMouseDown;
                            var pt2 = PrepareMousePosition(mousePosition);
                            var vector = pt2.Vector(pt1);
                            OnChangeParams?.Invoke(this, new object[] { vector });
                        }
                        break;
                }
            }

            selectionController.OnMouseMove(pt, ModifierKeys);

            if (mouseClickCount == 1 && editorMode == EditorMode.Selection)
            {
                // определение фигуры под рамкой выбора
                var pt1 = firstMouseDown;
                var pt2 = PrepareMousePosition(mousePosition);
                var selMode = pt1.X > pt2.X;
                var rectangle = new RectangleF(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y),
                    Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));
                underCursor.Clear();
                selectionController.SelectUnselectByFrame(Width, Height, figures, ModifierKeys, selMode, rectangle,
                        (manager, fig) =>
                        {
                            if (!underCursor.Contains(fig))
                                underCursor.Add(fig);
                        },
                        (manager, fig) =>
                        {
                            if (underCursor.Contains(fig))
                                underCursor.Remove(fig);
                        }
                    );
                OnSelected?.Invoke(this, selectionController.Selection);
            }
            if (mouseClickCount == 0 || (mouseClickCount == 1 || mouseClickCount == 2) && editorMode != EditorMode.Selection)
            {
                // определение фигуры непосредственно под курсором
                underCursor.Clear();
                var fig = figures.LastOrDefault(x => x.Contains(pt, (float)(8f / zoomPad.ZoomScale)));
                if (fig != null)
                {
                    underCursor.Add(fig);
                    var allowed = AllowedObjectBindings;
                    if (editorMode == EditorMode.MoveMarkers)
                    {
                        if (allowed.HasFlag(AllowedObjectBindings.Tangent))
                        {
                            if (fig.Geometry is ArcGeometry arc)
                            {
                                foreach (var marker in markers)
                                {
                                    if (marker.Owner.Geometry is LineGeometry segment)
                                    {
                                        selectionController.BuildBindingMarkers(underCursor, allowed, 
                                            marker.Position == segment.EndPoint ? segment.StartPoint : segment.EndPoint);
                                    }
                                }
                                zoomPad.Invalidate();
                                return;
                            }
                        }
                    }
                    selectionController.BuildBindingMarkers(underCursor, allowed, firstMouseDown);
                }
                else
                    selectionController.ClearBindingMarkers();
            }
            zoomPad.Invalidate();
        }

        private void zoomPad_MouseUp(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            var pt = PrepareMousePosition(mousePosition);
            if (e.Button == MouseButtons.Left)
            {
                selectionController.OnMouseUp(pt, ModifierKeys);
                OnSelected?.Invoke(this, selectionController.Selection);
            }
            zoomPad.Invalidate();
        }

        /// <summary>
        /// Создать отрезок на двух точках
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        private Figure CreateLine(PointF pt1, PointF pt2)
        {
            Figure line = new Figure();
            line.Style.BorderStyle = Layer.Style.BorderStyle.DeepCopy();
            FigureBuilder.BuildLineGeometry(line, pt1);
            ((LineGeometry)line.Geometry).AddPoint(pt2);
            line.Style.FillStyle.IsVisible = false;
            return line;
        }

        /// <summary>
        /// Добавление отрезка линии по двум точкам
        /// </summary>
        /// <param name="pt1">Первая точка</param>
        /// <param name="pt2">Вторая точка</param>
        /// <param name="loading">Признак загрузки из внешнего источника (для исключения поддержки undo)</param>
        public void AddLine(PointF pt1, PointF pt2, bool loading = false)
        {
            Figure line = CreateLine(pt1, pt2);
            if (loading)
                figures.Add(line);
            else
                undoRedoManager.Execute(new CreateFigureCommand(figures, line));
        }

        /// <summary>
        /// Создать дугу с центром, радиусом, начальным углом и углов створа
        /// </summary>
        /// <returns></returns>
        private Figure CreateArc(PointF center, float radius, float startAngle, float sweepAngle)
        {
            Figure arc = new Figure();
            arc.Style.BorderStyle = Layer.Style.BorderStyle.DeepCopy();
            FigureBuilder.BuildArcGeometry(arc, center, radius, startAngle, sweepAngle);
            arc.Style.FillStyle.IsVisible = false;
            return arc;
        }

        public void AddArc(PointF center, float radius, float startAngle, float sweepAngle, bool loading = false)
        {
            Figure arc = CreateArc(center, radius, startAngle, sweepAngle);
            if (loading)
                figures.Add(arc);
            else
                undoRedoManager.Execute(new CreateFigureCommand(figures, arc));
        }

        public void AddArc(PointF pt1, PointF pt2, PointF pt3)
        {
            float mx1 = (pt1.X + pt2.X) / 2f;
            float my1 = (pt1.Y + pt2.Y) / 2f;
            PointF mid1 = new PointF(mx1, my1);
            float dx1 = pt2.X - pt1.X;
            float dy1 = pt2.Y - pt1.Y;
            float px1 = dy1;
            float py1 = -dx1;
            float length1 = (float)Math.Sqrt(px1 * px1 + py1 * py1);
            if (length1 == 0) return; // отрезок вырожден в точку
            px1 /= length1;
            py1 /= length1;

            float mx2 = (pt3.X + pt2.X) / 2f;
            float my2 = (pt3.Y + pt2.Y) / 2f;
            PointF mid2 = new PointF(mx2, my2);
            float dx2 = pt3.X - pt2.X;
            float dy2 = pt3.Y - pt2.Y;
            float px2 = dy2;
            float py2 = -dx2;
            float length2 = (float)Math.Sqrt(px2 * px2 + py2 * py2);
            if (length2 == 0) return; // отрезок вырожден в точку
            px2 /= length2;
            py2 /= length2;

            // перпендикуляр в середине 1 отрезка
            float halfLength = Math.Max(length1, length2); //50f / zoom;
            PointF df1 = new PointF(mid1.X + px1 * halfLength, mid1.Y + py1 * halfLength);
            PointF ef1 = new PointF(mid1.X - px1 * halfLength, mid1.Y - py1 * halfLength);
            // перпендикуляр в середине 2 отрезка
            PointF df2 = new PointF(mid2.X + px2 * halfLength, mid2.Y + py2 * halfLength);
            PointF ef2 = new PointF(mid2.X - px2 * halfLength, mid2.Y - py2 * halfLength);
            // точка пересечения двух перпендикуляров
            PointF center = SegmentIntersection.Intersection(df1, ef1, df2, ef2);
            var radius = center.Vector(pt1).Length();
            
            #region блок коррекции углов дуги
            
            var angle1 = pt1.Vector(center).AngleDegree(); if (angle1 < 0) angle1 = 360f + angle1;
            var angle2 = pt2.Vector(center).AngleDegree(); if (angle2 < 0) angle2 = 360f + angle2;
            var angle3 = pt3.Vector(center).AngleDegree(); if (angle3 < 0) angle3 = 360f + angle3;
            if (angle2 < angle1) angle2 += 360f;
            if (angle3 < angle1) angle3 += 360f;
            var sweepAngle = angle3 - angle1; if (sweepAngle < 0) sweepAngle = 360f + sweepAngle;
            if (angle2 > angle3) sweepAngle = -360f + sweepAngle;

            #endregion блок коррекции углов дуги

            AddArc(center, radius, angle1, sweepAngle);
        }

        /// <summary>
        /// Добавление прямоугольника по четырём точкам вершин отрезками линий
        /// </summary>
        /// <param name="pt1">Первая точка (левый верхний)</param>
        /// <param name="pt2">Вторая точка (правый верхний)</param>
        /// <param name="pt3">Третья точка (правый нижний)</param>
        /// <param name="pt4">Четвёртая точка (левый нижний)</param>
        /// <param name="loading">Признак загрузки из внешнего источника (для исключения поддержки undo)</param>
        public void AddRectangle(PointF pt1, PointF pt2, PointF pt3, PointF pt4, bool loading = false)
        {
            Figure line1 = CreateLine(pt1, pt2);
            Figure line2 = CreateLine(pt2, pt3);
            Figure line3 = CreateLine(pt3, pt4);
            Figure line4 = CreateLine(pt4, pt1);
            if (loading)
            {
                figures.Add(line1);
                figures.Add(line2);
                figures.Add(line3);
                figures.Add(line4);
            }
            else
                undoRedoManager.Execute(new CreateFiguresCommand(figures, new List<Figure>() { line1, line2, line3, line4 }));
        }

        /// <summary>
        /// Удаление выбранных фигур
        /// </summary>
        public void RemoveSelected()
        {
            foreach (var fig in selectionController.Selection)
                undoRedoManager.Execute(new RemoveFigureCommand(figures, fig));
            selectionController.Clear();
            underCursor.Clear();
            Changed = true;
        }

        /// <summary>
        /// Установка и запоминание режима работы редактора
        /// </summary>
        /// <param name="selection"></param>
        public void SetMode(EditorMode selection)
        {
            editorMode = selection;
            timerClearMouseCount.Enabled = true;
            underCursor.Clear();
            zoomPad.Invalidate();
            switch (editorMode)
            {
                case EditorMode.Selection:
                    // при выборе режима "Выбор фигур" вызывается поключенное событие для обновления интерфейса
                    OnSelectionMode?.Invoke(this, EventArgs.Empty);
                    break;
                case EditorMode.MoveSelected:
                case EditorMode.MoveCopySelected:
                    selectionController.ClearMarkers();
                    OnChangeMode?.Invoke(this, selection);
                    break;
                case EditorMode.MoveMarkers:
                    OnChangeMode?.Invoke(this, selection);
                    break;
                default:
                    selectionController.Clear();
                    OnChangeMode?.Invoke(this, selection);
                    break;
            }
        }

        /// <summary>
        /// Сохранение документа чертежа в xml файле
        /// </summary>
        /// <param name="filename"></param>
        public void SaveDocument(string filename)
        {
            try
            {
                var root = new XElement("Document");
                root.Add(new XAttribute("Name", System.IO.Path.GetFileNameWithoutExtension(filename)));
                var doc = new XDocument(new XComment("Данные чертёжного документа"), root);
                var xmodel = new XElement("Model");
                root.Add(xmodel);
                foreach (var figure in figures)
                {
                    var xfigure = figure.GetXml();
                    xmodel.Add(xfigure);
                }
                doc.Save(filename);
                Changed = false;
                zoomPad.Invalidate();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Загрузка документа чертежа из xml файла
        /// </summary>
        /// <param name="filename"></param>
        public void LoadDocument(string filename)
        {
            try
            {
                undoRedoManager.Clear();
                var xdoc = XDocument.Load(filename);
                var root = xdoc.Element("Document");
                var name = root.Attribute("Name")?.Value;
                var model = root.Element("Model");
                figures.Clear();
                selectionController.Clear();
                zoomPad.Reset();
                foreach (var xelement in model.Descendants())
                {
                    var figureName = $"{xelement.Name}";
                    switch (figureName)
                    {
                        case "Figure":
                            var figure = new Figure();
                            figure.SetXml(xelement, (geometryName) =>
                            {
                                switch (geometryName)
                                {
                                    case "Segment":
                                        return new LineGeometry();
                                    case "Arc":
                                        return new ArcGeometry();
                                    default:
                                        return null;
                                }
                            });
                            figures.Add(figure);
                            break;
                    }
                }
                Changed = false;
                zoomPad.Invalidate();
            }
            catch
            {
                figures.Clear();
                Changed = false;
                throw;
            }
        }

        /// <summary>
        /// Создание нового документа
        /// </summary>
        public void CreateNewDocument()
        {
            figures.Clear();
            selectionController.Clear();
            undoRedoManager.Clear();
            zoomPad.Reset();
            Changed = false;
            zoomPad.Invalidate();
        }

        /// <summary>
        /// Выбор всех фигур в редакторе
        /// </summary>
        public void SelectAll()
        {
            selectionController.Clear();
            foreach (var fig in figures)
                selectionController.Selection.Add(fig);
            selectionController.BuildMarkers(selectionController.Selection);
            zoomPad.Invalidate();
        }

        /// <summary>
        /// Отменить предыдущее действие
        /// </summary>
        public void Undo()
        {
            selectionController.Clear();
            undoRedoManager.Undo();
            OnSelected?.Invoke(this, selectionController.Selection);
            Changed = true;
        }

        /// <summary>
        /// Вернуть отменённое предыдущее действие
        /// </summary>
        public void Redo()
        {
            selectionController.Clear();
            undoRedoManager.Redo();
            OnSelected?.Invoke(this, selectionController.Selection);
            Changed = true;
        }

        /// <summary>
        /// Возможность отмены
        /// </summary>
        /// <returns></returns>
        public bool CanUndo()
        {
            return undoRedoManager.UndoPossible();
        }

        /// <summary>
        /// Возможность вернуть отменённое
        /// </summary>
        /// <returns></returns>
        public bool CanRedo()
        {
            return undoRedoManager.RedoPossible();
        }

        /// <summary>
        /// Переключение в режим Переместить
        /// </summary>
        public void MoveSelected()
        {
            editorMode = EditorMode.MoveSelected;
        }

        /// <summary>
        /// Переключение в режим Копировать и Переместить
        /// </summary>
        public void MoveCopySelected()
        {
            editorMode = EditorMode.MoveCopySelected;
        }

        private void zoomPad_OnPanOrZoom(object sender, ZoomControl.PanOrZoomEventArgs e)
        {
            var pt = e.ViewPort;
            OnToolTipChanged?.Invoke(this, $"Смещение dX:{pt.X} dY:{pt.Y}, зум: {e.Zoom}");
        }

        public void ToolTipChanged(string message)
        {
            OnToolTipChanged?.Invoke(this, message);
        }

        public void EscapeKeyPressed()
        {
            PressRightMouseButton(MousePosition, calledByCode: true);
        }

        public void SetParameters(string[] strings)
        {
            buildLineController.SetParameters(strings);
            buildRectangleController.SetParameters(strings);
            buildArcByThreePointsController.SetParameters(strings);
            switch (editorMode)
            {
                case EditorMode.MoveSelected:
                case EditorMode.MoveCopySelected:
                    if (strings.Length == 2)
                    {
                        if (mouseClickCount == 0)
                        {
                            if (double.TryParse(strings[0], out double ppX) &&
                                double.TryParse(strings[1], out double ppY))
                                SetFirstPoint(ppX, ppY);
                        }
                        else
                        {
                            if (double.TryParse(strings[0], out double shiftX) &&
                                double.TryParse(strings[1], out double shiftY))
                                SetShiftPoint(shiftX, shiftY);
                        }
                    }
                    break;
            }
        }

        private void SetShiftPoint(double shiftX, double shiftY)
        {
            switch (editorMode)
            {
                case EditorMode.MoveSelected:
                    selectionController.Selection.Translate((float)shiftX, (float)shiftY,
                        (movedoffsets) =>
                        {
                            undoRedoManager.Execute(new MoveFiguresCommand(movedoffsets));
                        });
                    // предыдущий выбор стирается, т.к. перемещение - однократная операция
                    selectionController.Selection.Clear();
                    timerClearMouseCount.Enabled = true;
                    SetMode(EditorMode.Selection);
                    Changed = true;
                    break;
                case EditorMode.MoveCopySelected:
                    selectionController.Selection.TranslateCopy((float)shiftX, (float)shiftY,
                        (addedfigs) =>
                        {
                            undoRedoManager.Execute(new CreateFiguresCommand(figures, addedfigs));
                            // предыдущий выбор стирается
                            selectionController.Selection.Clear();
                            // добавленые фигуры добавляюся в выбор
                            foreach (var fig in addedfigs)
                                selectionController.Selection.Add(fig);
                        });
                    Changed = true;
                    OnChangeParams?.Invoke(this, new object[] { new PointF((float)shiftX, (float)shiftY) });
                    break;
            }
        }

        public void SetRectangleWidthAndHeight(double width, double height)
        {
            // построение прямоугольника по двум точкам диагонали
            var pt1 = firstMouseDown; // первая точка нажатия
            var pt2 = new PointF(pt1.X + (float)width, pt1.Y); // раcчётная точка
            var pt3 = new PointF(pt1.X + (float)width, pt1.Y + (float)height); // раcчётная точка
            var pt4 = new PointF(pt1.X, pt1.Y + (float)height); // раcчётная точка
            AddRectangle(pt1, pt2, pt3, pt4);
            // сброс количества нажатий, следующий прямоугольник будет строиться заново
            timerClearMouseCount.Enabled = true;
            Changed = true;
        }

        public void SetFirstPoint(double pxX, double pxY)
        {
            mousePosition = new PointF((float)pxX, (float)pxY);
            PressLeftMouseButton(mousePosition, calledByCode: true);
            zoomPad_MouseMove(zoomPad, new MouseEventArgs(MouseButtons.None, 1, (int)pxX, (int)pxY, 0));
        }

        public void SetLineLengthAndAngle(double length, double angledeg)
        {
            // построение отрезков линий по первой точке (начало отрезка), длине и углу наклона
            var pt1 = firstMouseDown;
            double angleRad = angledeg * (Math.PI / 180);
            var pt2 = new PointF(pt1.X + (float)(length * Math.Cos(angleRad)), pt1.Y + (float)(length * Math.Sin(angleRad)));
            AddLine(pt1, pt2);
            zoomPad_MouseMove(zoomPad, new MouseEventArgs(MouseButtons.None, 1, (int)pt2.X, (int)pt2.Y, 0));
            // точка начала следующего отрезка совпадает с концом предыдущего отрезка
            mouseClickCount = 1;
            firstMouseDown = pt2;
            Changed = true;
        }

        public void AddMouseCount()
        {
            timerAddMouseCount.Enabled = true;
        }

        private void timerAddMouseCount_Tick(object sender, EventArgs e)
        {
            timerAddMouseCount.Enabled = false;
            mouseClickCount++;
        }

        public void ClearMouseCount()
        {
            timerClearMouseCount.Enabled = true;
        }

        private void timerClearMouseCount_Tick(object sender, EventArgs e)
        {
            timerClearMouseCount.Enabled = false;
            if (mouseClickCount > 0)
            {
                mouseClickCount = 0;
            }
        }
    }
}
