using PetProj.Common;
using System;
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
        private void MainForm_Load(object sender, EventArgs e)
        {
            timerUpdateControls.Enabled = true;
        }

        private void tsbArrow_Click(object sender, EventArgs e)
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
                drawControl.SetMode(EditorMode.BuildRectangles);
                tsbRect.Checked = true;
            }
        }
        private void timerUpdateControls_Tick(object sender, EventArgs e)
        {
            var changed = drawControl.Changed;
            tsmiSaveDocument.Enabled = changed;
            tsbSaveDocument.Enabled = changed;
            tsslStatus.Text = $"Выбрано объектов: {drawControl.SelectionCount}";
            if (drawControl is IUndoRedoSupport support)
            {
                tsbUndo.Enabled = tsmiUndo.Enabled = support.CanUndo();
                tsbRedo.Enabled = tsmiRedo.Enabled = support.CanRedo();
            }
            tsmiMove.Enabled = tsbMove.Enabled = drawControl.SelectionCount > 0;
            tsmiDelete.Enabled = tsbCopy.Enabled = tsmiCopy.Enabled = tsbCut.Enabled = tsmiCut.Enabled = 
                drawControl.EditorMode == EditorMode.Selection && drawControl.SelectionCount > 0;
        }

        private void tsmiSaveDocument_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog() 
            {
                Title = "Сохранение чертежа",
                FileName = "Чертёж.gxml",
                DefaultExt = "gxml",
                Filter = "Файл графического документа (*.gxml)|*.gxml" 
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    drawControl.SaveDocument(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Сохранение чертежа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsmiOpenDocument_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                Title = "Загрузка ранее сохранённого чертежа",
                FileName = "Чертёж.gxml",
                DefaultExt = "gxml",
                Filter = "Файл графического документа (*.gxml)|*.gxml",
                Multiselect = false,
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    drawControl.LoadDocument(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Загрузка чертежа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsmiCreateDocument_Click(object sender, EventArgs e)
        {
            drawControl.CreateNewDocument();
        }

        private void tsmiSelectAll_Click(object sender, EventArgs e)
        {
            drawControl.SelectAll();
        }

        private void tsmiUndo_Click(object sender, EventArgs e)
        {
            if (drawControl is IUndoRedoSupport support && support.CanUndo())
                support.Undo();
        }

        private void tsmiRedo_Click(object sender, EventArgs e)
        {
            if (drawControl is IUndoRedoSupport support && support.CanRedo())
                support.Redo();
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            drawControl.RemoveSelected();
        }

        private void tsmiMove_Click(object sender, EventArgs e)
        {
            drawControl.MoveSelected();
        }
    }
}
