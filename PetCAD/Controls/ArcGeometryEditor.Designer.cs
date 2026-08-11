namespace PetCAD.Controls
{
    partial class ArcGeometryEditor
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
            this.lbCenterY = new System.Windows.Forms.Label();
            this.lbCenterX = new System.Windows.Forms.Label();
            this.lbStartY = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbStartX = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbEndX = new System.Windows.Forms.Label();
            this.lbEndY = new System.Windows.Forms.Label();
            this.lbRadius = new System.Windows.Forms.Label();
            this.lbStartAngle = new System.Windows.Forms.Label();
            this.tbStartX = new System.Windows.Forms.TextBox();
            this.tbStartY = new System.Windows.Forms.TextBox();
            this.tbCenterX = new System.Windows.Forms.TextBox();
            this.tbCenterY = new System.Windows.Forms.TextBox();
            this.tbEndX = new System.Windows.Forms.TextBox();
            this.tbEndY = new System.Windows.Forms.TextBox();
            this.tbRadius = new System.Windows.Forms.TextBox();
            this.tbStartAngle = new System.Windows.Forms.TextBox();
            this.errorProv = new System.Windows.Forms.ErrorProvider(this.components);
            this.lbEndAngle = new System.Windows.Forms.Label();
            this.tbEndAngle = new System.Windows.Forms.TextBox();
            this.lbSweepAngle = new System.Windows.Forms.Label();
            this.tbSweepAngle = new System.Windows.Forms.TextBox();
            this.lbArcLength = new System.Windows.Forms.Label();
            this.tbArcLength = new System.Windows.Forms.TextBox();
            this.lbSegmentSquare = new System.Windows.Forms.Label();
            this.tbSegmentSquare = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProv)).BeginInit();
            this.SuspendLayout();
            // 
            // lbCenterY
            // 
            this.lbCenterY.AutoSize = true;
            this.lbCenterY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCenterY.Location = new System.Drawing.Point(1, 107);
            this.lbCenterY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbCenterY.Name = "lbCenterY";
            this.lbCenterY.Size = new System.Drawing.Size(99, 23);
            this.lbCenterY.TabIndex = 6;
            this.lbCenterY.Text = "Центр Y";
            this.lbCenterY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCenterX
            // 
            this.lbCenterX.AutoSize = true;
            this.lbCenterX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCenterX.Location = new System.Drawing.Point(1, 81);
            this.lbCenterX.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbCenterX.Name = "lbCenterX";
            this.lbCenterX.Size = new System.Drawing.Size(99, 23);
            this.lbCenterX.TabIndex = 4;
            this.lbCenterX.Text = "Центр X";
            this.lbCenterX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbStartY
            // 
            this.lbStartY.AutoSize = true;
            this.lbStartY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbStartY.Location = new System.Drawing.Point(1, 55);
            this.lbStartY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbStartY.Name = "lbStartY";
            this.lbStartY.Size = new System.Drawing.Size(99, 23);
            this.lbStartY.TabIndex = 2;
            this.lbStartY.Text = "Начало Y";
            this.lbStartY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbSegmentSquare, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.lbArcLength, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.lbSweepAngle, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.lbEndAngle, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lbCenterY, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbStartY, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbCenterX, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbStartX, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbEndX, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbEndY, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbRadius, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lbStartAngle, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbStartX, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbStartY, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbCenterX, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbCenterY, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbEndX, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbEndY, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbRadius, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbStartAngle, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.tbEndAngle, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbSweepAngle, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.tbArcLength, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.tbSegmentSquare, 1, 12);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 339);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // lbStartX
            // 
            this.lbStartX.AutoSize = true;
            this.lbStartX.Location = new System.Drawing.Point(1, 29);
            this.lbStartX.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbStartX.Name = "lbStartX";
            this.lbStartX.Size = new System.Drawing.Size(59, 15);
            this.lbStartX.TabIndex = 2;
            this.lbStartX.Text = "Начало X";
            this.lbStartX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // lbEndX
            // 
            this.lbEndX.AutoSize = true;
            this.lbEndX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbEndX.Location = new System.Drawing.Point(1, 133);
            this.lbEndX.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbEndX.Name = "lbEndX";
            this.lbEndX.Size = new System.Drawing.Size(99, 23);
            this.lbEndX.TabIndex = 9;
            this.lbEndX.Text = "Конец X";
            this.lbEndX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbEndY
            // 
            this.lbEndY.AutoSize = true;
            this.lbEndY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbEndY.Location = new System.Drawing.Point(1, 159);
            this.lbEndY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbEndY.Name = "lbEndY";
            this.lbEndY.Size = new System.Drawing.Size(99, 23);
            this.lbEndY.TabIndex = 9;
            this.lbEndY.Text = "Конец Y";
            this.lbEndY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRadius
            // 
            this.lbRadius.AutoSize = true;
            this.lbRadius.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRadius.Location = new System.Drawing.Point(1, 185);
            this.lbRadius.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbRadius.Name = "lbRadius";
            this.lbRadius.Size = new System.Drawing.Size(99, 23);
            this.lbRadius.TabIndex = 9;
            this.lbRadius.Text = "Радиус";
            this.lbRadius.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbStartAngle
            // 
            this.lbStartAngle.AutoSize = true;
            this.lbStartAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbStartAngle.Location = new System.Drawing.Point(1, 211);
            this.lbStartAngle.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbStartAngle.Name = "lbStartAngle";
            this.lbStartAngle.Size = new System.Drawing.Size(99, 23);
            this.lbStartAngle.TabIndex = 9;
            this.lbStartAngle.Text = "Начальный угол";
            this.lbStartAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbStartX
            // 
            this.tbStartX.Enabled = false;
            this.tbStartX.Location = new System.Drawing.Point(102, 28);
            this.tbStartX.Margin = new System.Windows.Forms.Padding(1);
            this.tbStartX.Name = "tbStartX";
            this.tbStartX.Size = new System.Drawing.Size(96, 23);
            this.tbStartX.TabIndex = 10;
            // 
            // tbStartY
            // 
            this.tbStartY.Enabled = false;
            this.tbStartY.Location = new System.Drawing.Point(102, 54);
            this.tbStartY.Margin = new System.Windows.Forms.Padding(1);
            this.tbStartY.Name = "tbStartY";
            this.tbStartY.Size = new System.Drawing.Size(95, 23);
            this.tbStartY.TabIndex = 10;
            // 
            // tbCenterX
            // 
            this.tbCenterX.Location = new System.Drawing.Point(102, 80);
            this.tbCenterX.Margin = new System.Windows.Forms.Padding(1);
            this.tbCenterX.Name = "tbCenterX";
            this.tbCenterX.Size = new System.Drawing.Size(95, 23);
            this.tbCenterX.TabIndex = 10;
            this.tbCenterX.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbCenterY
            // 
            this.tbCenterY.Location = new System.Drawing.Point(102, 106);
            this.tbCenterY.Margin = new System.Windows.Forms.Padding(1);
            this.tbCenterY.Name = "tbCenterY";
            this.tbCenterY.Size = new System.Drawing.Size(95, 23);
            this.tbCenterY.TabIndex = 10;
            this.tbCenterY.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbEndX
            // 
            this.tbEndX.Enabled = false;
            this.tbEndX.Location = new System.Drawing.Point(102, 132);
            this.tbEndX.Margin = new System.Windows.Forms.Padding(1);
            this.tbEndX.Name = "tbEndX";
            this.tbEndX.Size = new System.Drawing.Size(95, 23);
            this.tbEndX.TabIndex = 10;
            // 
            // tbEndY
            // 
            this.tbEndY.Enabled = false;
            this.tbEndY.Location = new System.Drawing.Point(102, 158);
            this.tbEndY.Margin = new System.Windows.Forms.Padding(1);
            this.tbEndY.Name = "tbEndY";
            this.tbEndY.Size = new System.Drawing.Size(95, 23);
            this.tbEndY.TabIndex = 10;
            // 
            // tbRadius
            // 
            this.tbRadius.Location = new System.Drawing.Point(102, 184);
            this.tbRadius.Margin = new System.Windows.Forms.Padding(1);
            this.tbRadius.Name = "tbRadius";
            this.tbRadius.Size = new System.Drawing.Size(95, 23);
            this.tbRadius.TabIndex = 10;
            this.tbRadius.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbStartAngle
            // 
            this.tbStartAngle.Location = new System.Drawing.Point(102, 210);
            this.tbStartAngle.Margin = new System.Windows.Forms.Padding(1);
            this.tbStartAngle.Name = "tbStartAngle";
            this.tbStartAngle.Size = new System.Drawing.Size(95, 23);
            this.tbStartAngle.TabIndex = 10;
            this.tbStartAngle.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // errorProv
            // 
            this.errorProv.ContainerControl = this;
            // 
            // lbEndAngle
            // 
            this.lbEndAngle.AutoSize = true;
            this.lbEndAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbEndAngle.Location = new System.Drawing.Point(1, 237);
            this.lbEndAngle.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbEndAngle.Name = "lbEndAngle";
            this.lbEndAngle.Size = new System.Drawing.Size(99, 23);
            this.lbEndAngle.TabIndex = 11;
            this.lbEndAngle.Text = "Конечный угол";
            this.lbEndAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbEndAngle
            // 
            this.tbEndAngle.Enabled = false;
            this.tbEndAngle.Location = new System.Drawing.Point(102, 236);
            this.tbEndAngle.Margin = new System.Windows.Forms.Padding(1);
            this.tbEndAngle.Name = "tbEndAngle";
            this.tbEndAngle.Size = new System.Drawing.Size(95, 23);
            this.tbEndAngle.TabIndex = 10;
            // 
            // lbSweepAngle
            // 
            this.lbSweepAngle.AutoSize = true;
            this.lbSweepAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSweepAngle.Location = new System.Drawing.Point(1, 263);
            this.lbSweepAngle.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbSweepAngle.Name = "lbSweepAngle";
            this.lbSweepAngle.Size = new System.Drawing.Size(99, 23);
            this.lbSweepAngle.TabIndex = 12;
            this.lbSweepAngle.Text = "Полный угол";
            this.lbSweepAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbSweepAngle
            // 
            this.tbSweepAngle.Location = new System.Drawing.Point(102, 262);
            this.tbSweepAngle.Margin = new System.Windows.Forms.Padding(1);
            this.tbSweepAngle.Name = "tbSweepAngle";
            this.tbSweepAngle.Size = new System.Drawing.Size(95, 23);
            this.tbSweepAngle.TabIndex = 10;
            this.tbSweepAngle.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // lbArcLength
            // 
            this.lbArcLength.AutoSize = true;
            this.lbArcLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbArcLength.Location = new System.Drawing.Point(1, 289);
            this.lbArcLength.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbArcLength.Name = "lbArcLength";
            this.lbArcLength.Size = new System.Drawing.Size(99, 23);
            this.lbArcLength.TabIndex = 13;
            this.lbArcLength.Text = "Длина дуги";
            this.lbArcLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbArcLength
            // 
            this.tbArcLength.Enabled = false;
            this.tbArcLength.Location = new System.Drawing.Point(102, 288);
            this.tbArcLength.Margin = new System.Windows.Forms.Padding(1);
            this.tbArcLength.Name = "tbArcLength";
            this.tbArcLength.Size = new System.Drawing.Size(95, 23);
            this.tbArcLength.TabIndex = 10;
            // 
            // lbSegmentSquare
            // 
            this.lbSegmentSquare.AutoSize = true;
            this.lbSegmentSquare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSegmentSquare.Location = new System.Drawing.Point(1, 315);
            this.lbSegmentSquare.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbSegmentSquare.Name = "lbSegmentSquare";
            this.lbSegmentSquare.Size = new System.Drawing.Size(99, 23);
            this.lbSegmentSquare.TabIndex = 14;
            this.lbSegmentSquare.Text = "Площадь";
            this.lbSegmentSquare.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbSegmentSquare
            // 
            this.tbSegmentSquare.Enabled = false;
            this.tbSegmentSquare.Location = new System.Drawing.Point(102, 314);
            this.tbSegmentSquare.Margin = new System.Windows.Forms.Padding(1);
            this.tbSegmentSquare.Name = "tbSegmentSquare";
            this.tbSegmentSquare.Size = new System.Drawing.Size(95, 23);
            this.tbSegmentSquare.TabIndex = 10;
            // 
            // ArcGeometryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ArcGeometryEditor";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Size = new System.Drawing.Size(200, 339);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbCenterX;
        private System.Windows.Forms.Label lbCenterY;
        private System.Windows.Forms.Label lbStartY;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbStartX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbEndX;
        private System.Windows.Forms.Label lbEndY;
        private System.Windows.Forms.Label lbRadius;
        private System.Windows.Forms.Label lbStartAngle;
        private System.Windows.Forms.TextBox tbStartX;
        private System.Windows.Forms.TextBox tbStartY;
        private System.Windows.Forms.TextBox tbCenterX;
        private System.Windows.Forms.TextBox tbCenterY;
        private System.Windows.Forms.TextBox tbEndX;
        private System.Windows.Forms.TextBox tbEndY;
        private System.Windows.Forms.TextBox tbRadius;
        private System.Windows.Forms.TextBox tbStartAngle;
        private System.Windows.Forms.ErrorProvider errorProv;
        private System.Windows.Forms.Label lbSegmentSquare;
        private System.Windows.Forms.Label lbArcLength;
        private System.Windows.Forms.Label lbSweepAngle;
        private System.Windows.Forms.Label lbEndAngle;
        private System.Windows.Forms.TextBox tbEndAngle;
        private System.Windows.Forms.TextBox tbSweepAngle;
        private System.Windows.Forms.TextBox tbArcLength;
        private System.Windows.Forms.TextBox tbSegmentSquare;
    }
}
