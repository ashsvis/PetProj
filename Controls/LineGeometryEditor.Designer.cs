namespace PetProj.Controls
{
    partial class LineGeometryEditor
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
            this.components = new System.ComponentModel.Container();
            this.lbPattern = new System.Windows.Forms.Label();
            this.lbWidth = new System.Windows.Forms.Label();
            this.lbOpacity = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbStartX = new System.Windows.Forms.TextBox();
            this.tbStartY = new System.Windows.Forms.TextBox();
            this.tbEndX = new System.Windows.Forms.TextBox();
            this.tbEndY = new System.Windows.Forms.TextBox();
            this.tbDeltaX = new System.Windows.Forms.TextBox();
            this.tbDeltaY = new System.Windows.Forms.TextBox();
            this.tbLength = new System.Windows.Forms.TextBox();
            this.tbAngle = new System.Windows.Forms.TextBox();
            this.errorProv = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProv)).BeginInit();
            this.SuspendLayout();
            // 
            // lbPattern
            // 
            this.lbPattern.AutoSize = true;
            this.lbPattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPattern.Location = new System.Drawing.Point(1, 107);
            this.lbPattern.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbPattern.Name = "lbPattern";
            this.lbPattern.Size = new System.Drawing.Size(99, 23);
            this.lbPattern.TabIndex = 6;
            this.lbPattern.Text = "Конец Y";
            this.lbPattern.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbWidth
            // 
            this.lbWidth.AutoSize = true;
            this.lbWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbWidth.Location = new System.Drawing.Point(1, 81);
            this.lbWidth.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbWidth.Name = "lbWidth";
            this.lbWidth.Size = new System.Drawing.Size(99, 23);
            this.lbWidth.TabIndex = 4;
            this.lbWidth.Text = "Конец X";
            this.lbWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbOpacity
            // 
            this.lbOpacity.AutoSize = true;
            this.lbOpacity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbOpacity.Location = new System.Drawing.Point(1, 55);
            this.lbOpacity.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbOpacity.Name = "lbOpacity";
            this.lbOpacity.Size = new System.Drawing.Size(99, 23);
            this.lbOpacity.TabIndex = 2;
            this.lbOpacity.Text = "Начало Y";
            this.lbOpacity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbPattern, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbOpacity, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbWidth, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbStartX, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbStartY, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbEndX, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbEndY, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbDeltaX, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbDeltaY, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbLength, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbAngle, 1, 8);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 235);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Начало X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 2);
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(1, 1);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(1);
            this.label3.Size = new System.Drawing.Size(198, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "Геометрия";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(1, 133);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Смещение X";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(1, 159);
            this.label4.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 23);
            this.label4.TabIndex = 9;
            this.label4.Text = "Смещение Y";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(1, 185);
            this.label5.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Длина";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(1, 211);
            this.label6.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 23);
            this.label6.TabIndex = 9;
            this.label6.Text = "Угол в градусах";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbStartX
            // 
            this.tbStartX.Location = new System.Drawing.Point(102, 28);
            this.tbStartX.Margin = new System.Windows.Forms.Padding(1);
            this.tbStartX.Name = "tbStartX";
            this.tbStartX.Size = new System.Drawing.Size(96, 23);
            this.tbStartX.TabIndex = 10;
            this.tbStartX.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbStartY
            // 
            this.tbStartY.Location = new System.Drawing.Point(102, 54);
            this.tbStartY.Margin = new System.Windows.Forms.Padding(1);
            this.tbStartY.Name = "tbStartY";
            this.tbStartY.Size = new System.Drawing.Size(95, 23);
            this.tbStartY.TabIndex = 10;
            this.tbStartY.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbEndX
            // 
            this.tbEndX.Location = new System.Drawing.Point(102, 80);
            this.tbEndX.Margin = new System.Windows.Forms.Padding(1);
            this.tbEndX.Name = "tbEndX";
            this.tbEndX.Size = new System.Drawing.Size(95, 23);
            this.tbEndX.TabIndex = 10;
            this.tbEndX.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbEndY
            // 
            this.tbEndY.Location = new System.Drawing.Point(102, 106);
            this.tbEndY.Margin = new System.Windows.Forms.Padding(1);
            this.tbEndY.Name = "tbEndY";
            this.tbEndY.Size = new System.Drawing.Size(95, 23);
            this.tbEndY.TabIndex = 10;
            this.tbEndY.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbDeltaX
            // 
            this.tbDeltaX.Enabled = false;
            this.tbDeltaX.Location = new System.Drawing.Point(102, 132);
            this.tbDeltaX.Margin = new System.Windows.Forms.Padding(1);
            this.tbDeltaX.Name = "tbDeltaX";
            this.tbDeltaX.Size = new System.Drawing.Size(95, 23);
            this.tbDeltaX.TabIndex = 10;
            // 
            // tbDeltaY
            // 
            this.tbDeltaY.Enabled = false;
            this.tbDeltaY.Location = new System.Drawing.Point(102, 158);
            this.tbDeltaY.Margin = new System.Windows.Forms.Padding(1);
            this.tbDeltaY.Name = "tbDeltaY";
            this.tbDeltaY.Size = new System.Drawing.Size(95, 23);
            this.tbDeltaY.TabIndex = 10;
            // 
            // tbLength
            // 
            this.tbLength.Enabled = false;
            this.tbLength.Location = new System.Drawing.Point(102, 184);
            this.tbLength.Margin = new System.Windows.Forms.Padding(1);
            this.tbLength.Name = "tbLength";
            this.tbLength.Size = new System.Drawing.Size(95, 23);
            this.tbLength.TabIndex = 10;
            // 
            // tbAngle
            // 
            this.tbAngle.Enabled = false;
            this.tbAngle.Location = new System.Drawing.Point(102, 210);
            this.tbAngle.Margin = new System.Windows.Forms.Padding(1);
            this.tbAngle.Name = "tbAngle";
            this.tbAngle.Size = new System.Drawing.Size(95, 23);
            this.tbAngle.TabIndex = 10;
            // 
            // errorProv
            // 
            this.errorProv.ContainerControl = this;
            // 
            // LineGeometryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LineGeometryEditor";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Size = new System.Drawing.Size(200, 235);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbWidth;
        private System.Windows.Forms.Label lbPattern;
        private System.Windows.Forms.Label lbOpacity;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbStartX;
        private System.Windows.Forms.TextBox tbStartY;
        private System.Windows.Forms.TextBox tbEndX;
        private System.Windows.Forms.TextBox tbEndY;
        private System.Windows.Forms.TextBox tbDeltaX;
        private System.Windows.Forms.TextBox tbDeltaY;
        private System.Windows.Forms.TextBox tbLength;
        private System.Windows.Forms.TextBox tbAngle;
        private System.Windows.Forms.ErrorProvider errorProv;
    }
}
