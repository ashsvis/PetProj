using System.Drawing;
using System.Windows.Forms;

namespace PetProj
{
    public class ToolStripSplitButtonCheckable : ToolStripSplitButton
    {
        private bool @checked = false;

        public bool Checked
        {
            get { return @checked; }
            set
            {
                @checked = value;
                Invalidate(); // Перерисовать элемент
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (@checked)
            {
                var rect = e.ClipRectangle;
                rect.Height -= 1;
                rect.Width = rect.Height;
                // Здесь логика отрисовки для «нажатого» состояния
                using (var pen = new Pen(Color.FromArgb(0, 120, 215)))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
            base.OnPaint(e);
        }
    }
}
