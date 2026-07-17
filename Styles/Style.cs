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
