using PetCAD.Common;
using PetCAD.Renderers;
using System.Drawing;
using System.Windows.Forms;

namespace PetCAD.Controllers
{
    public class BuildBlockInsertController : IBuildFigureController
    {
        private readonly DrawControl drawer;

        public BuildBlockInsertController(DrawControl drawer, Control zoomer)
        {
            this.drawer = drawer;
            //
            zoomer.MouseDown += Container_MouseDown;
            zoomer.MouseMove += Container_MouseMove;
            zoomer.Paint += Container_Paint;
        }

        public void Container_Paint(object sender, PaintEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildInsertBlock)
            {
                var zoom = drawer.Zoom;
                if (drawer.IsDynamicalEnter)
                {
                    var pt = drawer.PrepareMousePosition(drawer.CurrentMousePosition);
                    var text = $"Укажите точку вставки X:{pt.X} Y:{pt.Y}";
                    using (var font = new Font("Arial", (float)(10f / zoom)))
                        e.Graphics.DrawString(text, font, Brushes.Black,
                            drawer.PrepareMousePosition(PointF.Add(drawer.CurrentMousePosition, new SizeF(1f, 1f))));
                }
                using (var pen = new Pen(Color.LightPink, (float)(2.6f / zoom)))
                    drawer.DrawRibbonBlock(e.Graphics, pen, "Block", drawer.PrepareMousePosition(drawer.CurrentMousePosition));
            }
        }

        public void Container_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildInsertBlock)
            {
                var mousePosition = e.Location;
                var pt = drawer.PrepareMousePosition(mousePosition); // первая точка нажатия;
                // поиск ближайшей точки привязки, если включен режим объектной привязки
                pt = drawer.FindBindingPoint(pt);
                pt = drawer.FindOrthoPoint(pt);
                drawer.InsertBlock(pt, "Block");
            }
        }

        public void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildInsertBlock)
            {

            }        
        }

        public void SetParameters(string[] strings)
        {
            if (drawer.EditorMode == EditorMode.BuildInsertBlock)
            {
                //throw new NotImplementedException();
            }
        }
    }
}
