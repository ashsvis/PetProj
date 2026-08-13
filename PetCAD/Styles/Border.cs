using PetCAD.Common;
using PetCAD.Figures;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace PetCAD.Styles
{
    /// <summary>
    /// Класс хранения данных контура фигуры
    /// </summary>
    public class Border
    {
        /// <summary>
        /// Конструктор класса хранения данных контура фигуры
        /// </summary>
        public Border()
        {
        }

        /// <summary>
        /// Величина прозрачности цвета контура
        /// </summary>
        public int Opacity { get; set; } = 255;

        /// <summary>
        /// Толщина линии для рисования контура
        /// </summary>
        public float Width { get; set; } = 0f;

        /// <summary>
        /// Цвет для рисования контура (цвет карандаша)
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// Признак возможности рисования контура
        /// </summary>
        public bool IsVisible { get; set; } = true;

        public DashStyle DashStyle { get; set; } = DashStyle.Solid;

        public XElement GetXml()
        {
            var xborder = new XElement("Border");
            var empty = true;
            if (!IsVisible)
            {
                xborder.Add(new XAttribute("IsVisible", IsVisible));
                empty = false;
            }
            if (Color.ToArgb() != Color.Black.ToArgb())
            {
                xborder.Add(new XAttribute("Color", ParseHelper.ColorToString(Color)));
                empty = false;
            }
            if (Opacity < 255)
            {
                xborder.Add(new XAttribute("Opacity", Opacity));
                empty = false;
            }
            if (Width > 0f)
            {
                xborder.Add(new XAttribute("Width", Width));
                empty = false;
            }
            if (DashStyle != DashStyle.Solid)
            {
                xborder.Add(new XAttribute("DashStyle", DashStyle));
                empty = false;
            }
            return empty ? null : xborder;
        }

        public void SetXml(XElement xborder)
        {
            if (xborder == null || xborder.Name != "Border") return;
            IsVisible = ParseHelper.ParseBoolean(xborder.Attribute("IsVisible")?.Value, IsVisible);
            Color = ParseHelper.ParseColor(xborder.Attribute("Color")?.Value, Color);
            Opacity = ParseHelper.ParseInteger(xborder.Attribute("Opacity")?.Value, Opacity);
            Width = ParseHelper.ParseSingle(xborder.Attribute("IsVisible")?.Value, Width);
            DashStyle = ParseHelper.ParseEnumeration(xborder.Attribute("DashStyle")?.Value, DashStyle);
        }

        /// <summary>
        /// Предоставление карандаша для рисования контура
        /// </summary>
        /// <param name="figure">Ссылка на фигуру</param>
        /// <returns></returns>
        public Pen GetPen(Figure figure)
        {
            // возвращаем созданный и настроенный карандаш для контура фигуры
            var pen = new Pen(Color.FromArgb(Opacity, Color), Width);
            if (DashStyle >= 0) pen.DashStyle = DashStyle;
            return pen;
        }

        public Border DeepCopy()
        {
            var border = new Border
            {
                Opacity = Opacity,
                Width = Width,
                Color = Color,
                IsVisible = IsVisible,
                DashStyle = DashStyle
            };
            return border;
        }
    }
}
