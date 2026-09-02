using PetCAD.Figures;
using PetCAD.Geometries;
using PetCAD.Selections;
using System;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace PetCAD.Controls
{
    public partial class ArcGeometryEditor : UserControl, IEditor<Selection>
    {
        private Figure figure;
        private Selection selection;
        private int updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<ChangeEventArgs> Changed = delegate { };

        public ArcGeometryEditor()
        {
            InitializeComponent();
        }

        public void Build(Selection selection)
        {
            figure = null;
            // проверка видимости
            Visible = selection.ForAll(f => f.Geometry is ArcGeometry) && selection.Count == 1;
            // показываем редактор только если одна фигура и это отрезок
            if (!Visible || selection == null) return; // ничего не строим            

            // запоминаем редактируемый объект
            this.selection = selection;

            figure = selection.First();

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
            var arcLength = Math.Abs(radius * sweepRad);
            var segmentSquare = Math.Abs(0.5 * radius * radius * (sweepRad - Math.Sin(sweepRad)));

            tbEndAngle.Text = $"{endAngle:0.#}";
            tbArcLength.Text = $"{arcLength:0.#}";
            tbSegmentSquare.Text = $"{segmentSquare:0.#}";
        }

        private void UpdateObject()
        {
            if (updating > 0 || selection == null) return; // we are in updating mode
            // получаем список объектов
            var geometryStyles = selection.ToList();

            // вызывем событие
            StartChanging(this, new ChangingEventArgs("ArcGeometry", figure.Geometry));

            // получаем список объектов
            var arcStyles = selection.Select(f => f.Geometry as ArcGeometry).ToList();

            // посылаем значения назад из GUI в объект
            arcStyles.SetProperty(f => f.CenterPoint = new PointF(float.Parse(tbCenterX.Text), float.Parse(tbCenterY.Text)));
            arcStyles.SetProperty(f => f.Radius = float.Parse(tbRadius.Text));
            arcStyles.SetProperty(f => f.StartAngle = float.Parse(tbStartAngle.Text));
            arcStyles.SetProperty(f => f.SweepAngle = float.Parse(tbSweepAngle.Text));

            var startAngle = arcStyles.GetProperty(f => f.StartAngle);
            var sweepAngle = arcStyles.GetProperty(f => f.SweepAngle);
            var radius = arcStyles.GetProperty(f => f.Radius);
            CalculateFields(radius, startAngle, sweepAngle);

            // вызывем событие
            Changed(this, new ChangeEventArgs("ArcGeometry", figure.Geometry));
        }

        private void tbCenterX_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "ArcGeometryCenterX");
        }

        private void tbCenterY_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "ArcGeometryCenterY");
        }

        private void tbRadius_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "ArcGeometryRadius");
        }

        private void tbStartAngle_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "ArcGeometryStartAngle");
        }

        private void tbSweepAngle_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "ArcGeometrySweepAngle");
        }

        private void SendChanges(object sender, string id)
        {
            if (updating > 0 || selection == null) return; // we are in updating mode
            // получаем список объектов
            var arcGeometries = selection.ToList();
            var tbox = (TextBox)sender;
            if (float.TryParse(tbox.Text, out float value))
            {
                errorProv.Clear();
                arcGeometries.SetProperty(f =>
                {
                    Changed(this, new ChangeEventArgs(id, f, value));
                });
            }
            else
            {
                errorProv.SetError(tbox, $"{tbox.Text} не число!");
                tbox.Focus();
            }
        }
    }
}
