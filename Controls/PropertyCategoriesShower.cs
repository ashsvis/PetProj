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
    public partial class PropertyCategoriesShower : UserControl, IEditor<Selection>
    {
        private Selection selection;
        private int updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<EventArgs> Changed = delegate { };

        public PropertyCategoriesShower()
        {
            InitializeComponent();
        }

        public void Build(Selection selection)
        {
            // показываем редактор
            if (selection == null) return; // ничего не строим            

            // запоминаем редактируемый объект
            this.selection = selection;

            if (selection.Count > 1)
            {
                labelCaption.Text = "Выбрано несколько объектов";
                Visible = true;
            }
            else if (selection.First().Geometry.Name == "Layer")
            {
                labelCaption.Text = "Ничего не выбрано";
                Visible = true;
            }
            else
                Visible = false;

            // копируем свойства объекта в GUI
            updating++;


            updating--;
        }

    }
}
