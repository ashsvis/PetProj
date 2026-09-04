namespace PetCAD.Controls
{
    partial class CircleGeometryEditor
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
            this.lbCenterX = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbSegmentSquare = new System.Windows.Forms.Label();
            this.lbArcLength = new System.Windows.Forms.Label();
            this.lbRadius = new System.Windows.Forms.Label();
            this.tbCenterX = new System.Windows.Forms.TextBox();
            this.tbRadius = new System.Windows.Forms.TextBox();
            this.tbArcLength = new System.Windows.Forms.TextBox();
            this.tbSegmentSquare = new System.Windows.Forms.TextBox();
            this.errorProv = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbCenterY = new System.Windows.Forms.TextBox();
            this.lbCenterY = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProv)).BeginInit();
            this.SuspendLayout();
            // 
            // lbCenterX
            // 
            this.lbCenterX.AutoSize = true;
            this.lbCenterX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCenterX.Location = new System.Drawing.Point(1, 29);
            this.lbCenterX.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbCenterX.Name = "lbCenterX";
            this.lbCenterX.Size = new System.Drawing.Size(114, 23);
            this.lbCenterX.TabIndex = 4;
            this.lbCenterX.Text = "Центр X";
            this.lbCenterX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbSegmentSquare, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbArcLength, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbCenterY, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbCenterX, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbRadius, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbCenterX, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbCenterY, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbRadius, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbArcLength, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbSegmentSquare, 1, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(267, 159);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // lbSegmentSquare
            // 
            this.lbSegmentSquare.AutoSize = true;
            this.lbSegmentSquare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSegmentSquare.Location = new System.Drawing.Point(1, 133);
            this.lbSegmentSquare.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbSegmentSquare.Name = "lbSegmentSquare";
            this.lbSegmentSquare.Size = new System.Drawing.Size(114, 23);
            this.lbSegmentSquare.TabIndex = 14;
            this.lbSegmentSquare.Text = "Площадь";
            this.lbSegmentSquare.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbArcLength
            // 
            this.lbArcLength.AutoEllipsis = true;
            this.lbArcLength.AutoSize = true;
            this.lbArcLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbArcLength.Location = new System.Drawing.Point(1, 107);
            this.lbArcLength.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbArcLength.Name = "lbArcLength";
            this.lbArcLength.Size = new System.Drawing.Size(114, 23);
            this.lbArcLength.TabIndex = 13;
            this.lbArcLength.Text = "Длина окружности";
            this.lbArcLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRadius
            // 
            this.lbRadius.AutoSize = true;
            this.lbRadius.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRadius.Location = new System.Drawing.Point(1, 81);
            this.lbRadius.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbRadius.Name = "lbRadius";
            this.lbRadius.Size = new System.Drawing.Size(114, 23);
            this.lbRadius.TabIndex = 9;
            this.lbRadius.Text = "Радиус";
            this.lbRadius.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbCenterX
            // 
            this.tbCenterX.Location = new System.Drawing.Point(117, 28);
            this.tbCenterX.Margin = new System.Windows.Forms.Padding(1);
            this.tbCenterX.Name = "tbCenterX";
            this.tbCenterX.Size = new System.Drawing.Size(95, 23);
            this.tbCenterX.TabIndex = 10;
            this.tbCenterX.Validated += new System.EventHandler(this.tbCenterX_Validated);
            // 
            // tbRadius
            // 
            this.tbRadius.Location = new System.Drawing.Point(117, 80);
            this.tbRadius.Margin = new System.Windows.Forms.Padding(1);
            this.tbRadius.Name = "tbRadius";
            this.tbRadius.Size = new System.Drawing.Size(95, 23);
            this.tbRadius.TabIndex = 10;
            this.tbRadius.Validated += new System.EventHandler(this.tbRadius_Validated);
            // 
            // tbArcLength
            // 
            this.tbArcLength.Enabled = false;
            this.tbArcLength.Location = new System.Drawing.Point(117, 106);
            this.tbArcLength.Margin = new System.Windows.Forms.Padding(1);
            this.tbArcLength.Name = "tbArcLength";
            this.tbArcLength.Size = new System.Drawing.Size(95, 23);
            this.tbArcLength.TabIndex = 10;
            // 
            // tbSegmentSquare
            // 
            this.tbSegmentSquare.Enabled = false;
            this.tbSegmentSquare.Location = new System.Drawing.Point(117, 132);
            this.tbSegmentSquare.Margin = new System.Windows.Forms.Padding(1);
            this.tbSegmentSquare.Name = "tbSegmentSquare";
            this.tbSegmentSquare.Size = new System.Drawing.Size(95, 23);
            this.tbSegmentSquare.TabIndex = 10;
            // 
            // errorProv
            // 
            this.errorProv.ContainerControl = this;
            // 
            // tbCenterY
            // 
            this.tbCenterY.Location = new System.Drawing.Point(117, 54);
            this.tbCenterY.Margin = new System.Windows.Forms.Padding(1);
            this.tbCenterY.Name = "tbCenterY";
            this.tbCenterY.Size = new System.Drawing.Size(95, 23);
            this.tbCenterY.TabIndex = 10;
            this.tbCenterY.Validated += new System.EventHandler(this.tbCenterY_Validated);
            // 
            // lbCenterY
            // 
            this.lbCenterY.AutoSize = true;
            this.lbCenterY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCenterY.Location = new System.Drawing.Point(1, 55);
            this.lbCenterY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbCenterY.Name = "lbCenterY";
            this.lbCenterY.Size = new System.Drawing.Size(114, 23);
            this.lbCenterY.TabIndex = 6;
            this.lbCenterY.Text = "Центр Y";
            this.lbCenterY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.label3.Size = new System.Drawing.Size(265, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "Геометрия";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CircleGeometryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CircleGeometryEditor";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Size = new System.Drawing.Size(230, 160);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbCenterX;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbRadius;
        private System.Windows.Forms.TextBox tbCenterX;
        private System.Windows.Forms.TextBox tbRadius;
        private System.Windows.Forms.ErrorProvider errorProv;
        private System.Windows.Forms.Label lbSegmentSquare;
        private System.Windows.Forms.Label lbArcLength;
        private System.Windows.Forms.TextBox tbArcLength;
        private System.Windows.Forms.TextBox tbSegmentSquare;
        private System.Windows.Forms.Label lbCenterY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbCenterY;
    }
}
