using PetCAD.Figures;
using PetCAD.Geometries;
using PetCAD.Selections;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PetCAD.Controls
{
    public partial class LineGeometryEditor : UserControl, IEditor<Selection>
    {
        private Figure figure;
        private Selection selection;
        private int updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<ChangeEventArgs> Changed = delegate { };

        public LineGeometryEditor()
        {
            InitializeComponent();
        }

        public void Build(Selection selection)
        {
            figure = null;
            // проверка видимости
            Visible = selection.ForAll(f => f.Geometry is LineGeometry) && selection.Count == 1;
            // показываем редактор только если одна фигура и это отрезок
            if (!Visible || selection == null) return; // ничего не строим            

            // запоминаем редактируемый объект
            this.selection = selection;

            figure = selection.First();

            // получаем список объектов
            var lineStyles = selection.Select(f => f.Geometry as LineGeometry).ToList();

            // копируем свойства объекта в GUI
            updating++;

            var start = lineStyles.GetProperty(f => f.StartPoint);
            var end = lineStyles.GetProperty(f => f.EndPoint);
            tbStartX.Text = start.X.ToString();
            tbStartY.Text = start.Y.ToString();
            tbEndX.Text = end.X.ToString();
            tbEndY.Text = end.Y.ToString();
            CalculateFields(start, end);

            updating--;
        }

        private void CalculateFields(PointF start, PointF end)
        {
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            var angle = Math.Atan2(dy, dx) * 180 / Math.PI;
            if (angle < 0) angle = 360 + angle;
            tbDeltaX.Text = $"{dx}";
            tbDeltaY.Text = $"{dy}";
            tbLength.Text = $"{length:0.####}";
            tbAngle.Text = $"{angle:0.#}";
        }

        //private void UpdateObject()
        //{
        //    if (updating > 0 || selection == null) return; // we are in updating mode

        //    // вызывем событие
        //    StartChanging(this, new ChangingEventArgs("LineGeometry", figure.Geometry));

        //    // получаем список объектов
        //    var lineStyles = selection.Select(f => f.Geometry as LineGeometry).ToList();

        //    // посылаем значения назад из GUI в объект
        //    lineStyles.SetProperty(f => f.Points[0] = new PointF(float.Parse(tbStartX.Text), float.Parse(tbStartY.Text)));
        //    lineStyles.SetProperty(f => f.Points[1] = new PointF(float.Parse(tbEndX.Text), float.Parse(tbEndY.Text)));

        //    var start = lineStyles.GetProperty(f => f.StartPoint);
        //    var end = lineStyles.GetProperty(f => f.EndPoint);
        //    CalculateFields(start, end);

        //    // вызывем событие
        //    Changed(this, new ChangeEventArgs("LineGeometry", figure.Geometry));
        //}

        //private void tbText_Validated(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        UpdateObject();
        //        errorProv.Clear();
        //    }
        //    catch
        //    {
        //        var tbox = (TextBox)sender;
        //        errorProv.SetError(tbox, $"{tbox.Text} не число!");
        //        tbox.Focus();
        //    }
        //}

        private void tbStartX_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "LineGeometryStartX");
        }

        private void tbStartY_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "LineGeometryStartY");
        }

        private void tbEndX_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "LineGeometrybEndX");
        }

        private void tbEndY_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "LineGeometryEndY");
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
