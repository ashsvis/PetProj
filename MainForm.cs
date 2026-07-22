using PetProj.Common;
using PetProj.Controls;
using PetProj.Selections;
using System;
using System.Drawing;
using System.Linq;
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
            drawControl.OnSelectionMode += drawControl_OnSelectionMode;
            drawControl.OnToolTipChanged += DrawControl_OnToolTipChanged;
            drawControl.OnCursorMoved += DrawControl_OnCursorMoved;
            drawControl.Enter += DrawControl_Enter;
            drawControl.OnSelected += DrawControl_OnSelected;
            placeHolder.Controls.Add(drawControl);
            ConnectEditors();
        }

        private void ConnectEditors()
        {
            panelTools.Controls.Clear();
            var editors = new[]
            {
                typeof(BorderStyleEditor),
                typeof(LineGeometryEditor),
            };
            foreach (var typeName in editors)
            {
                var uc = (UserControl)Activator.CreateInstance(typeName);
                uc.Width = panelTools.ClientSize.Width;
                uc.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                if (uc is IEditor<Selection> figEditor)
                {
                    figEditor.StartChanging += FigEditor_StartChanging;
                    figEditor.Changed += FigEditor_Changed;
                }
                panelTools.Controls.Add(uc);
            }
            BuildInterface();
        }

        private void FigEditor_StartChanging(object sender, ChangingEventArgs e)
        {

        }

        private void FigEditor_Changed(object sender, EventArgs e)
        {
            drawControl.UpdateInterface();
        }

        private  void BuildInterface()
        {
            foreach (var editor in panelTools.Controls.OfType<IEditor<Selection>>()) //get editors of figure
                editor.Build(drawControl.SelectionController.Selection);
        }

        private void DrawControl_OnSelected(object sender, Selection e)
        {
            BuildInterface();
        }

        private void DrawControl_Enter(object sender, EventArgs e)
        {
            if (textBox1.Visible)
            {
                textBox1.Focus();
                textBox1.SelectAll();
            }
        }

        private void DrawControl_OnCursorMoved(object sender, (int clickCount, PointF first, Point location) e)
        {
            if (drawControl.IsDynamicalEnter)
            {
                switch (drawControl.EditorMode)
                {
                    case EditorMode.BuildLines:
                        var pt = new PointF(e.location.X, e.location.Y);
                        float dx = pt.X - e.first.X;
                        float dy = pt.Y - e.first.Y;
                        if (drawControl.IsDrawOrtoMode)
                        {
                            if (dx < dy)
                                pt.X = (int)e.first.X;
                            else
                                pt.Y = (int)e.first.Y;
                        }
                        if (!textBox1.Visible)
                        {
                            textBox1.Visible = false; // true;
                            textBox1.Focus();
                            textBox1.SelectAll();
                        }
                        pt = PointF.Add(pt, new SizeF(5, 5));
                        if (e.clickCount == 0)
                            textBox1.Location = Point.Ceiling(pt);
                        else if (e.clickCount == 1)
                        {
                            var pt1 = Point.Ceiling(drawControl.GetFirstMouseDownPosition());
                            var pt2 = pt;
                            if (pt1 == pt2)
                            {
                                pt2 = PointF.Add(pt2, new SizeF(0, -25));
                                textBox1.Location = Point.Ceiling(pt2);
                            }
                            else
                            {
                                float px = dy;
                                float py = -dx;
                                float length = pt2.Vector(pt1).Length();
                                if (length > 0) // Отрезок не вырожден в точку
                                {
                                    px /= length;
                                    py /= length;
                                    var mid = new PointF((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
                                    var shift = px > 0 ? 50 : -50;
                                    var midpoint = Point.Ceiling(new PointF(mid.X - px * shift, mid.Y - py * shift));
                                    midpoint.Offset(-textBox1.Width / 2, -textBox1.Height / 2);
                                    textBox1.Location = Point.Ceiling(midpoint);
                                }
                            }
                        }
                        if (!textBox2.Visible) textBox2.Visible = false; // true;
                        pt = PointF.Add(pt, new SizeF(textBox1.Width + 5, 0));
                        if (e.clickCount == 0)
                            textBox2.Location = Point.Ceiling(pt);
                        else if (e.clickCount == 1)
                        {
                            var pt1 = Point.Ceiling(drawControl.GetFirstMouseDownPosition());
                            var pt2 = pt;
                            if (pt1 == pt2)
                            {
                                pt2 = PointF.Add(pt2, new SizeF(0, 25));
                                textBox2.Location = Point.Ceiling(pt2);
                            }
                            else
                            {
                                float length = pt2.Vector(pt1).Length();
                                var angle = Math.Abs(pt2.Vector(pt1).AngleDegree());
                                if (angle > 20 && length > textBox2.Width)
                                {
                                    angle /= 2;
                                    var kf = (dy < 0) ? -1 : 1;
                                    var endX = pt1.X + length * Math.Cos(angle * Math.PI / 180);
                                    var endY = pt1.Y + kf * length * Math.Sin(angle * Math.PI / 180);
                                    var location = new Point((int)endX, (int)endY);
                                    location.Offset(-textBox2.Width / 2, -textBox2.Height / 2);
                                    textBox2.Location = location;
                                }
                                else
                                {
                                    pt2 = PointF.Add(pt2, new SizeF(0, 15));
                                    textBox2.Location = Point.Ceiling(pt2);
                                }
                            }
                        }
                        // показываем значения координат в полях ввода
                        var ploc = drawControl.PrepareMousePosition(e.location);
                        var vector = ploc.Vector(e.first);
                        textBox1.Text = e.clickCount == 0 ? ploc.X.ToString() : vector.Length().ToString();
                        textBox1.SelectAll();
                        textBox2.Text = e.clickCount == 0 ? ploc.Y.ToString() : $"{vector.AngleDegree()}°";
                        textBox2.SelectAll();
                        break;
                    default:
                        if (textBox1.Visible) textBox1.Visible = false;
                        if (textBox2.Visible) textBox2.Visible = false;
                        break;
                }
            }
            else
            {
                if (textBox1.Visible) textBox1.Visible = false;
                if (textBox2.Visible) textBox2.Visible = false;
            }
        }

        private void DrawControl_OnToolTipChanged(object sender, string text)
        {
            tsslStatus.Text = text;
        }

        /// <summary>
        /// Метод события при программном переключении режимов редактора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawControl_OnSelectionMode(object sender, EventArgs e)
        {
            drawControl.OnSelectionMode -= drawControl_OnSelectionMode;
            // переключение режимов редактора при нажатии на кнопки интерфейса
            SelectEditorMode(tsbArrow);
            drawControl.OnSelectionMode += drawControl_OnSelectionMode;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            drawControl.IsDrawOrtoMode = Properties.Settings.Default.ModeDrawOrto;
            drawControl.IsDynamicalEnter = Properties.Settings.Default.ModeDynamicalEnter;
            ShowHideLeftPanel(Properties.Settings.Default.HideLeftPanel);
            timerUpdateControls.Enabled = true;
        }

        /// <summary>
        /// Выбор базового режима редактора: выбор фигур
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbArrow_Click(object sender, EventArgs e)
        {
            SelectEditorMode(sender);
        }

        /// <summary>
        /// Выключение выбора для всех кнопок интерфейса команд
        /// </summary>
        private void SwitchOffButtons()
        {
            tsbArrow.Checked = false;
            tsbLine.Checked = false;
            tsbRect.Checked = false;
            tsbMove.Checked = false;
            tsbMoveCopy.Checked = false;
        }

        /// <summary>
        /// Переключение режимов редактора при нажатии на кнопки интерфейса
        /// </summary>
        /// <param name="sender"></param>
        private void SelectEditorMode(object sender)
        {
            if (sender == tsbArrow)
            {
                SwitchOffButtons();
                drawControl.SetMode(EditorMode.Selection);
                tsbArrow.Checked = true;
            }
            else if (sender == tsbLine)
            {
                SwitchOffButtons();
                drawControl.SetMode(EditorMode.BuildLines);
                tsbLine.Checked = true;
            }
            else if (sender == tsbRect)
            {
                SwitchOffButtons();
                drawControl.SetMode(EditorMode.BuildRectangles);
                tsbRect.Checked = true;
            }
            else if (sender == tsbMove)
            {
                SwitchOffButtons();
                drawControl.SetMode(EditorMode.MoveSelected);
                tsbMove.Checked = true;
            }
            else if (sender == tsbMoveCopy)
            {
                SwitchOffButtons();
                drawControl.SetMode(EditorMode.MoveCopySelected);
                tsbMoveCopy.Checked = true;
            }
        }

        /// <summary>
        /// Событие таймера для обновления вида управляющих элементов формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUpdateControls_Tick(object sender, EventArgs e)
        {
            var changed = drawControl.Changed;
            tsmiSaveDocument.Enabled = changed;
            tsbSaveDocument.Enabled = changed;
            if (drawControl is IUndoRedoSupport support)
            {
                tsbUndo.Enabled = tsmiUndo.Enabled = support.CanUndo();
                tsbRedo.Enabled = tsmiRedo.Enabled = support.CanRedo();
            }
            tsmiMove.Enabled = tsbMove.Enabled = drawControl.SelectionController.Selection.Count > 0;
            tsmiMoveCopy.Enabled = tsbMoveCopy.Enabled = drawControl.SelectionController.Selection.Count > 0;
            tsmiDelete.Enabled = tsbCopy.Enabled = tsmiCopy.Enabled = tsbCut.Enabled = tsmiCut.Enabled = 
                drawControl.EditorMode == EditorMode.Selection && drawControl.SelectionController.Selection.Count > 0;
        }

        /// <summary>
        /// Сохранить документ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Открыть сохранённый ранее документ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Создать новый документ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiCreateDocument_Click(object sender, EventArgs e)
        {
            drawControl.CreateNewDocument();
        }

        /// <summary>
        /// Выбрать всё
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiSelectAll_Click(object sender, EventArgs e)
        {
            drawControl.SelectAll();
        }

        /// <summary>
        /// Обработка дейстия кнопки для режима Отменить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiUndo_Click(object sender, EventArgs e)
        {
            if (drawControl is IUndoRedoSupport support && support.CanUndo())
                support.Undo();
        }

        /// <summary>
        /// Обработка дейстия кнопки для режима Вернуть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiRedo_Click(object sender, EventArgs e)
        {
            if (drawControl is IUndoRedoSupport support && support.CanRedo())
                support.Redo();
        }

        /// <summary>
        /// Обработка дейстия кнопки Удалить выбранное
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            drawControl.RemoveSelected();
        }

        /// <summary>
        /// Обработка дейстия кнопки для режима Переместить выбранное
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiMove_Click(object sender, EventArgs e)
        {
            SelectEditorMode(sender);
            drawControl.MoveSelected();
        }

        /// <summary>
        /// Обработка дейстия кнопки для режима Копировать выбранное и Переместить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiMoveCopy_Click(object sender, EventArgs e)
        {
            SelectEditorMode(sender);
            drawControl.MoveCopySelected();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (textBox1.Visible) textBox1.Visible = false;
                if (textBox2.Visible) textBox2.Visible = false;
                drawControl.EscapeKeyPressed();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                switch (drawControl.EditorMode)
                {
                    case EditorMode.BuildLines:
                        if (drawControl.MouseClickCount == 0)
                        {
                            // ввод координат X и Y
                            if (double.TryParse(textBox1.Text, out double x) &&
                                double.TryParse(textBox2.Text, out double y))
                            {
                                textBox1.Focus();
                                drawControl.SetFirstPoint(x, y);
                            }
                        }
                        else if (drawControl.MouseClickCount == 1)
                        {
                            // ввод длины отрезка и угла наклона к горизонтали
                            if (double.TryParse(textBox1.Text, out double length) &&
                                double.TryParse(textBox2.Text.TrimEnd('°'), out double angle))
                            {
                                textBox1.Focus();
                                drawControl.SetLineLengthAndAngle(length, angle);
                            }
                        }
                        break;
                }
            }
        }

        private void btnHideShowLeftPanel_Click(object sender, EventArgs e)
        {
            ShowHideLeftPanel(btnHideShowLeftPanel.Text == "«");
        }

        private void ShowHideLeftPanel(bool hide)
        {
            Properties.Settings.Default.HideLeftPanel = hide;
            Properties.Settings.Default.Save();
            if (hide)
            {
                propsHolder.Tag = propsHolder.Width;
                propsHolder.Width = splitterHolders.MinSize;
                btnHideShowLeftPanel.Text = "»";
                splitterHolders.Visible = false;
                toolTip1.SetToolTip(btnHideShowLeftPanel, "Показать панель");
            }
            else
            {
                propsHolder.Width = (int)(propsHolder.Tag ?? 250);
                btnHideShowLeftPanel.Text = "«";
                splitterHolders.Visible = true;
                toolTip1.SetToolTip(btnHideShowLeftPanel, "Спрятать панель");
            }
        }

        private void panLeftCaption_Paint(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            var rect = ((Panel)sender).ClientRectangle;
            rect.Offset(0, btnHideShowLeftPanel.Height - 1);
            rect.Height -= btnHideShowLeftPanel.Height + 1;
            rect.Width -= 1;
            gr.DrawRectangle(SystemPens.ControlDarkDark, rect);
            var caption = "Свойства фигур";
            using (var font = new Font("Segoe UI", 10f, FontStyle.Bold))
            {
                var sz = gr.MeasureString(caption, font);
                var gs = gr.Save();
                gr.TranslateTransform(0f, (rect.Height + sz.Width + btnHideShowLeftPanel.Height) / 2f);
                gr.RotateTransform(-90f);
                gr.DrawString(caption, font, SystemBrushes.ActiveCaptionText, PointF.Empty);
                gr.Restore(gs);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            panLeftCaption.Invalidate();
        }

        /// <summary>
        /// Включение/отключение режима динамического ввода F12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDynamicalEnter_Click(object sender, EventArgs e)
        {
            var mode = !drawControl.IsDynamicalEnter;
            drawControl.IsDynamicalEnter = mode;
            drawControl.UpdateInterface();
            textBox1.Visible = drawControl.Focused && mode;
            textBox2.Visible = drawControl.Focused && mode;
            Properties.Settings.Default.ModeDynamicalEnter = mode;
            Properties.Settings.Default.Save();
        }

        private void tsmiOrto_Click(object sender, EventArgs e)
        {
            var mode = !drawControl.IsDrawOrtoMode;
            drawControl.IsDrawOrtoMode = mode;
            Properties.Settings.Default.ModeDrawOrto = mode;
            Properties.Settings.Default.Save();
        }
    }
}
