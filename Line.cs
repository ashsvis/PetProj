using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace PetProj
{
    public class Line : Primitive
    {
        public Line(PointF point1, PointF point2) 
        { 
            Origin = point1;
            Offsets.Add(new SizeF(point2.X - point1.X, point2.Y - point1.Y));
        }

        public Line(XElement xelement)
        {
            SetData(xelement);
        }

        public override void DrawAt(Graphics graphics, Color forecolor)
        {
            if (Offsets.Count == 1)
            {
                var pt1 = Origin;
                var pt2 = PointF.Add(pt1, Offsets[0]);
                using (var pen = new Pen(forecolor))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.DrawLine(pen, pt1, pt2);
                }
            }
        }

        public override void DrawHighlightAt(Graphics graphics, Color forecolor)
        {
            if (Offsets.Count == 1)
            {
                var pt1 = Origin;
                var pt2 = PointF.Add(pt1, Offsets[0]);
                using (var pen = new Pen(Color.FromArgb(80, forecolor), 3))
                {
                    graphics.DrawLine(pen, pt1, pt2);
                }
                using (var pen = new Pen(forecolor))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.DrawLine(pen, pt1, pt2);
                }
            }
        }

        public override bool Contains(PointF point)
        {
            if (Offsets.Count == 1)
            {
                var pt1 = Origin;
                var pt2 = PointF.Add(pt1, Offsets[0]);
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
            return false;
        }

        public PointF StartPoint => Origin;
        public PointF EndPoint => Offsets.Count == 1 ? PointF.Add(Origin, Offsets[0]) : Origin;

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

        public void MoveEndPointTo(PointF point)
        {
            var sz = new SizeF(point.X - Origin.X, point.Y - Origin.Y);
            if (Offsets.Count == 1)
                Offsets[0] = sz;
            else 
                Offsets.Add(sz);
        }

        public override XElement GetData()
        {
            var xline = new XElement("Line");
            xline.Add(new XAttribute("Start", StartPoint));
            xline.Add(new XAttribute("End", EndPoint));
            return xline;
        }

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
}
