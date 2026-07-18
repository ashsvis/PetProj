using PetProj.Common;
using PetProj.Figures;
using System.Drawing;
using System.Xml.Linq;

namespace PetProj.Styles
{
    /// <summary>
    /// Класс рисовальщика фона фигуры
    /// </summary>
    public abstract class Fill
    {
        /// <summary>
        /// Величина прозрачности цвета заливки
        /// </summary>
        public virtual int Opacity { get; set; }

        /// <summary>
        /// Цвет для заполнения фона (цвет заливки)
        /// </summary>
        public virtual Color Color { get; set; }

        /// <summary>
        /// Признак возможности заливки фигуры
        /// </summary>
        public virtual bool IsVisible { get; set; }

        public XElement GetXml()
        {
            var xfill = new XElement("Fill");
            xfill.Add(new XAttribute("IsVisible", IsVisible));
            xfill.Add(new XAttribute("Color", ParseHelper.ColorToString(Color)));
            xfill.Add(new XAttribute("Opacity", Opacity));
            return xfill;
        }

        public void SetXml(XElement xfill)
        {
            if (xfill == null || xfill.Name != "Fill") return;
            IsVisible = ParseHelper.ParseBoolean(xfill.Attribute("IsVisible")?.Value, IsVisible);
            Color = ParseHelper.ParseColor(xfill.Attribute("Color")?.Value, Color);
            Opacity = ParseHelper.ParseInteger(xfill.Attribute("Opacity")?.Value, Opacity);
        }

        public abstract Brush GetBrush(Figure figure);

        public abstract Fill DeepCopy();

        /// <summary>
        /// Допустимые операции над заливкой
        /// </summary>
        public abstract AllowedFillDecorators AllowedDecorators { get; }
    }
}
