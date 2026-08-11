using PetCAD.Common;
using PetCAD.Renderers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PetCAD.Controllers
{
    public class BuildBlockController : IBuildFigureController
    {
        private readonly DrawControl drawer;

        public BuildBlockController(DrawControl drawer, Control zoomer)
        {
            this.drawer = drawer;
            //
            zoomer.MouseDown += Container_MouseDown;
            zoomer.MouseMove += Container_MouseMove;
            zoomer.Paint += Container_Paint;
        }

        public void Container_Paint(object sender, PaintEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildCreateBlock)
            {
                var zoom = drawer.Zoom;
                if (drawer.IsDynamicalEnter)
                {
                    var pt = drawer.PrepareMousePosition(drawer.CurrentMousePosition);
                    var text = (drawer.MouseClickCount == 0
                        ? $"Укажите базовую точку вставки " 
                        : drawer.MouseClickCount == 1 
                              ? $"Укажите точку первого угла " 
                              : "Укажите точку второго угла") + $" X:{pt.X} Y:{pt.Y}";
                    using (var font = new Font("Arial", (float)(10f / zoom)))
                        e.Graphics.DrawString(text, font, Brushes.Black, drawer.PrepareMousePosition(PointF.Add(drawer.CurrentMousePosition, new SizeF(1f, 1f))));
                }
                if (drawer.MouseClickCount >= 1)
                {
                    var basePoint = drawer.FirstMouseDown;
                    using (var pen = new Pen(Color.Black, 1f / zoom))
                    {
                        e.Graphics.DrawLine(pen,
                        new PointF(basePoint.X - 4f / zoom, basePoint.Y),
                        new PointF(basePoint.X + 4f / zoom, basePoint.Y));
                        e.Graphics.DrawLine(pen,
                            new PointF(basePoint.X, basePoint.Y - 4f / zoom),
                            new PointF(basePoint.X, basePoint.Y + 4f / zoom));
                    }
                }
                if (drawer.MouseClickCount == 2)
                {
                    drawer.DrawRibbonSelectionRect(e.Graphics, drawer.SecondMouseDown, drawer.CurrentMousePosition);

                }
            }
        }

        public void Container_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildCreateBlock)
            {
                var mousePosition = e.Location;
                if (drawer.MouseClickCount == 1)
                {
                    var pt = drawer.PrepareMousePosition(mousePosition); // вторая точка нажатия;
                    //поиск ближайшей точки привязки, если включен режим объектной привязки
                    pt = drawer.FindBindingPoint(pt);
                    pt = drawer.FindOrthoPoint(pt);
                    drawer.SecondMouseDown = pt;
                    drawer.AddMouseCount();
                }
                else if (drawer.MouseClickCount == 2)
                {
                    // построение дуги трём точкам 
                    var pt1 = drawer.FirstMouseDown; // первая точка нажатия (базовая точка вставки блока)
                    var pt2 = drawer.SecondMouseDown; // вторая точка нажатия (первый угол рамки выделения)
                    var pt3 = drawer.PrepareMousePosition(mousePosition); // третья точка нажатия (другой угол рамки выделения)
                    // поиск ближайшей точки привязки, если включен режим объектной привязки
                    pt3 = drawer.FindBindingPoint(pt3);

                    // создание блока здесь

                    drawer.SelectionController.Selection.Clear();
                    drawer.ClearMouseCount();
                    drawer.SetMode(EditorMode.Selection);
                    drawer.Changed = true;
                }
            }
        }

        public void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildCreateBlock)
            {
                //throw new System.NotImplementedException();
            }
        }

        public void SetParameters(string[] strings)
        {
            if (drawer.EditorMode == EditorMode.BuildCreateBlock)
            {
                //throw new System.NotImplementedException();
            }
        }
    }
}
