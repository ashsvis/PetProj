using PetCAD.Commands;
using PetCAD.Common;
using PetCAD.Controls;
using PetCAD.Dialogs;
using PetCAD.Figures;
using PetCAD.Geometries;
using PetCAD.Makers;
using PetCAD.Selections;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PetCAD
{
    public partial class MainForm : Form
    {
        private readonly DrawControl drawControl;
        private string workedFileName = string.Empty;

        public MainForm()
        {
            InitializeComponent();
            drawControl = new DrawControl() { Dock = DockStyle.Fill };
            drawControl.OnToolTipChanged += DrawControl_OnToolTipChanged;
            drawControl.OnSelectionMode += drawControl_OnSelectionMode;
            drawControl.OnChangeParams += DrawControl_OnChangeParams;
            drawControl.OnCursorMoved += DrawControl_OnCursorMoved;
            drawControl.OnChangeMode += DrawControl_OnChangeMode;
            drawControl.OnSelected += DrawControl_OnSelected;
            placeHolder.Controls.Add(drawControl);
            ConnectEditors();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var modeOrto = Properties.Settings.Default.ModeDrawOrto;
            drawControl.IsDrawOrthoMode = modeOrto;
            tsmiOrto.Checked = modeOrto;
            tsbOrto.Checked = modeOrto;
            var modeDynamicEnter = Properties.Settings.Default.ModeDynamicalEnter;
            drawControl.IsDynamicalEnter = modeDynamicEnter;
            tsmiDynamicalEnter.Checked = modeDynamicEnter;
            tsbDynamicalEnter.Checked = modeDynamicEnter;
            tslParamName1.Visible = false;
            tstbTextParam1.Visible = false;
            tslParamName2.Visible = false;
            tstbTextParam2.Visible = false;
            tsbObjectBinding.DropDown.Closing += DropDown_Closing;
            LoadObjectBindingsSettings();
            ShowHideLeftPanel(Properties.Settings.Default.HideLeftPanel);
            timerUpdateControls.Enabled = true;
            BuildInterface();
        }

        private void LoadObjectBindingsSettings()
        {
            var modeObjBinding = Properties.Settings.Default.ModeObjectBinding;
            drawControl.IsObjectBinding = modeObjBinding;
            tsmiObjectBinding.Checked = modeObjBinding;
            tsbObjectBinding.Checked = modeObjBinding;
            drawControl.AllowedObjectBindings = (AllowedObjectBindings)Properties.Settings.Default.ObjectBindingFlags;
            tsmiBindToEndPoint.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.EndPoint);
            tsmiBindToMiddle.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Middle);
            tsmiBindToCenter.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Center);
            tsmiBindToNormal.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Normal);
            tsmiBindToQuadrant.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Quadrant);
            tsmiBindToTangent.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Tangent);
        }

        private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                e.Cancel = true; // Отменяем закрытие
            }
        }

        private void DrawControl_OnChangeParams(object sender, object[] parametes)
        {
            if (drawControl.IsDynamicalEnter)
            {
                tslParamName1.Visible = true;
                tstbTextParam1.Visible = true;
                tslParamName2.Visible = true;
                tstbTextParam2.Visible = true;
                switch (drawControl.EditorMode)
                {
                    case EditorMode.BuildLines:
                        if (drawControl.MouseClickCount == 0)
                        {
                            var pt = (PointF)parametes[0];
                            tslParamName1.Text = "X:";
                            tstbTextParam1.Text = pt.X.ToString();
                            tstbTextParam1.Focus();
                            tstbTextParam1.SelectAll();
                            tslParamName2.Text = "Y:";
                            tstbTextParam2.Text = pt.Y.ToString();
                        }
                        else
                        {
                            tslParamName1.Text = "Длина:";
                            tstbTextParam1.Text = $"{parametes[0]}";
                            tstbTextParam1.Focus();
                            tstbTextParam1.SelectAll();
                            tslParamName2.Text = "Угол:";
                            tstbTextParam2.Text = $"{parametes[1]}";
                        }
                        break;
                    case EditorMode.BuildRectangle:
                        if (drawControl.MouseClickCount == 0)
                        {
                            var pt = (PointF)parametes[0];
                            tslParamName1.Text = "X:";
                            tstbTextParam1.Text = pt.X.ToString();
                            tstbTextParam1.Focus();
                            tstbTextParam1.SelectAll();
                            tslParamName2.Text = "Y:";
                            tstbTextParam2.Text = pt.Y.ToString();
                        }
                        else
                        {
                            tslParamName1.Text = "Ширина:";
                            tstbTextParam1.Text = $"{parametes[0]}";
                            tstbTextParam1.Focus();
                            tstbTextParam1.SelectAll();
                            tslParamName2.Text = "Высота:";
                            tstbTextParam2.Text = $"{parametes[1]}";
                        }
                        break;
                    case EditorMode.MoveSelected:
                    case EditorMode.MoveCopySelected:
                        if (drawControl.MouseClickCount == 0)
                        {
                            var pt = (PointF)parametes[0];
                            tslParamName1.Text = "X:";
                            tstbTextParam1.Text = pt.X.ToString();
                            tstbTextParam1.Focus();
                            tstbTextParam1.SelectAll();
                            tslParamName2.Text = "Y:";
                            tstbTextParam2.Text = pt.Y.ToString();
                        }
                        else
                        {
                            var pt = (PointF)parametes[0];
                            tslParamName1.Text = "Смещение X:";
                            tstbTextParam1.Text = pt.X.ToString();
                            tstbTextParam1.Focus();
                            tstbTextParam1.SelectAll();
                            tslParamName2.Text = "Смещение Y:";
                            tstbTextParam2.Text = pt.Y.ToString();
                        }
                        break;
                }
            }
        }

        private void DrawControl_OnChangeMode(object sender, EditorMode e)
        {
            DrawControl_OnChangeParams(sender, new object[] { PointF.Empty });
        }

        private void ConnectEditors()
        {
            panelTools.Controls.Clear();
            var editors = new[]
            {
                typeof(PropertyCategoriesShower),
                typeof(BorderStyleEditor),
                typeof(LineGeometryEditor),
                typeof(ArcGeometryEditor),
                typeof(BlockGeometryEditor),
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

        private void FigEditor_Changed(object sender, ChangeEventArgs e)
        {
            switch (e.Name)
            {
                case "BorderStyleWidth":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig1 && e.Arguments[1] is float width)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeBorderWidthCommand(fig1, width));
                        drawControl.Changed = true;
                    }
                    break;
                case "BorderStyleColor":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig2 && e.Arguments[1] is Color color)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeBorderColorCommand(fig2, color));
                        drawControl.Changed = true;
                    }
                    break;
                case "BorderStyleOpacity":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig3 && e.Arguments[1] is int opacity)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeBorderOpacityCommand(fig3, opacity));
                        drawControl.Changed = true;
                    }
                    break;
                case "BorderStyleDashStyle":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig4 && e.Arguments[1] is DashStyle dashStyle)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeBorderDashStyleCommand(fig4, dashStyle));
                        drawControl.Changed = true;
                    }
                    break;
                case "BorderStyleIsVisible":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig5 && e.Arguments[1] is bool isVisible)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeBorderIsVisibleCommand(fig5, isVisible));
                        drawControl.Changed = true;
                    }
                    break;
                case "ArcGeometryCenterX":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig6 && e.Arguments[1] is float centerX)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeArcCenterXCommand(fig6, centerX));
                        drawControl.Changed = true;
                    }
                    break;
                case "ArcGeometryCenterY":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig7 && e.Arguments[1] is float centerY)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeArcCenterYCommand(fig7, centerY));
                        drawControl.Changed = true;
                    }
                    break;
                case "ArcGeometryRadius":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig8 && e.Arguments[1] is float radius)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeArcRadiusCommand(fig8, radius));
                        drawControl.Changed = true;
                    }
                    break;
                case "ArcGeometryStartAngle":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig9 && e.Arguments[1] is float startAngle)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeArcStartAngleCommand(fig9, startAngle));
                        drawControl.Changed = true;
                    }
                    break;
                case "ArcGeometrySweepAngle":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig10 && e.Arguments[1] is float sweepAngle)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeArcSweepAngleCommand(fig10, sweepAngle));
                        drawControl.Changed = true;
                    }
                    break;
                case "LineGeometryStartX":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig11 && e.Arguments[1] is float startX)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeLineStartXCommand(fig11, startX));
                        drawControl.Changed = true;
                    }
                    break;
                case "LineGeometryStartY":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig12 && e.Arguments[1] is float startY)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeLineStartYCommand(fig12, startY));
                        drawControl.Changed = true;
                    }
                    break;
                case "LineGeometrybEndX":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig13 && e.Arguments[1] is float endX)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeLineEndXCommand(fig13, endX));
                        drawControl.Changed = true;
                    }
                    break;
                case "LineGeometryEndY":
                    if (e.Arguments.Length == 2 && e.Arguments[0] is Figure fig14 && e.Arguments[1] is float endY)
                    {
                        drawControl.UndoRedoManager.Execute(new ChangeLineEndYCommand(fig14, endY));
                        drawControl.Changed = true;
                    }
                    break;
            }
            drawControl.SelectionController.BuildMarkers(drawControl.SelectionController.Selection);
            drawControl.UpdateInterface();
        }

        private  void BuildInterface()
        {
            var selection = drawControl.SelectionController.Selection;
            if (selection.Count > 0)
            { 
                foreach (var editor in panelTools.Controls.OfType<IEditor<Selection>>())
                    editor.Build(selection);
            }
            else
            {
                var layerselection = new Selection { drawControl.Layer };
                foreach (var editor in panelTools.Controls.OfType<IEditor<Selection>>())
                    editor.Build(layerselection);
            }
        }

        private void DrawControl_OnSelected(object sender, Selection e)
        {
            BuildInterface();
        }

        private void DrawControl_OnCursorMoved(object sender, (int clickCount, PointF first, Point location) e)
        {

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
            tslParamName1.Visible = false;
            tstbTextParam1.Visible = false;
            tslParamName2.Visible = false;
            tstbTextParam2.Visible = false;
            // переключение режимов редактора при нажатии на кнопки интерфейса
            SelectEditorMode(tsbArrow);
            drawControl.OnSelectionMode += drawControl_OnSelectionMode;
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
            tsbCircle.Checked = false;
            tsbArc.Checked = false;
            tsbCreateBlock.Checked = false;
            tsbInsertBlock.Checked = false;
            tsbMove.Checked = false;
            tsbMoveCopy.Checked = false;
            tsbScale.Checked = false;
            tsbRotate.Checked = false;
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
                drawControl.SetMode(EditorMode.BuildRectangle);
                tsbRect.Checked = true;
            }
            else if (sender == tsbCircle)
            {
                SwitchOffButtons();
                drawControl.SetMode((EditorMode)(tsbCircle.Tag ?? EditorMode.BuildCircR));
                tsbCircle.Checked = true;
            }
            else if (sender == tsbArc)
            {
                SwitchOffButtons();
                drawControl.SetMode((EditorMode)(tsbArc.Tag ?? EditorMode.BuildArcThreePoints));
                tsbArc.Checked = true;
            }
            else if (sender == tsbCreateBlock)
            {
                var frm = new BlockDefinitionForm();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (!BlockGeometry.DefinedBlocks.ContainsKey(frm.EnteredBlockName))
                    {
                        SwitchOffButtons();
                        drawControl.DefineBlockName(frm.EnteredBlockName);
                        drawControl.SetMode(EditorMode.BuildCreateBlock);
                        tsbCreateBlock.Checked = true;
                    }
                    else
                    {
                        MessageBox.Show("Существующий блок " + frm.EnteredBlockName + " не изменён.\n" +
                            "Для продолжения измените существующий блок или укажите другое имя блока.",
                            "Блок - изменения не внесены", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else if (sender == tsbInsertBlock)
            {
                SwitchOffButtons();
                drawControl.DefineBlockName($"{tsbInsertBlock.Tag}");
                drawControl.SetMode(EditorMode.BuildInsertBlock);
                tsbInsertBlock.Checked = true;
            }
            else if (sender == tsbMove)
            {
                SwitchOffButtons();
                drawControl.SetMode(EditorMode.MoveSelected);
                tsbMove.Checked = true;
            }
            else if (sender == tsbRotate)
            {
                SwitchOffButtons();
                drawControl.SetMode(EditorMode.RotateSelected);
                tsbRotate.Checked = true;
            }
            else if (sender == tsbMoveCopy)
            {
                SwitchOffButtons();
                drawControl.SetMode(EditorMode.MoveCopySelected);
                tsbMoveCopy.Checked = true;
            }
            else if (sender == tsbScale)
            {
                SwitchOffButtons();
                drawControl.SetMode(EditorMode.ScaleSelected);
                tsbScale.Checked = true;
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
            tsmiRotate.Enabled = tsbRotate.Enabled = drawControl.SelectionController.Selection.Count > 0;
            tsmiMoveCopy.Enabled = tsbMoveCopy.Enabled = drawControl.SelectionController.Selection.Count > 0;
            tsmiScale.Enabled = tsbScale.Enabled = drawControl.SelectionController.Selection.Count > 0;
            tsmiDelete.Enabled = tsbCopy.Enabled = tsmiCopy.Enabled = tsbCut.Enabled = tsmiCut.Enabled = 
                drawControl.EditorMode == EditorMode.Selection && drawControl.SelectionController.Selection.Count > 0;
            tsmiSaveDocumentAs.Enabled = !string.IsNullOrEmpty(workedFileName);
            tsbInsertBlock.Enabled = BlockGeometry.DefinedBlocks.Count > 0;
            tsbExplode.Enabled = drawControl.SelectionController.Selection.Count > 0 &&
                drawControl.SelectionController.Selection.All(x => x.Geometry is IExplodeGeometry);
        }

        /// <summary>
        /// Сохранить документ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiSaveDocument_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(workedFileName))
            {
                var dlg = new SaveFileDialog()
                {
                    Title = "Сохранение чертежа",
                    FileName = "",
                    DefaultExt = "gxml",
                    Filter = "Файл графического документа (*.gxml)|*.gxml"
                };
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        drawControl.SaveDocument(dlg.FileName);
                        workedFileName = dlg.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Сохранение чертежа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
                try
                {
                    drawControl.SaveDocument(workedFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Сохранение чертежа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void tsmiSaveDocumentAs_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog()
            {
                Title = "Сохранение чертежа под другим именем",
                InitialDirectory = System.IO.Path.GetDirectoryName(workedFileName),
                FileName = System.IO.Path.GetFileNameWithoutExtension(workedFileName),
                DefaultExt = "gxml",
                Filter = "Файл графического документа (*.gxml)|*.gxml"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    drawControl.SaveDocument(dlg.FileName);
                    workedFileName = dlg.FileName;
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
                    workedFileName = dlg.FileName;
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
            workedFileName = string.Empty;
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
            using (var font = new Font("Arial", 10f, FontStyle.Regular))
            {
                var sz = gr.MeasureString(caption, font);
                var gs = gr.Save();
                gr.TranslateTransform(0, (rect.Height + sz.Width + btnHideShowLeftPanel.Height) / 2f);
                gr.RotateTransform(-90f);
                gr.DrawString(caption, font, SystemBrushes.ActiveCaptionText, new PointF(0, 2f));
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
            tsmiDynamicalEnter.Checked = mode;
            tsbDynamicalEnter.Checked = mode;
            drawControl.IsDynamicalEnter = mode;
            drawControl.UpdateInterface();
            tslParamName1.Visible = mode;
            tstbTextParam1.Visible = mode;
            tslParamName2.Visible = mode;
            tstbTextParam2.Visible = mode;
            Properties.Settings.Default.ModeDynamicalEnter = mode;
            Properties.Settings.Default.Save();
        }

        private void tsmiOrto_Click(object sender, EventArgs e)
        {
            var mode = !drawControl.IsDrawOrthoMode;
            tsmiOrto.Checked = mode;
            tsbOrto.Checked = mode;
            drawControl.IsDrawOrthoMode = mode;
            Properties.Settings.Default.ModeDrawOrto = mode;
            Properties.Settings.Default.Save();
        }

        private void tsmiObjectBinding_Click(object sender, EventArgs e)
        {
            var mode = !drawControl.IsObjectBinding;
            tsmiObjectBinding.Checked = mode;
            tsbObjectBinding.Checked = mode;
            drawControl.IsObjectBinding = mode;
            Properties.Settings.Default.ModeObjectBinding = mode;
            Properties.Settings.Default.Save();
        }

        private void tsbObjectBinding_Paint(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            var rect = tsbObjectBinding.Bounds;
            rect.Inflate(-3, -3);
            gr.DrawRectangle(Pens.Red, rect);
        }

        private void tsbObjectBinding_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == tsmiBindParameters)
            {
                ((ToolStripSplitButton)sender).DropDown.Close();
                var dlg = new DrawingModesForm("Объектная привязка");
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadObjectBindingsSettings();
                }
                return;
            }
            if (e.ClickedItem == tsmiBindToEndPoint)
            {
                drawControl.AllowedObjectBindings = drawControl.AllowedObjectBindings ^ AllowedObjectBindings.EndPoint;
                tsmiBindToEndPoint.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.EndPoint);
            }
            else if (e.ClickedItem == tsmiBindToMiddle)
            {
                drawControl.AllowedObjectBindings = drawControl.AllowedObjectBindings ^ AllowedObjectBindings.Middle;
                tsmiBindToMiddle.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Middle);
            }
            else if (e.ClickedItem == tsmiBindToCenter)
            {
                drawControl.AllowedObjectBindings = drawControl.AllowedObjectBindings ^ AllowedObjectBindings.Center;
                tsmiBindToCenter.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Center);
            }
            else if (e.ClickedItem == tsmiBindToNormal)
            {
                drawControl.AllowedObjectBindings = drawControl.AllowedObjectBindings ^ AllowedObjectBindings.Normal;
                tsmiBindToNormal.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Normal);
            }
            else if (e.ClickedItem == tsmiBindToQuadrant)
            {
                drawControl.AllowedObjectBindings = drawControl.AllowedObjectBindings ^ AllowedObjectBindings.Quadrant;
                tsmiBindToQuadrant.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Quadrant);
            }
            else if (e.ClickedItem == tsmiBindToTangent)
            {
                drawControl.AllowedObjectBindings = drawControl.AllowedObjectBindings ^ AllowedObjectBindings.Tangent;
                tsmiBindToTangent.Checked = drawControl.AllowedObjectBindings.HasFlag(AllowedObjectBindings.Tangent);
            }
            else
                return;
            Properties.Settings.Default.ObjectBindingFlags = (uint)drawControl.AllowedObjectBindings;
            Properties.Settings.Default.Save();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    drawControl.EscapeKeyPressed();
                    break;
                case Keys.Enter:
                    if (tstbTextParam1.Focused || tstbTextParam2.Focused)
                        drawControl.SetParameters(new string[] { tstbTextParam1.Text, tstbTextParam2.Text });
                    break;
            }
        }

        private void tsmiBuildArcByThreePoints_Click(object sender, EventArgs e)
        {
            SwitchOffButtons();
            drawControl.SetMode(EditorMode.BuildArcThreePoints);
            tsbArc.Image = ((ToolStripMenuItem)sender).Image;
            tsbArc.Tag = EditorMode.BuildArcThreePoints;
            tsbArc.Checked = true;
        }

        private void tsmiBuildArcByBeginCenterEnd_Click(object sender, EventArgs e)
        {
            SwitchOffButtons();
            drawControl.SetMode(EditorMode.BuildArcStartCenterEnd);
            tsbArc.Image = ((ToolStripMenuItem)sender).Image;
            tsbArc.Tag = EditorMode.BuildArcStartCenterEnd;
            tsbArc.Checked = true;
        }

        private void tsmiBuildArcByCenterBeginEnd_Click(object sender, EventArgs e)
        {
            SwitchOffButtons();
            drawControl.SetMode(EditorMode.BuildArcCenterStartEnd);
            tsbArc.Image = ((ToolStripMenuItem)sender).Image;
            tsbArc.Tag = EditorMode.BuildArcCenterStartEnd;
            tsbArc.Checked = true;
        }

        private void tsbInsertBlock_ButtonClick(object sender, EventArgs e)
        {
            tsbInsertBlock.ShowDropDown();
        }

        private void tsbInsertBlock_DropDownOpening(object sender, EventArgs e)
        {
            tsbInsertBlock.DropDownItems.Clear();
            var defblocks = BlockGeometry.DefinedBlocks.Keys;
            if (defblocks.Count > 0)
            {
                foreach (var key in BlockGeometry.DefinedBlocks.Keys.OrderBy(x => x))
                {
                    var item = new ToolStripMenuItem() { Text = key };
                    tsbInsertBlock.DropDownItems.Add(item);
                    item.Click += (o, a) => 
                    {
                        tsbInsertBlock.Tag = ((ToolStripMenuItem)o).Text;
                        SelectEditorMode(tsbInsertBlock);
                    };
                }
            }
            else
            {
                var item = new ToolStripMenuItem() { Text = "Нет определений блоков" };
                tsbInsertBlock.DropDownItems.Add(item);
            }
        }

        private void tsmiScale_Click(object sender, EventArgs e)
        {
            SelectEditorMode(sender);
            drawControl.ScaleSelected();
        }

        private void tsmiRotate_Click(object sender, EventArgs e)
        {
            SelectEditorMode(sender);
            drawControl.RotateSelected();
        }

        private void tsbExplode_Click(object sender, EventArgs e)
        {
            if (drawControl.EditorMode == EditorMode.Selection &&
                drawControl.SelectionController.Selection.All(x => x.Geometry is IExplodeGeometry))
            {
                drawControl.ExplodeSelected();
            }
        }

        private void tcmiBuildCircR_Click(object sender, EventArgs e)
        {
            SwitchOffButtons();
            drawControl.SetMode(EditorMode.BuildCircR);
            tsbCircle.Image = ((ToolStripMenuItem)sender).Image;
            tsbCircle.Tag = EditorMode.BuildCircR;
            tsbCircle.Checked = true;
        }

        private void tsmiBuildCircD_Click(object sender, EventArgs e)
        {
            SwitchOffButtons();
            drawControl.SetMode(EditorMode.BuildCircD);
            tsbCircle.Image = ((ToolStripMenuItem)sender).Image;
            tsbCircle.Tag = EditorMode.BuildCircD;
            tsbCircle.Checked = true;
        }

        private void tsmiBuildCircTwoPoints_Click(object sender, EventArgs e)
        {
            SwitchOffButtons();
            drawControl.SetMode(EditorMode.BuildCircTwoPoints);
            tsbCircle.Image = ((ToolStripMenuItem)sender).Image;
            tsbCircle.Tag = EditorMode.BuildCircTwoPoints;
            tsbCircle.Checked = true;
        }

        private void tsmiBuildCircThreePoints_Click(object sender, EventArgs e)
        {
            SwitchOffButtons();
            drawControl.SetMode(EditorMode.BuildCircThreePoints);
            tsbCircle.Image = ((ToolStripMenuItem)sender).Image;
            tsbCircle.Tag = EditorMode.BuildCircThreePoints;
            tsbCircle.Checked = true;
        }

        private void tsmiBuildCircTwoTangentsR_Click(object sender, EventArgs e)
        {
            SwitchOffButtons();
            drawControl.SetMode(EditorMode.BuildCircR2Tangets);
            tsbCircle.Image = ((ToolStripMenuItem)sender).Image;
            tsbCircle.Tag = EditorMode.BuildCircR2Tangets;
            tsbCircle.Checked = true;
        }

        private void tsmiBuildCircThreeTangents_Click(object sender, EventArgs e)
        {
            SwitchOffButtons();
            drawControl.SetMode(EditorMode.BuildCirc3Tangets);
            tsbCircle.Image = ((ToolStripMenuItem)sender).Image;
            tsbCircle.Tag = EditorMode.BuildCirc3Tangets;
            tsbCircle.Checked = true;
        }
    }
}
