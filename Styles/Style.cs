using System.Xml.Linq;

namespace PetProj.Styles
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
            xstyle.Add(BorderStyle.GetXml());
            xstyle.Add(FillStyle.GetXml());
            return xstyle;
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
