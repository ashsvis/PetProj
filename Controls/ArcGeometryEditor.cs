using PetProj.Geometries;
using PetProj.Selections;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PetProj.Controls
{
    public partial class ArcGeometryEditor : UserControl, IEditor<Selection>
    {
        private Selection selection;
        private int updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<EventArgs> Changed = delegate { };

        public ArcGeometryEditor()
        {
            InitializeComponent();
        }

        public void Build(Selection selection)
        {
            // проверка видимости
            Visible = selection.ForAll(f => f.Geometry is ArcGeometry) && selection.Count == 1;
            // показываем редактор только если одна фигура и это отрезок
            if (!Visible || selection == null) return; // ничего не строим            

            // запоминаем редактируемый объект
            this.selection = selection;

            // получаем список объектов
            var lineStyles = selection.Select(f => f.Geometry as ArcGeometry).ToList();

            // копируем свойства объекта в GUI
            updating++;

            var start = lineStyles.GetProperty(f => f.StartPoint);
            var center = lineStyles.GetProperty(f => f.CenterPoint);
            var end = lineStyles.GetProperty(f => f.EndPoint);
            var radius = lineStyles.GetProperty(f => f.Radius);
            var startAngle = lineStyles.GetProperty(f => f.StartAngle);
            var sweepAngle = lineStyles.GetProperty(f => f.SweepAngle);

            tbStartX.Text = $"{start.X:0.####}";
            tbStartY.Text = $"{start.Y:0.####}";
            tbCenterX.Text = center.X.ToString();
            tbCenterY.Text = center.Y.ToString();
            tbEndX.Text = $"{end.X:0.####}";
            tbEndY.Text = $"{end.Y:0.####}";
            tbRadius.Text = radius.ToString();
            tbStartAngle.Text = startAngle.ToString();
            tbSweepAngle.Text = sweepAngle.ToString();

            CalculateFields(radius, startAngle, sweepAngle);

            updating--;
        }

        private void CalculateFields(float radius, float startAngle, float sweepAngle)
        {
            var sweepRad = Math.PI * sweepAngle / 180;
            var endAngle = startAngle + sweepAngle;
            var arcLength = radius * sweepRad;
            var segmentSquare = 0.5 * radius * radius * (sweepRad - Math.Sin(sweepRad));

            tbEndAngle.Text = $"{endAngle:0.#}";
            tbArcLength.Text = $"{arcLength:0.####}";
            tbSegmentSquare.Text = $"{segmentSquare:0.####}";
        }

        private void UpdateObject()
        {
            if (updating > 0 || selection == null) return; // we are in updating mode

            // вызывем событие
            StartChanging(this, new ChangingEventArgs("Arc Geometry"));

            // получаем список объектов
            var lineStyles = selection.Select(f => f.Geometry as ArcGeometry).ToList();

            // посылаем значения назад из GUI в объект
            lineStyles.SetProperty(f => f.CenterPoint = new PointF(float.Parse(tbCenterX.Text), float.Parse(tbCenterY.Text)));
            lineStyles.SetProperty(f => f.Radius = float.Parse(tbRadius.Text));
            lineStyles.SetProperty(f => f.StartAngle = float.Parse(tbStartAngle.Text));
            lineStyles.SetProperty(f => f.SweepAngle = float.Parse(tbSweepAngle.Text));

            var startAngle = lineStyles.GetProperty(f => f.StartAngle);
            var sweepAngle = lineStyles.GetProperty(f => f.SweepAngle);
            var radius = lineStyles.GetProperty(f => f.Radius);
            CalculateFields(radius, startAngle, sweepAngle);

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
