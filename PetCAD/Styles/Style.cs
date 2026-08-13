using System.Xml.Linq;

namespace PetCAD.Styles
{
    /// <summary>
    /// Класс стилей фигуры
    /// </summary>
    public class Style
    {
        /// <summary>
        /// Свойство для хранения данных для карандаша
        /// </summary>
        public Border BorderStyle { get; set; }

        /// <summary>
        /// Свойство для хранения данных кисти
        /// </summary>
        public Fill FillStyle { get; set; }

        public XElement GetXml()
        {
            var xstyle = new XElement("Style");
            var xBorderStyle = BorderStyle.GetXml();
            if (xBorderStyle != null)
                xstyle.Add(xBorderStyle);
            var xFillStyle = FillStyle.GetXml(); 
            if (xFillStyle != null)
                xstyle.Add(xFillStyle);
            return xBorderStyle == null && xFillStyle == null ? null : xstyle;
        }

        public void SetXml(XElement xstyle)
        {
            if (xstyle == null || xstyle.Name != "Style") return;
            BorderStyle.SetXml(xstyle.Element("Border"));
            FillStyle.SetXml(xstyle.Element("Fill"));
        }

        public Style DeepCopy()
        {
            var style = new Style
            {
                BorderStyle = BorderStyle.DeepCopy(),
                FillStyle = FillStyle.DeepCopy()
            };
            return style;
        }

        /// <summary>
        /// Конструктор стилей, для задания свойств по умолчанию
        /// </summary>
        public Style()
        {
            BorderStyle = new Border();
            FillStyle = new DefaultFill();
        }
    }
}
