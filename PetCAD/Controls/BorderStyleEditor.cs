using PetCAD.Selections;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PetCAD.Controls
{
    public partial class BorderStyleEditor : UserControl, IEditor<Selection>
    {
        private Selection selection;
        private int updating;

        public event EventHandler<ChangingEventArgs> StartChanging = delegate { };
        public event EventHandler<ChangeEventArgs> Changed = delegate { };

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

            if (borderStyles.GetProperty(f => f.DashStyle, out DashStyle style))
                cbPattern.SelectedIndex = (int)style;
            else
                cbPattern.SelectedIndex = -1;

            if (borderStyles.GetProperty(f => f.Width, out float width))
                nudWidth.Text = $"{width}";
            else
                nudWidth.Text = "";

            if (borderStyles.GetProperty(f => f.Opacity, out int opacity) && opacity >= 0 && opacity < 256)
                nudOpacity.Text = $"{opacity}";
            else
                nudOpacity.Text = "";

            if (borderStyles.GetProperty(f => f.Color, out Color color))
            {
                lbColor.BackColor = color;
                lbColor.Tag = color;
                lbColor.Image = null;
            }
            else
            {
                lbColor.BackColor = Color.Transparent;
                lbColor.Tag = null;
                lbColor.Image = Properties.Resources.transparent4;
            }

            if (borderStyles.GetProperty(f => f.IsVisible, out bool visible))
                cbVisible.Checked = visible;
            else
                cbVisible.CheckState = CheckState.Indeterminate;

            updating--;
        }

        private void cbVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (cbVisible.CheckState != CheckState.Indeterminate)
            {
                if (updating > 0 || selection == null) return; // we are in updating mode
                // получаем список объектов
                var borderStyles = selection.ToList();
                borderStyles.SetProperty(f =>
                {
                    Changed(this, new ChangeEventArgs("BorderStyleIsVisible", f, cbVisible.Checked));
                });
            }
        }

        private void lbColor_Click(object sender, EventArgs e)
        {
            var dlg = new ColorDialog { Color = lbColor.BackColor };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lbColor.BackColor = dlg.Color;
                lbColor.Tag = dlg.Color;
                lbColor.Image = null;
                if (updating > 0 || selection == null) return; // we are in updating mode
                // получаем список объектов
                var borderStyles = selection.ToList();
                borderStyles.SetProperty(f =>
                {
                    Changed(this, new ChangeEventArgs("BorderStyleColor", f, lbColor.BackColor));
                });
            }
        }

        private void cbPattern_DrawItem(object sender, DrawItemEventArgs e)
        {
            var g = e.Graphics;
            // рисуем фон окна редактора
            e.DrawBackground();
            var rect = new Rectangle(e.Bounds.X, e.Bounds.Top, e.Bounds.Width - 1, e.Bounds.Height - 1);
            rect.Inflate(-4, 0);
            if (e.Index >= 0)
            {
                using (var p = new Pen(e.ForeColor))
                {
                    p.Width = 2;
                    p.DashStyle = (DashStyle)e.Index;
                    g.DrawLine(p, new Point(rect.Left, rect.Top + rect.Height / 2),
                               new Point(rect.Right, rect.Top + rect.Height / 2));
                }
            }
            e.DrawFocusRectangle();
        }

        private void nudOpacity_Validated(object sender, EventArgs e)
        {
            if (updating > 0 || selection == null) return; // we are in updating mode
            // получаем список объектов
            var borderStyles = selection.ToList();
            if (byte.TryParse(nudOpacity.Text, out byte opacity))
            {
                errorProv.Clear();
                borderStyles.SetProperty(f =>
                {
                    Changed(this, new ChangeEventArgs("BorderStyleOpacity", f, opacity));
                });
            }
            else
            {
                errorProv.SetError(nudOpacity, $"{nudOpacity.Text} не допустимо здесь!");
                nudOpacity.Focus();
            }
        }

        private void nudWidth_Validated(object sender, EventArgs e)
        {
            if (updating > 0 || selection == null) return; // we are in updating mode
            // получаем список объектов
            var borderStyles = selection.ToList();
            if (float.TryParse(nudWidth.Text, out float width))
            {
                errorProv.Clear();
                borderStyles.SetProperty(f =>
                {
                    Changed(this, new ChangeEventArgs("BorderStyleWidth", f, width));
                });
            }
            else
            {
                errorProv.SetError(nudWidth, $"{nudWidth.Text} не число!");
                nudWidth.Focus();
            }
        }

        private void cbPattern_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (updating > 0 || selection == null) return; // we are in updating mode
            // получаем список объектов
            var borderStyles = selection.ToList();
            if (cbPattern.SelectedIndex >= 0)
            {
                borderStyles.SetProperty(f =>
                {
                    Changed(this, new ChangeEventArgs("BorderStyleDashStyle", f, (DashStyle)cbPattern.SelectedIndex));
                });
            }
        }
    }
}
