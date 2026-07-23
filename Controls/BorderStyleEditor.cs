using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PetProj.Selections;

namespace PetProj.Controls
{
    public partial class BorderStyleEditor : UserControl, IEditor<Selection>
    {
        private Selection selection;
        private int updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<EventArgs> Changed = delegate { };

        public BorderStyleEditor()
        {
            InitializeComponent();
            cbPattern.Items.Clear();
            cbPattern.Items.AddRange(GetPenPatternNames()); // получение всех имён доступных типов линий
            cbPattern.SelectedIndex = 0;
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
            Visible = selection.ForAll(f => f.Style.BorderStyle != null); 
            // показываем редактор только если все фигуры содержат свойство BorderStyle
            if (!Visible || selection == null) return; // ничего не строим            

            // запоминаем редактируемый объект
            this.selection = selection;

            // получаем список объектов
            var borderStyles = selection.Select(f => f.Style.BorderStyle).ToList();

            // копируем свойства объекта в GUI
            updating++;

            cbPattern.SelectedIndex = (int)borderStyles.GetProperty(f => f.DashStyle, DashStyle.Solid);
            nudWidth.Value = (decimal)borderStyles.GetProperty(f => f.Width, 0);
            nudOpacity.Value = borderStyles.GetProperty(f => f.Opacity, 255);
            lbColor.BackColor = borderStyles.GetProperty(f => f.Color, Color.Gray);
            cbVisible.Checked = borderStyles.GetProperty(f => f.IsVisible, true);

            updating--;
        }

        private void UpdateObject()
        {
            if (updating > 0 || selection == null) return; // we are in updating mode

            // вызывем событие
            StartChanging(this, new ChangingEventArgs("Border Style"));

            // получаем список объектов
            var borderStyles = selection.Select(f => f.Style.BorderStyle).ToList();

            // посылаем значения назад из GUI в объект
            borderStyles.SetProperty(f => f.DashStyle = (DashStyle)cbPattern.SelectedIndex);
            borderStyles.SetProperty(f => f.Width = (float)nudWidth.Value);
            borderStyles.SetProperty(f => f.Opacity = (int)nudOpacity.Value);
            borderStyles.SetProperty(f => f.Color = lbColor.BackColor);
            borderStyles.SetProperty(f => f.IsVisible = cbVisible.Checked);

            // вызывем событие
            Changed(this, EventArgs.Empty);
        }

        private void cbVisible_CheckedChanged(object sender, EventArgs e)
        {
            lbColor.Enabled = nudWidth.Enabled = nudOpacity.Enabled = cbPattern.Enabled = 
                lbWidth.Enabled = lbPattern.Enabled = lbOpacity.Enabled = cbVisible.Checked;
            UpdateObject();
        }

        private void lbColor_Click(object sender, EventArgs e)
        {
            var dlg = new ColorDialog { Color = lbColor.BackColor };
            if (dlg.ShowDialog() == DialogResult.OK)
                lbColor.BackColor = dlg.Color;
        }

        private void cbPattern_DrawItem(object sender, DrawItemEventArgs e)
        {
            var g = e.Graphics;
            // рисуем фон окна редактора
            e.DrawBackground();
            var rect = new Rectangle(e.Bounds.X, e.Bounds.Top, e.Bounds.Width - 1, e.Bounds.Height - 1);
            rect.Inflate(-4, 0);
            using (var p = new Pen(e.ForeColor))
            {
                p.Width = 2;
                p.DashStyle = (DashStyle) e.Index;
                g.DrawLine(p, new Point(rect.Left, rect.Top + rect.Height/2),
                           new Point(rect.Right, rect.Top + rect.Height/2));
            }
            e.DrawFocusRectangle();
        }
    }
}
