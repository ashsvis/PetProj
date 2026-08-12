namespace PetCAD.Controls
{
    partial class BorderStyleEditor
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbVisible = new System.Windows.Forms.CheckBox();
            this.nudWidth = new System.Windows.Forms.TextBox();
            this.cbPattern = new System.Windows.Forms.ComboBox();
            this.lbPattern = new System.Windows.Forms.Label();
            this.lbWidth = new System.Windows.Forms.Label();
            this.lbOpacity = new System.Windows.Forms.Label();
            this.nudOpacity = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbColor = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelCaption = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbVisible
            // 
            this.cbVisible.AutoSize = true;
            this.cbVisible.Location = new System.Drawing.Point(105, 34);
            this.cbVisible.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new System.Drawing.Size(15, 14);
            this.cbVisible.TabIndex = 0;
            this.cbVisible.UseVisualStyleBackColor = true;
            this.cbVisible.CheckedChanged += new System.EventHandler(this.cbVisible_CheckedChanged);
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(103, 102);
            this.nudWidth.Margin = new System.Windows.Forms.Padding(1);
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(64, 23);
            this.nudWidth.TabIndex = 5;
            this.nudWidth.Text = "0";
            this.nudWidth.Validated += new System.EventHandler(this.nudWidth_Validated);
            // 
            // cbPattern
            // 
            this.cbPattern.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPattern.FormattingEnabled = true;
            this.cbPattern.Location = new System.Drawing.Point(103, 128);
            this.cbPattern.Margin = new System.Windows.Forms.Padding(1);
            this.cbPattern.Name = "cbPattern";
            this.cbPattern.Size = new System.Drawing.Size(63, 24);
            this.cbPattern.TabIndex = 7;
            this.cbPattern.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbPattern_DrawItem);
            this.cbPattern.SelectionChangeCommitted += new System.EventHandler(this.cbPattern_SelectionChangeCommitted);
            // 
            // lbPattern
            // 
            this.lbPattern.AutoSize = true;
            this.lbPattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPattern.Location = new System.Drawing.Point(1, 129);
            this.lbPattern.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbPattern.Name = "lbPattern";
            this.lbPattern.Size = new System.Drawing.Size(100, 24);
            this.lbPattern.TabIndex = 6;
            this.lbPattern.Text = "Стиль линии";
            this.lbPattern.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbWidth
            // 
            this.lbWidth.AutoSize = true;
            this.lbWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbWidth.Location = new System.Drawing.Point(1, 103);
            this.lbWidth.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbWidth.Name = "lbWidth";
            this.lbWidth.Size = new System.Drawing.Size(100, 23);
            this.lbWidth.TabIndex = 4;
            this.lbWidth.Text = "Толщина";
            this.lbWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbOpacity
            // 
            this.lbOpacity.AutoSize = true;
            this.lbOpacity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbOpacity.Location = new System.Drawing.Point(1, 77);
            this.lbOpacity.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbOpacity.Name = "lbOpacity";
            this.lbOpacity.Size = new System.Drawing.Size(100, 23);
            this.lbOpacity.TabIndex = 2;
            this.lbOpacity.Text = "Прозрачность";
            this.lbOpacity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudOpacity
            // 
            this.nudOpacity.Location = new System.Drawing.Point(103, 76);
            this.nudOpacity.Margin = new System.Windows.Forms.Padding(1);
            this.nudOpacity.Name = "nudOpacity";
            this.nudOpacity.Size = new System.Drawing.Size(63, 23);
            this.nudOpacity.TabIndex = 3;
            this.nudOpacity.Text = "255";
            this.nudOpacity.Validated += new System.EventHandler(this.nudOpacity_Validated);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cbVisible, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbColor, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbPattern, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.nudOpacity, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.nudWidth, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbOpacity, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbPattern, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbWidth, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelCaption, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 154);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // lbColor
            // 
            this.lbColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbColor.Image = global::PetCAD.Properties.Resources.transparent4;
            this.lbColor.Location = new System.Drawing.Point(103, 55);
            this.lbColor.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.lbColor.Name = "lbColor";
            this.lbColor.Size = new System.Drawing.Size(64, 16);
            this.lbColor.TabIndex = 1;
            this.lbColor.Click += new System.EventHandler(this.lbColor_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 54);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Цвет";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(1, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Показывать";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCaption
            // 
            this.labelCaption.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.SetColumnSpan(this.labelCaption, 2);
            this.labelCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCaption.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelCaption.Location = new System.Drawing.Point(1, 1);
            this.labelCaption.Margin = new System.Windows.Forms.Padding(0);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Padding = new System.Windows.Forms.Padding(1);
            this.labelCaption.Size = new System.Drawing.Size(198, 26);
            this.labelCaption.TabIndex = 8;
            this.labelCaption.Text = "Контур";
            this.labelCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BorderStyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.Name = "BorderStyleEditor";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Size = new System.Drawing.Size(200, 154);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbColor;
        private System.Windows.Forms.CheckBox cbVisible;
        private System.Windows.Forms.TextBox nudWidth;
        private System.Windows.Forms.ComboBox cbPattern;
        private System.Windows.Forms.Label lbWidth;
        private System.Windows.Forms.Label lbPattern;
        private System.Windows.Forms.Label lbOpacity;
        private System.Windows.Forms.TextBox nudOpacity;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelCaption;
    }
}
