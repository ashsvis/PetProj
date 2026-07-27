namespace PetProj.Dialogs
{
    partial class DrawingModesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnShowTuningDialog = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShowHelpWindow = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbObjectBindingMode = new System.Windows.Forms.CheckBox();
            this.gbBindingModes = new System.Windows.Forms.GroupBox();
            this.cbObjectTracking = new System.Windows.Forms.CheckBox();
            this.cbBindingToEndPoint = new System.Windows.Forms.CheckBox();
            this.cbBindingToMiddle = new System.Windows.Forms.CheckBox();
            this.cbBindingToCenter = new System.Windows.Forms.CheckBox();
            this.cbBindingToNormal = new System.Windows.Forms.CheckBox();
            this.btnSelectAllBindings = new System.Windows.Forms.Button();
            this.btnClearAllBindings = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.gbBindingModes.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(820, 408);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(814, 366);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.checkBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(806, 340);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Шаг и сетка";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gbBindingModes);
            this.tabPage2.Controls.Add(this.cbObjectTracking);
            this.tabPage2.Controls.Add(this.cbObjectBindingMode);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(806, 340);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Объектная привязка";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnShowTuningDialog
            // 
            this.btnShowTuningDialog.AutoSize = true;
            this.btnShowTuningDialog.Enabled = false;
            this.btnShowTuningDialog.Location = new System.Drawing.Point(3, 3);
            this.btnShowTuningDialog.Name = "btnShowTuningDialog";
            this.btnShowTuningDialog.Size = new System.Drawing.Size(83, 24);
            this.btnShowTuningDialog.TabIndex = 1;
            this.btnShowTuningDialog.Text = "Настройка...";
            this.btnShowTuningDialog.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnOk);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnShowHelpWindow);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(574, 375);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(243, 30);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(3, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 24);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "ОК";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(84, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnShowHelpWindow
            // 
            this.btnShowHelpWindow.Enabled = false;
            this.btnShowHelpWindow.Location = new System.Drawing.Point(165, 3);
            this.btnShowHelpWindow.Name = "btnShowHelpWindow";
            this.btnShowHelpWindow.Size = new System.Drawing.Size(75, 24);
            this.btnShowHelpWindow.TabIndex = 1;
            this.btnShowHelpWindow.Text = "Справка";
            this.btnShowHelpWindow.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.btnShowTuningDialog);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 375);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(89, 30);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // cbObjectBindingMode
            // 
            this.cbObjectBindingMode.AutoSize = true;
            this.cbObjectBindingMode.Location = new System.Drawing.Point(6, 6);
            this.cbObjectBindingMode.Name = "cbObjectBindingMode";
            this.cbObjectBindingMode.Size = new System.Drawing.Size(159, 17);
            this.cbObjectBindingMode.TabIndex = 0;
            this.cbObjectBindingMode.Text = "Объектная привязка (F3)";
            this.cbObjectBindingMode.UseVisualStyleBackColor = true;
            // 
            // gbBindingModes
            // 
            this.gbBindingModes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBindingModes.Controls.Add(this.btnClearAllBindings);
            this.gbBindingModes.Controls.Add(this.btnSelectAllBindings);
            this.gbBindingModes.Controls.Add(this.cbBindingToNormal);
            this.gbBindingModes.Controls.Add(this.cbBindingToCenter);
            this.gbBindingModes.Controls.Add(this.cbBindingToMiddle);
            this.gbBindingModes.Controls.Add(this.cbBindingToEndPoint);
            this.gbBindingModes.Location = new System.Drawing.Point(6, 29);
            this.gbBindingModes.Name = "gbBindingModes";
            this.gbBindingModes.Size = new System.Drawing.Size(794, 275);
            this.gbBindingModes.TabIndex = 1;
            this.gbBindingModes.TabStop = false;
            this.gbBindingModes.Text = "Режимы объектной привязки";
            // 
            // cbObjectTracking
            // 
            this.cbObjectTracking.AutoSize = true;
            this.cbObjectTracking.Location = new System.Drawing.Point(411, 6);
            this.cbObjectTracking.Name = "cbObjectTracking";
            this.cbObjectTracking.Size = new System.Drawing.Size(193, 17);
            this.cbObjectTracking.TabIndex = 0;
            this.cbObjectTracking.Text = "Объектное отслеживание (F11)";
            this.cbObjectTracking.UseVisualStyleBackColor = true;
            this.cbObjectTracking.Visible = false;
            // 
            // cbBindingToEndPoint
            // 
            this.cbBindingToEndPoint.AutoSize = true;
            this.cbBindingToEndPoint.Location = new System.Drawing.Point(31, 21);
            this.cbBindingToEndPoint.Name = "cbBindingToEndPoint";
            this.cbBindingToEndPoint.Size = new System.Drawing.Size(77, 17);
            this.cbBindingToEndPoint.TabIndex = 0;
            this.cbBindingToEndPoint.Text = "Конточка";
            this.cbBindingToEndPoint.UseVisualStyleBackColor = true;
            // 
            // cbBindingToMiddle
            // 
            this.cbBindingToMiddle.AutoSize = true;
            this.cbBindingToMiddle.Location = new System.Drawing.Point(31, 44);
            this.cbBindingToMiddle.Name = "cbBindingToMiddle";
            this.cbBindingToMiddle.Size = new System.Drawing.Size(78, 17);
            this.cbBindingToMiddle.TabIndex = 0;
            this.cbBindingToMiddle.Text = "Середина";
            this.cbBindingToMiddle.UseVisualStyleBackColor = true;
            // 
            // cbBindingToCenter
            // 
            this.cbBindingToCenter.AutoSize = true;
            this.cbBindingToCenter.Location = new System.Drawing.Point(31, 67);
            this.cbBindingToCenter.Name = "cbBindingToCenter";
            this.cbBindingToCenter.Size = new System.Drawing.Size(59, 17);
            this.cbBindingToCenter.TabIndex = 0;
            this.cbBindingToCenter.Text = "Центр";
            this.cbBindingToCenter.UseVisualStyleBackColor = true;
            // 
            // cbBindingToNormal
            // 
            this.cbBindingToNormal.AutoSize = true;
            this.cbBindingToNormal.Location = new System.Drawing.Point(31, 90);
            this.cbBindingToNormal.Name = "cbBindingToNormal";
            this.cbBindingToNormal.Size = new System.Drawing.Size(74, 17);
            this.cbBindingToNormal.TabIndex = 0;
            this.cbBindingToNormal.Text = "Нормаль";
            this.cbBindingToNormal.UseVisualStyleBackColor = true;
            // 
            // btnSelectAllBindings
            // 
            this.btnSelectAllBindings.AutoSize = true;
            this.btnSelectAllBindings.Location = new System.Drawing.Point(694, 21);
            this.btnSelectAllBindings.Name = "btnSelectAllBindings";
            this.btnSelectAllBindings.Size = new System.Drawing.Size(88, 24);
            this.btnSelectAllBindings.TabIndex = 2;
            this.btnSelectAllBindings.Text = "Выбрать все";
            this.btnSelectAllBindings.UseVisualStyleBackColor = true;
            this.btnSelectAllBindings.Click += new System.EventHandler(this.btnSelectAllBindings_Click);
            // 
            // btnClearAllBindings
            // 
            this.btnClearAllBindings.AutoSize = true;
            this.btnClearAllBindings.Location = new System.Drawing.Point(694, 51);
            this.btnClearAllBindings.Name = "btnClearAllBindings";
            this.btnClearAllBindings.Size = new System.Drawing.Size(88, 24);
            this.btnClearAllBindings.TabIndex = 2;
            this.btnClearAllBindings.Text = "Очистить все";
            this.btnClearAllBindings.UseVisualStyleBackColor = true;
            this.btnClearAllBindings.Click += new System.EventHandler(this.btnClearAllBindings_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(411, 6);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(77, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Сетка (F7)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(6, 6);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(98, 17);
            this.checkBox3.TabIndex = 0;
            this.checkBox3.Text = "Привязка (F9)";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Location = new System.Drawing.Point(6, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(397, 109);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Шаг привязки";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDown1);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(409, 29);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(391, 109);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Шаг сетки";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Шаг привязки по X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Шаг привязки по Y:";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(11, 78);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(162, 17);
            this.checkBox4.TabIndex = 2;
            this.checkBox4.Text = "Равный шаг по осям X и Y";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(125, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(70, 22);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "10";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(124, 47);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(71, 22);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Шаг сетки по X:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Шаг сетки по Y:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(146, 21);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(70, 22);
            this.textBox3.TabIndex = 0;
            this.textBox3.Text = "10";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(145, 48);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(71, 22);
            this.textBox4.TabIndex = 1;
            this.textBox4.Text = "10";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Основная линия через:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(145, 76);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(71, 22);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // DrawingModesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 408);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DrawingModesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Режимы рисования";
            this.Load += new System.EventHandler(this.DrawingModesForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.gbBindingModes.ResumeLayout(false);
            this.gbBindingModes.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnShowTuningDialog;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShowHelpWindow;
        private System.Windows.Forms.CheckBox cbObjectBindingMode;
        private System.Windows.Forms.GroupBox gbBindingModes;
        private System.Windows.Forms.CheckBox cbObjectTracking;
        private System.Windows.Forms.CheckBox cbBindingToNormal;
        private System.Windows.Forms.CheckBox cbBindingToCenter;
        private System.Windows.Forms.CheckBox cbBindingToMiddle;
        private System.Windows.Forms.CheckBox cbBindingToEndPoint;
        private System.Windows.Forms.Button btnClearAllBindings;
        private System.Windows.Forms.Button btnSelectAllBindings;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}