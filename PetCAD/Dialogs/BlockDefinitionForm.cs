using System.Windows.Forms;

namespace PetCAD.Dialogs
{
    public partial class BlockDefinitionForm : Form
    {
        public BlockDefinitionForm()
        {
            InitializeComponent();
        }

        public string EnteredBlockName => cbBlockName.Text;

        private void cbBlockName_TextChanged(object sender, System.EventArgs e)
        {
            btnOk.Enabled = !string.IsNullOrEmpty(cbBlockName.Text);
        }
    }
}
