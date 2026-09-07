namespace PetCAD.Dialogs
{
    partial class BlockDefinitionForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbBlockName = new System.Windows.Forms.ComboBox();
            this.cbOpenInEditor = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbOffsetY = new System.Windows.Forms.TextBox();
            this.tbOffsetX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPointOriginInScreen = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbRemoveSource = new System.Windows.Forms.RadioButton();
            this.rbConvertSource = new System.Windows.Forms.RadioButton();
            this.rbLeaveSource = new System.Windows.Forms.RadioButton();
            this.cbSelectObjectsInScreen = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbExplodeEnabled = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя:";
            // 
            // cbBlockName
            // 
            this.cbBlockName.FormattingEnabled = true;
            this.cbBlockName.Location = new System.Drawing.Point(15, 28);
            this.cbBlockName.Name = "cbBlockName";
            this.cbBlockName.Size = new System.Drawing.Size(284, 23);
            this.cbBlockName.TabIndex = 1;
            this.cbBlockName.TextChanged += new System.EventHandler(this.cbBlockName_TextChanged);
            // 
            // cbOpenInEditor
            // 
            this.cbOpenInEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOpenInEditor.AutoSize = true;
            this.cbOpenInEditor.Enabled = false;
            this.cbOpenInEditor.Location = new System.Drawing.Point(12, 275);
            this.cbOpenInEditor.Name = "cbOpenInEditor";
            this.cbOpenInEditor.Size = new System.Drawing.Size(184, 19);
            this.cbOpenInEditor.TabIndex = 2;
            this.cbOpenInEditor.Text = "Открыть в редакторе блоков";
            this.cbOpenInEditor.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(416, 271);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 24);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ввод";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(497, 271);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbOffsetY);
            this.groupBox1.Controls.Add(this.tbOffsetX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbPointOriginInScreen);
            this.groupBox1.Location = new System.Drawing.Point(15, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(181, 178);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Базовая точка";
            // 
            // tbOffsetY
            // 
            this.tbOffsetY.Enabled = false;
            this.tbOffsetY.Location = new System.Drawing.Point(31, 75);
            this.tbOffsetY.Name = "tbOffsetY";
            this.tbOffsetY.Size = new System.Drawing.Size(135, 23);
            this.tbOffsetY.TabIndex = 2;
            this.tbOffsetY.Text = "0";
            // 
            // tbOffsetX
            // 
            this.tbOffsetX.Enabled = false;
            this.tbOffsetX.Location = new System.Drawing.Point(31, 46);
            this.tbOffsetX.Name = "tbOffsetX";
            this.tbOffsetX.Size = new System.Drawing.Size(135, 23);
            this.tbOffsetX.TabIndex = 2;
            this.tbOffsetX.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Y:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "X:";
            // 
            // cbPointOriginInScreen
            // 
            this.cbPointOriginInScreen.AutoSize = true;
            this.cbPointOriginInScreen.Checked = true;
            this.cbPointOriginInScreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPointOriginInScreen.Enabled = false;
            this.cbPointOriginInScreen.Location = new System.Drawing.Point(7, 23);
            this.cbPointOriginInScreen.Name = "cbPointOriginInScreen";
            this.cbPointOriginInScreen.Size = new System.Drawing.Size(124, 19);
            this.cbPointOriginInScreen.TabIndex = 0;
            this.cbPointOriginInScreen.Text = "Указать на экране";
            this.cbPointOriginInScreen.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbRemoveSource);
            this.groupBox2.Controls.Add(this.rbConvertSource);
            this.groupBox2.Controls.Add(this.rbLeaveSource);
            this.groupBox2.Controls.Add(this.cbSelectObjectsInScreen);
            this.groupBox2.Location = new System.Drawing.Point(202, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(181, 178);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Объекты";
            // 
            // rbRemoveSource
            // 
            this.rbRemoveSource.AutoSize = true;
            this.rbRemoveSource.Location = new System.Drawing.Point(7, 125);
            this.rbRemoveSource.Name = "rbRemoveSource";
            this.rbRemoveSource.Size = new System.Drawing.Size(69, 19);
            this.rbRemoveSource.TabIndex = 1;
            this.rbRemoveSource.Text = "Удалить";
            this.rbRemoveSource.UseVisualStyleBackColor = true;
            // 
            // rbConvertSource
            // 
            this.rbConvertSource.AutoSize = true;
            this.rbConvertSource.Checked = true;
            this.rbConvertSource.Location = new System.Drawing.Point(7, 100);
            this.rbConvertSource.Name = "rbConvertSource";
            this.rbConvertSource.Size = new System.Drawing.Size(148, 19);
            this.rbConvertSource.TabIndex = 1;
            this.rbConvertSource.TabStop = true;
            this.rbConvertSource.Text = "Преобразовать в блок";
            this.rbConvertSource.UseVisualStyleBackColor = true;
            // 
            // rbLeaveSource
            // 
            this.rbLeaveSource.AutoSize = true;
            this.rbLeaveSource.Location = new System.Drawing.Point(7, 75);
            this.rbLeaveSource.Name = "rbLeaveSource";
            this.rbLeaveSource.Size = new System.Drawing.Size(75, 19);
            this.rbLeaveSource.TabIndex = 1;
            this.rbLeaveSource.Text = "Оставить";
            this.rbLeaveSource.UseVisualStyleBackColor = true;
            // 
            // cbSelectObjectsInScreen
            // 
            this.cbSelectObjectsInScreen.AutoSize = true;
            this.cbSelectObjectsInScreen.Checked = true;
            this.cbSelectObjectsInScreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSelectObjectsInScreen.Enabled = false;
            this.cbSelectObjectsInScreen.Location = new System.Drawing.Point(7, 23);
            this.cbSelectObjectsInScreen.Name = "cbSelectObjectsInScreen";
            this.cbSelectObjectsInScreen.Size = new System.Drawing.Size(124, 19);
            this.cbSelectObjectsInScreen.TabIndex = 0;
            this.cbSelectObjectsInScreen.Text = "Указать на экране";
            this.cbSelectObjectsInScreen.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbExplodeEnabled);
            this.groupBox3.Location = new System.Drawing.Point(389, 57);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(181, 178);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Поведение";
            // 
            // cbExplodeEnabled
            // 
            this.cbExplodeEnabled.AutoSize = true;
            this.cbExplodeEnabled.Checked = true;
            this.cbExplodeEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbExplodeEnabled.Location = new System.Drawing.Point(7, 23);
            this.cbExplodeEnabled.Name = "cbExplodeEnabled";
            this.cbExplodeEnabled.Size = new System.Drawing.Size(161, 19);
            this.cbExplodeEnabled.TabIndex = 0;
            this.cbExplodeEnabled.Text = "Разрешить расчленение";
            this.cbExplodeEnabled.UseVisualStyleBackColor = true;
            // 
            // BlockDefinitionForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 306);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cbOpenInEditor);
            this.Controls.Add(this.cbBlockName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlockDefinitionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Определение блока";
            this.Load += new System.EventHandler(this.BlockDefinitionForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbBlockName;
        private System.Windows.Forms.CheckBox cbOpenInEditor;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbPointOriginInScreen;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbSelectObjectsInScreen;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbExplodeEnabled;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbOffsetY;
        private System.Windows.Forms.TextBox tbOffsetX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbRemoveSource;
        private System.Windows.Forms.RadioButton rbConvertSource;
        private System.Windows.Forms.RadioButton rbLeaveSource;
    }
}