using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace PetProj.Figures
{
    /*
    public class Line : Figure
    {
        /// <summary>
        /// Конструктор для создания из редактора
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        public Line(PointF point1, PointF point2) 
        { 
            Origin = point1;
            Offsets.Add(new SizeF(point2.X - point1.X, point2.Y - point1.Y));
        }

        /// <summary>
        /// Конструктор для создания при загрузке из файла
        /// </summary>
        /// <param name="xelement"></param>
        public Line(XElement xelement)
        {
            SetData(xelement);
        }

        /// <summary>
        /// Нарисовать отрезок заданным цветом
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="forecolor"></param>
        public override void DrawAt(Graphics graphics, Color forecolor)
        {
            var pt1 = Origin;
            var pt2 = Offsets.Count == 1 ? PointF.Add(pt1, Offsets[0]) : Origin;
            using (var pen = new Pen(forecolor))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                graphics.DrawLine(pen, pt1, pt2);
            }
        }

        /// <summary>
        /// Рисование выделенной линии с маркерами
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="forecolor"></param>
        public override void DrawSelectedAt(Graphics graphics, Color forecolor)
        {
            var pt1 = Origin;
            var pt2 = Offsets.Count == 1 ? PointF.Add(pt1, Offsets[0]) : Origin;
            var selColor = Color.FromArgb(0, 127, 255);
            using (var pen = new Pen(Color.FromArgb(64, selColor), 3))
                graphics.DrawLine(pen, pt1, pt2);
            using (var pen = new Pen(Color.FromArgb(128, selColor), 2))
                graphics.DrawLine(pen, pt1, pt2);
            using (var pen = new Pen(forecolor))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                graphics.DrawLine(pen, pt1, pt2);
            }
            using (var brush = new SolidBrush(selColor))
                graphics.FillRectangles(brush, GetMarkers());
            using (var pen = new Pen(Color.Gray, 0))
                graphics.DrawRectangles(pen, GetMarkers());
        }

        /// <summary>
        /// Получение массива маркерных квадратиков
        /// </summary>
        /// <returns></returns>
        public override RectangleF[] GetMarkers()
        {
            var pt1 = Origin;
            var pt2 = Offsets.Count == 1 ? PointF.Add(pt1, Offsets[0]) : Origin;
            var middle = new PointF(pt1.X + (pt2.X - pt1.X) / 2f, pt1.Y + (pt2.Y - pt1.Y) / 2f);
            return new RectangleF[] { GetSquare(pt1), GetSquare(middle), GetSquare(pt2) };
        }

        /// <summary>
        /// Вычисление размера маркерного квадратика
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private RectangleF GetSquare(PointF point)
        {
            var sz = new SizeF(6f, 6f);
            return new RectangleF(new PointF(point.X - sz.Width / 2f, point.Y - sz.Height / 2f), sz);
        }

        /// <summary>
        /// Рисование подсвеченного отрезка
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="forecolor"></param>
        public override void DrawHighlightAt(Graphics graphics, Color forecolor)
        {
            var pt1 = Origin;
            var pt2 = Offsets.Count == 1 ? PointF.Add(pt1, Offsets[0]) : Origin;
            using (var pen = new Pen(Color.FromArgb(64, forecolor), 3))
                graphics.DrawLine(pen, pt1, pt2);
            using (var pen = new Pen(Color.FromArgb(128, forecolor), 2))
                graphics.DrawLine(pen, pt1, pt2);
            using (var pen = new Pen(forecolor))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                graphics.DrawLine(pen, pt1, pt2);
            }
        }

        /// <summary>
        /// Точка аргумента попадает на отрезок
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contains(PointF point)
        {
            var pt1 = Origin;
            var pt2 = Offsets.Count == 1 ? PointF.Add(pt1, Offsets[0]) : Origin;
            using (var path = new GraphicsPath())
            {
                path.AddLine(pt1, pt2);
                using (var pen = new Pen(Color.Black, 3))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    return path.IsOutlineVisible(point, pen);
                }
            }
        }

        /// <summary>
        /// Начальная точка отрезка
        /// </summary>
        public PointF StartPoint => Origin;

        /// <summary>
        /// Конечная точка отрезка
        /// </summary>
        public PointF EndPoint => Offsets.Count == 1 ? PointF.Add(Origin, Offsets[0]) : Origin;

        /// <summary>
        /// Коррекция начальной точки отрезка
        /// </summary>
        /// <param name="point"></param>
        public void MoveStartPointTo(PointF point)
        {
            var endPoint = EndPoint;
            Origin = point;
            var sz = new SizeF(endPoint.X - Origin.X, endPoint.Y - Origin.Y);
            if (Offsets.Count == 1)
                Offsets[0] = sz;
            else
                Offsets.Add(sz);
        }

        /// <summary>
        /// Коррекция конечной точки отрезка
        /// </summary>
        /// <param name="point"></param>
        public void MoveEndPointTo(PointF point)
        {
            var sz = new SizeF(point.X - Origin.X, point.Y - Origin.Y);
            if (Offsets.Count == 1)
                Offsets[0] = sz;
            else 
                Offsets.Add(sz);
        }

        /// <summary>
        /// Получение данных конфигурации отрезка
        /// </summary>
        /// <returns></returns>
        public override XElement GetData()
        {
            var xline = new XElement("Line");
            xline.Add(new XAttribute("Start", StartPoint));
            xline.Add(new XAttribute("End", EndPoint));
            return xline;
        }

        /// <summary>
        /// Восстановление данных конфигурации отрезка
        /// </summary>
        /// <param name="xelement"></param>
        private void SetData(XElement xelement)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            var decsep = culture.NumberFormat.NumberDecimalSeparator;
            var point1 = Parse(xelement.Attribute("Start")?.Value, decsep);
            var point2 = Parse(xelement.Attribute("End")?.Value, decsep);
            Origin = point1;
            Offsets.Clear();
            Offsets.Add(new SizeF(point2.X - point1.X, point2.Y - point1.Y));
        }

        /// <summary>
        /// Разбор символьной записи для получения координат точки
        /// </summary>
        /// <param name="line"></param>
        /// <param name="decimalseparator"></param>
        /// <returns></returns>
        private PointF Parse(string line, string decimalseparator)
        {
            // Разбиваем по запятым и убираем пустые элементы
            string[] tokens = line.Trim('{','}').Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 2)
            {
                string valueX = tokens[0].Split('=').Last();
                string valueY = tokens[1].Split('=').Last();

                switch (decimalseparator)
                {
                    case ".":
                        valueX = valueX.Replace(',', '.');
                        valueY = valueY.Replace(',', '.');
                        break;
                    case ",":
                        valueX = valueX.Replace('.', ',');
                        valueY = valueY.Replace('.', ',');
                        break;
                }

                // Проверяем, что удалось успешно преобразовать обе координаты
                if (float.TryParse(valueX, out float x) && float.TryParse(valueY, out float y))
                    return new PointF(x, y);
            }
            return PointF.Empty;
        }
    }
    */
}
