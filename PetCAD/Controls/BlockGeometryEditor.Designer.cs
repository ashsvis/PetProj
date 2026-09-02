namespace PetCAD.Controls
{
    partial class BlockGeometryEditor
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
            this.lbAngle = new System.Windows.Forms.Label();
            this.lbScaleY = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbScaleX = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbScaleX = new System.Windows.Forms.TextBox();
            this.tbScaleY = new System.Windows.Forms.TextBox();
            this.tbAngle = new System.Windows.Forms.TextBox();
            this.lbPositionX = new System.Windows.Forms.Label();
            this.lbPositionY = new System.Windows.Forms.Label();
            this.tbOffsetX = new System.Windows.Forms.TextBox();
            this.tbOffsetY = new System.Windows.Forms.TextBox();
            this.lbBlockNameCaption = new System.Windows.Forms.Label();
            this.errorProv = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbBlockName = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProv)).BeginInit();
            this.SuspendLayout();
            // 
            // lbAngle
            // 
            this.lbAngle.AutoSize = true;
            this.lbAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAngle.Location = new System.Drawing.Point(1, 160);
            this.lbAngle.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbAngle.Name = "lbAngle";
            this.lbAngle.Size = new System.Drawing.Size(99, 22);
            this.lbAngle.TabIndex = 4;
            this.lbAngle.Text = "Поворот";
            this.lbAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbScaleY
            // 
            this.lbScaleY.AutoSize = true;
            this.lbScaleY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbScaleY.Location = new System.Drawing.Point(1, 133);
            this.lbScaleY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbScaleY.Name = "lbScaleY";
            this.lbScaleY.Size = new System.Drawing.Size(99, 24);
            this.lbScaleY.TabIndex = 2;
            this.lbScaleY.Text = "Масштаб Y";
            this.lbScaleY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbScaleY, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbAngle, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbScaleX, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbScaleX, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbScaleY, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbAngle, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbPositionX, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbPositionY, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbOffsetX, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbOffsetY, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbBlockNameCaption, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbBlockName, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 184);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // lbScaleX
            // 
            this.lbScaleX.AutoSize = true;
            this.lbScaleX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbScaleX.Location = new System.Drawing.Point(1, 107);
            this.lbScaleX.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbScaleX.Name = "lbScaleX";
            this.lbScaleX.Size = new System.Drawing.Size(99, 23);
            this.lbScaleX.TabIndex = 2;
            this.lbScaleX.Text = "Масштаб X";
            this.lbScaleX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // tbScaleX
            // 
            this.tbScaleX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbScaleX.Enabled = false;
            this.tbScaleX.Location = new System.Drawing.Point(102, 106);
            this.tbScaleX.Margin = new System.Windows.Forms.Padding(1);
            this.tbScaleX.Name = "tbScaleX";
            this.tbScaleX.Size = new System.Drawing.Size(96, 23);
            this.tbScaleX.TabIndex = 10;
            // 
            // tbScaleY
            // 
            this.tbScaleY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbScaleY.Enabled = false;
            this.tbScaleY.Location = new System.Drawing.Point(102, 132);
            this.tbScaleY.Margin = new System.Windows.Forms.Padding(1);
            this.tbScaleY.Name = "tbScaleY";
            this.tbScaleY.Size = new System.Drawing.Size(95, 23);
            this.tbScaleY.TabIndex = 10;
            // 
            // tbAngle
            // 
            this.tbAngle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAngle.Enabled = false;
            this.tbAngle.Location = new System.Drawing.Point(102, 159);
            this.tbAngle.Margin = new System.Windows.Forms.Padding(1);
            this.tbAngle.Name = "tbAngle";
            this.tbAngle.Size = new System.Drawing.Size(95, 23);
            this.tbAngle.TabIndex = 10;
            // 
            // lbPositionX
            // 
            this.lbPositionX.AutoSize = true;
            this.lbPositionX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPositionX.Location = new System.Drawing.Point(1, 55);
            this.lbPositionX.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbPositionX.Name = "lbPositionX";
            this.lbPositionX.Size = new System.Drawing.Size(99, 23);
            this.lbPositionX.TabIndex = 2;
            this.lbPositionX.Text = "Положение X";
            this.lbPositionX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbPositionY
            // 
            this.lbPositionY.AutoSize = true;
            this.lbPositionY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPositionY.Location = new System.Drawing.Point(1, 81);
            this.lbPositionY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbPositionY.Name = "lbPositionY";
            this.lbPositionY.Size = new System.Drawing.Size(99, 23);
            this.lbPositionY.TabIndex = 2;
            this.lbPositionY.Text = "Положение Y";
            this.lbPositionY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbOffsetX
            // 
            this.tbOffsetX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOffsetX.Enabled = false;
            this.tbOffsetX.Location = new System.Drawing.Point(102, 54);
            this.tbOffsetX.Margin = new System.Windows.Forms.Padding(1);
            this.tbOffsetX.Name = "tbOffsetX";
            this.tbOffsetX.Size = new System.Drawing.Size(96, 23);
            this.tbOffsetX.TabIndex = 10;
            // 
            // tbOffsetY
            // 
            this.tbOffsetY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOffsetY.Enabled = false;
            this.tbOffsetY.Location = new System.Drawing.Point(102, 80);
            this.tbOffsetY.Margin = new System.Windows.Forms.Padding(1);
            this.tbOffsetY.Name = "tbOffsetY";
            this.tbOffsetY.Size = new System.Drawing.Size(96, 23);
            this.tbOffsetY.TabIndex = 10;
            // 
            // lbBlockNameCaption
            // 
            this.lbBlockNameCaption.AutoSize = true;
            this.lbBlockNameCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBlockNameCaption.Location = new System.Drawing.Point(1, 29);
            this.lbBlockNameCaption.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lbBlockNameCaption.Name = "lbBlockNameCaption";
            this.lbBlockNameCaption.Size = new System.Drawing.Size(99, 23);
            this.lbBlockNameCaption.TabIndex = 2;
            this.lbBlockNameCaption.Text = "Имя";
            this.lbBlockNameCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // errorProv
            // 
            this.errorProv.ContainerControl = this;
            // 
            // tbBlockName
            // 
            this.tbBlockName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbBlockName.Enabled = false;
            this.tbBlockName.Location = new System.Drawing.Point(102, 28);
            this.tbBlockName.Margin = new System.Windows.Forms.Padding(1);
            this.tbBlockName.Name = "tbBlockName";
            this.tbBlockName.Size = new System.Drawing.Size(96, 23);
            this.tbBlockName.TabIndex = 10;
            this.tbBlockName.WordWrap = false;
            // 
            // BlockGeometryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "BlockGeometryEditor";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Size = new System.Drawing.Size(200, 185);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbAngle;
        private System.Windows.Forms.Label lbScaleY;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbScaleX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbScaleX;
        private System.Windows.Forms.TextBox tbScaleY;
        private System.Windows.Forms.TextBox tbAngle;
        private System.Windows.Forms.ErrorProvider errorProv;
        private System.Windows.Forms.Label lbPositionX;
        private System.Windows.Forms.Label lbPositionY;
        private System.Windows.Forms.TextBox tbOffsetX;
        private System.Windows.Forms.TextBox tbOffsetY;
        private System.Windows.Forms.Label lbBlockNameCaption;
        private System.Windows.Forms.TextBox tbBlockName;
    }
}
