using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PetProj.Common;
using PetProj.Geometries;
using PetProj.Selections;

namespace PetProj.Controls
{
    public partial class LineGeometryEditor : UserControl, IEditor<Selection>
    {
        private Selection selection;
        private int updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<EventArgs> Changed = delegate { };

        public LineGeometryEditor()
        {
            InitializeComponent();
            //cbPattern.Items.Clear();
            //cbPattern.Items.AddRange(GetPenPatternNames()); // получение всех имён доступных типов линий
            //cbPattern.SelectedIndex = 0;
        }

        static readonly DashStyle[] DashStyleArray = (DashStyle[])Enum.GetValues(typeof(DashStyle));

        static readonly int DashStyleCount = DashStyleArray.Length - 1;

        public static object[] GetPenPatternNames()
        {
            var dashNameArray = Enum.GetNames(typeof(DashStyle));
            var names = new object[DashStyleCount];
            for (var i = 0; i < DashStyleCount; i++)
                names[i] = dashNameArray[i];
            return names;
        }

        public void Build(Selection selection)
        {
            // проверка видимости
            Visible = selection.ForAll(f => f.Geometry is AddLineGeometry) && selection.Count == 1; 
            // показываем редактор только если одна фигура и это отрезок
            if (!Visible || selection == null) return; // ничего не строим            

            // запоминаем редактируемый объект
            this.selection = selection;

            // получаем список объектов
            var lineStyles = selection.Select(f => f.Geometry as AddLineGeometry).ToList();

            // копируем свойства объекта в GUI
            updating++;

            var start = lineStyles.GetProperty(f => f.StartPoint);
            var end = lineStyles.GetProperty(f => f.EndPoint);
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            var angle = Math.Atan2(dy, dx) * 180 / Math.PI;
            if (angle < 0) angle = 360 + angle;
            tbStartX.Text = start.X.ToString();
            tbStartY.Text = start.Y.ToString();
            tbEndX.Text = end.X.ToString();
            tbEndY.Text = end.Y.ToString();
            tbDeltaX.Text = $"{dx}";
            tbDeltaY.Text = $"{dy}";
            tbLength.Text = $"{length:F1}";
            tbAngle.Text = $"{angle:F1}";

            updating--;
        }

        private void UpdateObject()
        {
            if (updating > 0 || selection == null) return; // we are in updating mode

            // вызывем событие
            StartChanging(this, new ChangingEventArgs("Line Geometry"));

            // получаем список объектов
            var lineStyles = selection.Select(f => f.Geometry as AddLineGeometry).ToList();

            // посылаем значения назад из GUI в объект
            lineStyles.SetProperty(f => f.Points[0] = new PointF(float.Parse(tbStartX.Text), float.Parse(tbStartY.Text)));
            lineStyles.SetProperty(f => f.Points[1] = f.EndPoint = new PointF(float.Parse(tbEndX.Text), float.Parse(tbEndY.Text)));

            // вызывем событие
            Changed(this, EventArgs.Empty);
        }

        private void tbText_Validated(object sender, EventArgs e)
        {
            try
            {
                UpdateObject();
                errorProv.Clear();
            }
            catch 
            {
                var tbox = (TextBox)sender;
                errorProv.SetError(tbox, $"{tbox.Text} не число!");
                tbox.Focus();
            }
        }
    }
}
