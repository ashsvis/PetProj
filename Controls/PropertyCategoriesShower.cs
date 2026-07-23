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
                var categories = selection.Select(f => f.Geometry.Name).Distinct().ToList();
                if (categories.Count == 1)
                    labelCaption.Text = $"{categories.First()} ({selection.Count})";
                else
                    labelCaption.Text = $"Несколько фигур ({selection.Count})";
            }
            else if (selection.First().Geometry.Name == "Слой")
            {
                labelCaption.Text = "Ничего не выбрано";
            }
            else 
            {
                labelCaption.Text = selection.First().Geometry.Name;
            }

            // копируем свойства объекта в GUI
            updating++;


            updating--;
        }

    }
}
