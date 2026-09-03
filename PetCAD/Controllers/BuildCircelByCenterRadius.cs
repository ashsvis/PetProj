using PetCAD.Common;
using PetCAD.Renderers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PetCAD.Controllers
{
    public class BuildCircelByCenterRadius : IBuildFigure
    {
        private readonly DrawControl drawer;
        private readonly Control zoomer;

        public BuildCircelByCenterRadius(DrawControl drawer, Control zoomer)
        {
            this.drawer = drawer;
            this.zoomer = zoomer;
            //
            zoomer.MouseDown += Container_MouseDown;
            zoomer.MouseMove += Container_MouseMove;
            zoomer.Paint += Container_Paint;
        }

        public void Container_Paint(object sender, PaintEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildCircR)
            {
                var zoom = drawer.Zoom;
                if (drawer.IsDynamicalEnter)
                {
                    var pt = drawer.PrepareMousePosition(drawer.CurrentMousePosition);
                    var text = (drawer.MouseClickCount == 0
                        ? $"Центр круга " : "Радиус круга") + $" X:{pt.X} Y:{pt.Y}";
                    using (var pen = new Pen(Color.Black, zoom))
                    using (var font = new Font("Arial", (float)(10f / zoom)))
                        e.Graphics.DrawString(text, font, Brushes.Black, drawer.PrepareMousePosition(PointF.Add(drawer.CurrentMousePosition, new SizeF(1f, 1f))));
                }
                if (drawer.MouseClickCount == 1)
                {
                    using (var pen = new Pen(Color.LightPink, (float)(2.6f / zoom)))
                        drawer.DrawRibbonCircle(e.Graphics, pen, drawer.FirstMouseDown, drawer.CurrentMousePosition);
                    using (var pen = new Pen(Color.Orange, (float)(2.6f / zoom)) { DashStyle = DashStyle.Dash })
                        drawer.DrawRibbonSizeLine(e.Graphics, pen, drawer.FirstMouseDown, drawer.CurrentMousePosition, false);
                }
            }
        }

        public void Container_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildCircR)
            {
                var mousePosition = e.Location;
                if (drawer.MouseClickCount == 1)
                {
                    // построение круга по двум точкам 
                    var pt1 = drawer.FirstMouseDown; // первая точка нажатия
                    var pt2 = drawer.PrepareMousePosition(mousePosition); // вторая точка нажатия
                    // поиск ближайшей точки привязки, если включен режим объектной привязки
                    pt2 = drawer.FindBindingPoint(pt2);

                    drawer.AddCircleByCenterRadius(pt1, pt2);

                    drawer.SelectionController.Selection.Clear();
                    drawer.ClearMouseCount();
                    drawer.Changed = true;
                }
            }
        }

        public void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildCircR)
            {
                if (e.Button == MouseButtons.None)
                {
                    var mousePosition = e.Location;
                    var pt = drawer.PrepareMousePosition(mousePosition);
                    if (drawer.MouseClickCount == 0)
                        drawer.SendParamsOnChange(new object[] { pt });
                    else if (drawer.MouseClickCount == 1)
                        drawer.SendParamsOnChange(new object[] { pt });
                }
            }
        }

        public void SetParameters(string[] strings)
        {
            if (drawer.EditorMode == EditorMode.BuildCircR)
            {

            }
        }
    }
}
