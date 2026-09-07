using PetCAD.Geometries;
using System.Linq;
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

        private void BlockDefinitionForm_Load(object sender, System.EventArgs e)
        {
            cbBlockName.Items.AddRange(BlockGeometry.DefinedBlocks.Select(x => (object)x.Key).ToArray());
        }

        private void cbBlockName_TextChanged(object sender, System.EventArgs e)
        {
            btnOk.Enabled = !string.IsNullOrEmpty(cbBlockName.Text);
        }
    }
}
