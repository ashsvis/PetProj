using PetProj.Common;
using System;
using System.Drawing;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

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
            placeHolder.Controls.Add(drawControl);
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
            switch (drawControl.EditorMode)
            {
                case EditorMode.BuildLines:
                    //if (!label1.Visible) label1.Visible = true;
                    //label1.Text = e.clickCount > 0 ? "Следующая точка " : "Первая точка ";
                    var pt = e.location;
                    //pt.Offset(5, -label1.Height / 2);
                    //label1.Location = pt;
                    if (!textBox1.Visible)
                    {
                        textBox1.Visible = true;
                        textBox1.Focus();
                        textBox1.SelectAll();
                    }
                    pt.Offset(5, 0);
                    if (e.clickCount == 0)
                        textBox1.Location = pt;
                    else if (e.clickCount == 1)
                    {
                        var pt1 = Point.Ceiling(drawControl.GetFirstMouseDownPosition());
                        var pt2 = e.location;
                        if (pt1 == pt2)
                        {
                            pt2.Offset(0, -25);
                            textBox1.Location = pt2;
                        }
                        else
                        {
                            float dx = pt2.X - pt1.X;
                            float dy = pt2.Y - pt1.Y;
                            float px = dy;
                            float py = -dx;
                            float length = (float)Math.Sqrt(px * px + py * py);
                            if (length > 0) // Отрезок не вырожден в точку
                            {
                                px /= length;
                                py /= length;
                                var mid = new Point((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
                                var shift = px > 0 ? 50 : -50;
                                var midpoint = Point.Ceiling(new PointF(mid.X - px * shift, mid.Y - py * shift));
                                midpoint.Offset(-textBox1.Width / 2, -textBox1.Height / 2);
                                textBox1.Location = Point.Ceiling(midpoint);
                            }
                        }
                    }
                    if (!textBox2.Visible) textBox2.Visible = true;
                    pt.Offset(textBox1.Width + 5, 0);
                    if (e.clickCount == 0)
                        textBox2.Location = pt;
                    else if (e.clickCount == 1)
                    {
                        var pt1 = Point.Ceiling(drawControl.GetFirstMouseDownPosition());
                        var pt2 = e.location;
                        if (pt1 == pt2)
                        {
                            pt2.Offset(0, 25);
                            textBox2.Location = pt2;
                        }
                        else
                        {
                            float dx = pt2.X - pt1.X;
                            float dy = pt2.Y - pt1.Y;
                            float length = (float)Math.Sqrt(dx * dx + dy * dy);
                            var plc = drawControl.PrepareMousePosition(e.location);
                            var angle = Math.Abs(MmsPoint.GetAngle(e.first, plc));
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
                                pt2.Offset(0, 30);
                                textBox2.Location = pt2;
                            }
                        }
                    }
                    // показываем значения координат в полях ввода
                    var ploc = drawControl.PrepareMousePosition(e.location);
                    var ptm = new MmsPoint(this, ploc);
                    textBox1.Text = e.clickCount == 0 ? ptm.X.ToString() : MmsPoint.GetLength(this, e.first, ploc).ToString();
                    textBox1.SelectAll();
                    textBox2.Text = e.clickCount == 0 ? ptm.Y.ToString() : MmsPoint.GetAngleString(e.first, ploc);
                    textBox2.SelectAll();
                    break;
                default:
                    //if (label1.Visible) label1.Visible = false;
                    if (textBox1.Visible) textBox1.Visible = false;
                    if (textBox2.Visible) textBox2.Visible = false;
                    break;
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
            //tsslStatus.Text = $"Выбрано объектов: {drawControl.SelectionCount}";
            if (drawControl is IUndoRedoSupport support)
            {
                tsbUndo.Enabled = tsmiUndo.Enabled = support.CanUndo();
                tsbRedo.Enabled = tsmiRedo.Enabled = support.CanRedo();
            }
            tsmiMove.Enabled = tsbMove.Enabled = drawControl.SelectionCount > 0;
            tsmiMoveCopy.Enabled = tsbMoveCopy.Enabled = drawControl.SelectionCount > 0;
            tsmiDelete.Enabled = tsbCopy.Enabled = tsmiCopy.Enabled = tsbCut.Enabled = tsmiCut.Enabled = 
                drawControl.EditorMode == EditorMode.Selection && drawControl.SelectionCount > 0;
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
    }
}
