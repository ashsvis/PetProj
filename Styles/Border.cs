using PetProj.Figures;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace PetProj.Styles
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
            // по умолчанию заливка разрешена
            IsVisible = true;
            // по умолчанию полная непрозрачность
            Opacity = 255;
            // по умолчанию чёрный цвет контура
            Color = Color.Black;
            // по умолчанию 1 единица
            Width = 1f;
            // по умолчанию сплошная линия
            DashStyle = DashStyle.Solid;
        }

        /// <summary>
        /// Величина прозрачности цвета контура
        /// </summary>
        public int Opacity { get; set; }

        /// <summary>
        /// Толщина линии для рисования контура
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Цвет для рисования контура (цвет карандаша)
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Признак возможности рисования контура
        /// </summary>
        public bool IsVisible { get; set; }

        public DashStyle DashStyle { get; set; }

        public XElement GetXml()
        {
            var xborder = new XElement("Border");
            xborder.Add(new XAttribute("IsVisible", IsVisible));
            xborder.Add(new XAttribute("Color", $"{Color.A};{Color.R};{Color.G};{Color.B}"));
            xborder.Add(new XAttribute("Opacity", Opacity));
             xborder.Add(new XAttribute("Width", Width));
           xborder.Add(new XAttribute("DashStyle", DashStyle));
            return xborder;
        }

        /// <summary>
        /// Предоставление карандаша для рисования контура
        /// </summary>
        /// <param name="figure">Ссылка на фигуру</param>
        /// <returns></returns>
        public Pen GetPen(Figure figure)
        {
            // возвращаем созданный и настроенный карандаш для контура фигуры
            return new Pen(Color.FromArgb(Opacity, Color), Width) { DashStyle = DashStyle };
        }
    }
}
