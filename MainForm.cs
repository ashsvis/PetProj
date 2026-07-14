using System.Windows.Forms;

namespace PetProj
{
    public partial class MainForm : Form
    {
        private readonly DrawControl drawControl;

        public MainForm()
        {
            InitializeComponent();
            drawControl = new DrawControl() { Dock = DockStyle.Fill };
            drawControl.OnSelectionMode += (o, e) => 
            {
                tsbLine.Checked = false;
                tsbRect.Checked = false;
                tsbArrow.Checked = true;
            };
            placeHolder.Controls.Add(drawControl);
        }

        private void tsbArrow_Click(object sender, System.EventArgs e)
        {
            SelectEditorMode(sender);
        }

        private void SelectEditorMode(object sender)
        {
            if (sender == tsbArrow)
            {
                tsbLine.Checked = false;
                tsbRect.Checked = false;
                drawControl.SetMode(EditorMode.Selection);
                tsbArrow.Checked = true;
            }
            else if (sender == tsbLine)
            {
                tsbArrow.Checked = false;
                tsbRect.Checked = false;
                drawControl.SetMode(EditorMode.BuildLines);
                tsbLine.Checked = true;
            }
            else if (sender == tsbRect)
            {
                tsbArrow.Checked = false;
                tsbLine.Checked = false;
                drawControl.SetMode(EditorMode.BuildRectangle);
                tsbRect.Checked = true;
            }
        }
    }
}
