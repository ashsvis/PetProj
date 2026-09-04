using PetCAD.Figures;
using PetCAD.Geometries;
using PetCAD.Selections;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PetCAD.Controls
{
    public partial class CircleGeometryEditor : UserControl, IEditor<Selection>
    {
        private Figure figure;
        private Selection selection;
        private int updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<ChangeEventArgs> Changed = delegate { };

        public CircleGeometryEditor()
        {
            InitializeComponent();
        }

        public void Build(Selection selection)
        {
            figure = null;
            // проверка видимости
            Visible = selection.ForAll(f => f.Geometry is CircleGeometry) && selection.Count == 1;
            // показываем редактор только если одна фигура и это отрезок
            if (!Visible || selection == null) return; // ничего не строим            

            // запоминаем редактируемый объект
            this.selection = selection;

            figure = selection.First();

            // получаем список объектов
            var lineStyles = selection.Select(f => f.Geometry as CircleGeometry).ToList();

            // копируем свойства объекта в GUI
            updating++;

            var center = lineStyles.GetProperty(f => f.CenterPoint);
            var radius = lineStyles.GetProperty(f => f.Radius);

            tbCenterX.Text = center.X.ToString();
            tbCenterY.Text = center.Y.ToString();
            tbRadius.Text = radius.ToString();

            CalculateFields(radius);

            updating--;
        }

        private void CalculateFields(float radius)
        {
            var arcLength = Math.PI * 2 * radius;
            var circleSquare = Math.PI * radius * radius;

            tbArcLength.Text = $"{arcLength:0.#}";
            tbSegmentSquare.Text = $"{circleSquare:0.#}";
        }

        private void tbCenterX_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "CircleGeometryCenterX");
        }

        private void tbCenterY_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "CircleGeometryCenterY");
        }

        private void tbRadius_Validated(object sender, EventArgs e)
        {
            SendChanges(sender, "CircleGeometryRadius");
            // получаем список объектов
            var lineStyles = selection.Select(f => f.Geometry as CircleGeometry).ToList();
            var radius = lineStyles.GetProperty(f => f.Radius);
            CalculateFields(radius);
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
