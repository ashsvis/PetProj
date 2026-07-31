using PetProj.Common;
using PetProj.Renderers;
using System.Drawing;
using System.Windows.Forms;

namespace PetProj.Controllers
{
    public class BuildLineController : IBuildFigureController
    {
        private readonly DrawControl drawer;

        public BuildLineController(DrawControl drawer, Control zoomer)
        {
            this.drawer = drawer;
            //
            zoomer.MouseDown += Container_MouseDown;
            zoomer.MouseMove += Container_MouseMove;
            zoomer.Paint += Container_Paint;
        }

        public void Container_Paint(object sender, PaintEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildLines)
            {
                if (drawer.IsDynamicalEnter)
                {
                    var pt = drawer.PrepareMousePosition(drawer.CurrentMousePosition);
                    var text = (drawer.MouseClickCount == 0 ? $"Первая точка " : $"Следующая точка ") + $" X:{pt.X} Y:{pt.Y}";
                    using (var pen = new Pen(Color.Black, drawer.Zoom))
                    using (var font = new Font("Arial", (float)(10f / drawer.Zoom)))
                        e.Graphics.DrawString(text, font, Brushes.Black,
                            drawer.PrepareMousePosition(PointF.Add(drawer.CurrentMousePosition, new SizeF(1f, 1f))));
                }
                if (drawer.MouseClickCount == 1)
                {
                    using (var pen = new Pen(Color.LightPink, (float)(2.6f / drawer.Zoom)))
                        drawer.DrawRibbonLine(e.Graphics, pen, drawer.FirstMouseDown, drawer.CurrentMousePosition);
                }
            }
        }

        public void Container_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildLines)
            {
                if (drawer.MouseClickCount == 1)
                {
                    // построение отрезков линий по двум точкам (концы отрезка)
                    var pt1 = drawer.FirstMouseDown;
                    var pt2 = drawer.PrepareMousePosition(drawer.CurrentMousePosition);
                    //поиск ортогональной точки, если включен режим ортогонального построения
                    pt2 = drawer.FindOrthoPoint(pt2);
                    //поиск ближайшей точки привязки, если включен режим объектной привязки
                    pt2 = drawer.FindBindingPoint(pt2);

                    drawer.AddLine(pt1, pt2);
                    // сброс количества нажатий, следующий прямоугольник будет строиться заново
                    // точка начала следующего отрезка совпадает с концом предыдущего отрезка
                    drawer.FirstMouseDown = pt2;
                    drawer.ClearMouseCount();
                    drawer.AddMouseCount();
                    drawer.Changed = true;
                }
                //else if (drawer.MouseClickCount > 1)
                //    drawer.MouseClickCount = 0;
            }
        }

        public void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildLines)
            {
                if (e.Button == MouseButtons.None)
                {
                    var mousePosition = e.Location;
                    var pt = drawer.PrepareMousePosition(mousePosition);
                    if (drawer.MouseClickCount == 0)
                        drawer.SendParamsOnChange(new object[] { pt });
                    else if (drawer.MouseClickCount == 1)
                    {
                        var pt1 = drawer.FirstMouseDown;
                        var pt2 = drawer.PrepareMousePosition(drawer.CurrentMousePosition);
                        var vector = pt2.Vector(pt1);
                        drawer.SendParamsOnChange(new object[] { vector.Length(), vector.AngleDegree() });
                    }
                }
            }
        }

        public void SetParameters(string[] strings)
        {
            if (drawer.EditorMode == EditorMode.BuildLines)
            {
                if (strings.Length == 2)
                {
                    if (drawer.MouseClickCount == 0)
                    {
                        if (double.TryParse(strings[0], out double ppX) &&
                            double.TryParse(strings[1], out double ppY))
                            drawer.SetFirstPoint(ppX, ppY);
                    }
                    else
                    {
                        if (double.TryParse(strings[0], out double length) &&
                            double.TryParse(strings[1], out double angledeg))
                            drawer.SetLineLengthAndAngle(length, angledeg);
                    }
                }
            }
        }
    }
}
