namespace PetProj
{
    partial class DrawControl
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
            this.zoomPad = new PetProj.ZoomControl();
            this.SuspendLayout();
            // 
            // zoomPad
            // 
            this.zoomPad.Cursor = System.Windows.Forms.Cursors.Cross;
            this.zoomPad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoomPad.Location = new System.Drawing.Point(0, 0);
            this.zoomPad.MaxZoom = 100F;
            this.zoomPad.MinZoom = 0.01F;
            this.zoomPad.Name = "zoomPad";
            this.zoomPad.Size = new System.Drawing.Size(423, 298);
            this.zoomPad.TabIndex = 0;
            this.zoomPad.ZoomScale = 1D;
            this.zoomPad.ZoomSensitivity = 0.2F;
            this.zoomPad.OnDraw += new System.EventHandler<PetProj.ZoomControl.DrawEventArgs>(this.zoomPad_OnDraw);
            this.zoomPad.MouseDown += new System.Windows.Forms.MouseEventHandler(this.zoomPad_MouseDown);
            this.zoomPad.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zoomPad_MouseMove);
            this.zoomPad.MouseUp += new System.Windows.Forms.MouseEventHandler(this.zoomPad_MouseUp);
            // 
            // DrawControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zoomPad);
            this.Name = "DrawControl";
            this.Size = new System.Drawing.Size(423, 298);
            this.ResumeLayout(false);

        }

        #endregion

        private ZoomControl zoomPad;
    }
}
