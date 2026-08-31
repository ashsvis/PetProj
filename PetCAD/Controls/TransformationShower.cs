using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Geometries;
using PetCAD.Selections;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PetCAD.Controls
{
    public partial class TransformationShower : UserControl, IEditor<Selection>
    {
        private Selection selection;
        private int updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<ChangeEventArgs> Changed = delegate { };

        public TransformationShower()
        {
            InitializeComponent();
        }

        public void Build(Selection selection)
        {
            // проверка видимости
            Visible = selection.ForAll(f => f.Geometry is BlockGeometry) && selection.Count == 1;
            // показываем редактор только если одна фигура и это отрезок
            if (!Visible || selection == null) return; // ничего не строим            

            // запоминаем редактируемый объект
            this.selection = selection;

            // получаем список объектов
            var figTrans = selection.Select(f => f.Geometry as BlockGeometry).ToList();

            // копируем свойства объекта в GUI
            updating++;

            var scale = figTrans.GetProperty(f => f.Owner.Transformation.GetSize());
            var angle = figTrans.GetProperty(f => f.Owner.Transformation.GetAngle());

            tbScaleX.Text = $"{scale.Width:0.####}";
            tbScaleY.Text = $"{scale.Height:0.####}";
            tbAngle.Text = angle.ToString();

            updating--;
        }

    }
}
