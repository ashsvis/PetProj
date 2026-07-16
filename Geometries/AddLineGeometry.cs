using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace PetProj.Geometries
{
    public sealed class AddLineGeometry : Geometry, IGeometry
    {
        public readonly List<PointF> Points = new List<PointF>();

        public PointF StartPoint => Points.Count > 0 ? Points.First() : PointF.Empty;
        public PointF EndPoint { get; set; }

        /// <summary>
        /// Признак замкнутого контура фигуры
        /// </summary>
        public bool IsClosed { get; set; } = true;

        public bool IsSmoothed { get; set; }

        /// <summary>
        /// Свойство возвращает определённые в конструкторе ограничения для операций
        /// </summary>
        public override AllowedGeometryOperations AllowedOperations { get { return AllowedGeometryOperations.None; } }

        public override GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                if (Points.Count > 0)
                {
                    if (IsSmoothed)
                    {
                        var list = new List<PointF>(Points);
                        list.AddRange(new[] { EndPoint });
                        if (list.Count > 2)
                            path.AddCurve(list.ToArray());
                        else
                            path.AddLines(list.ToArray());
                    }
                    else
                    {
                        path.AddLines(Points.ToArray());
                        path.AddLines(new[] { EndPoint });
                        if (IsClosed)
                            path.CloseFigure();
                    }
                }
                return path;
            }
        }

        /// <summary>
        /// Конструктор, недоступный вне проекта EditorModel
        /// (только для внутреннего использования)
        /// </summary>
        internal AddLineGeometry(PointF startPoint)
        {
            Points.Add(startPoint);
            EndPoint = startPoint;
        }

        public void AddPoint(PointF point)
        {
            if (!Points.Contains(point)) Points.Add(point);
            EndPoint = point;
        }
    }
}
