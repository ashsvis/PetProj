using PetCAD.Makers;
using System.Linq;
using System.Windows.Forms;

namespace PetCAD.Dialogs
{
    public partial class DrawingModesForm : Form
    {
        public DrawingModesForm(string tabName = null)
        {
            InitializeComponent();
            switch (tabName)
            {
                case "Объектная привязка":
                    tabControl1.SelectedIndex = 1;
                    break;
            }
        }

        private void DrawingModesForm_Load(object sender, System.EventArgs e)
        {
            cbObjectBindingMode.Checked = Properties.Settings.Default.ModeObjectBinding;
            var allowedObjectBindings = (AllowedObjectBindings)Properties.Settings.Default.ObjectBindingFlags;
            cbBindingToEndPoint.Checked = allowedObjectBindings.HasFlag(AllowedObjectBindings.EndPoint);
            cbBindingToMiddle.Checked = allowedObjectBindings.HasFlag(AllowedObjectBindings.Middle);
            cbBindingToCenter.Checked = allowedObjectBindings.HasFlag(AllowedObjectBindings.Center);
            cbBindingToNormal.Checked = allowedObjectBindings.HasFlag(AllowedObjectBindings.Normal);
            cbBindingToQuadrant.Checked = allowedObjectBindings.HasFlag(AllowedObjectBindings.Quadrant);
            cbBindingToTangent.Checked = allowedObjectBindings.HasFlag(AllowedObjectBindings.Tangent);
        }

        private void btnSelectAllBindings_Click(object sender, System.EventArgs e)
        {
            foreach (var cbox in gbBindingModes.Controls.OfType<CheckBox>())
            {
                cbox.Checked = true;
            }
        }

        private void btnClearAllBindings_Click(object sender, System.EventArgs e)
        {
            foreach (var cbox in gbBindingModes.Controls.OfType<CheckBox>())
            {
                cbox.Checked = false;
            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            Properties.Settings.Default.ModeObjectBinding = cbObjectBindingMode.Checked;
            var allowedObjectBindings = new AllowedObjectBindings();
            if (cbBindingToEndPoint.Checked)
                allowedObjectBindings = allowedObjectBindings ^ AllowedObjectBindings.EndPoint;
            if (cbBindingToMiddle.Checked)
                allowedObjectBindings = allowedObjectBindings ^ AllowedObjectBindings.Middle;
            if (cbBindingToCenter.Checked)
                allowedObjectBindings = allowedObjectBindings ^ AllowedObjectBindings.Center;
            if (cbBindingToNormal.Checked)
                allowedObjectBindings = allowedObjectBindings ^ AllowedObjectBindings.Normal;
            if (cbBindingToQuadrant.Checked)
                allowedObjectBindings = allowedObjectBindings ^ AllowedObjectBindings.Quadrant;
            if (cbBindingToTangent.Checked)
                allowedObjectBindings = allowedObjectBindings ^ AllowedObjectBindings.Tangent;
            Properties.Settings.Default.ObjectBindingFlags = (uint)allowedObjectBindings;
            Properties.Settings.Default.Save();
        }
    }
}
