using PetProj.Renderers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PetProj.Controllers
{
    public class BuildArcByThreePointsController : IBuildFigureController
    {
        private readonly DrawControl drawer;
        private readonly Control zoomer;

        public BuildArcByThreePointsController(DrawControl drawer, Control zoomer)
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
            if (drawer.EditorMode == EditorMode.BuildArcThreePoints)
            {
                if (drawer.IsDynamicalEnter)
                {
                    var pt = drawer.PrepareMousePosition(drawer.CurrentMousePosition);
                    var text = (drawer.MouseClickCount == 0 
                        ? $"Начальная точка дуги " : drawer.MouseClickCount == 1 ? $"Вторая точка дуги " : "Конечная точка дуги") + $" X:{pt.X} Y:{pt.Y}";
                    using (var pen = new Pen(Color.Black, drawer.Zoom))
                    using (var font = new Font("Arial", (float)(10f / drawer.Zoom)))
                        e.Graphics.DrawString(text, font, Brushes.Black, drawer.PrepareMousePosition(PointF.Add(drawer.CurrentMousePosition, new SizeF(1f, 1f))));
                }
                if (drawer.MouseClickCount == 1)
                {
                    using (var pen = new Pen(Color.Orange, (float)(2.6f / drawer.Zoom)) { DashStyle = DashStyle.Dash })
                        drawer.DrawRibbonLine(e.Graphics, pen, drawer.FirstMouseDown, drawer.CurrentMousePosition);
                }
                else if (drawer.MouseClickCount == 2)
                {
                    using (var pen = new Pen(Color.Orange, (float)(2.6f / drawer.Zoom)) { DashStyle = DashStyle.Dash })
                        drawer.DrawRibbonLine(e.Graphics, pen, drawer.SecondMouseDown, drawer.CurrentMousePosition);
                    using (var pen = new Pen(Color.LightPink, (float)(2.6f / drawer.Zoom)))
                        drawer.DrawRibbonArc(e.Graphics, pen, drawer.FirstMouseDown, drawer.SecondMouseDown, drawer.CurrentMousePosition);
                }
            }
        }

        public void Container_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildArcThreePoints)
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
                    var pt1 = drawer.FirstMouseDown; // первая точка нажатия
                    var pt2 = drawer.SecondMouseDown; // вторая точка нажатия
                    var pt3 = drawer.PrepareMousePosition(mousePosition); // третья точка нажатия
                    //поиск ближайшей точки привязки, если включен режим объектной привязки
                    pt3 = drawer.FindBindingPoint(pt3);
                    pt3 = drawer.FindOrthoPoint(pt3);

                    //var pt2 = new PointF(pt3.X, pt1.Y); // раcчётная точка
                    //var pt4 = new PointF(pt1.X, pt3.Y); // раcчётная точка
                    //drawer.AddRectangle(pt1, pt2, pt3, pt4);

                    //drawer.SelectionController.Selection.Clear();
                    //drawer.MouseClickCount = 0;
                    drawer.ClearMouseCount();
                    //drawer.Changed = true;
                }
            }
        }

        public void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildArcThreePoints)
            {
                if (e.Button == MouseButtons.None)
                {
                    var mousePosition = e.Location;
                    var pt = drawer.PrepareMousePosition(mousePosition);
                    if (drawer.MouseClickCount == 0)
                        drawer.SendParamsOnChange(new object[] { pt });
                    else if (drawer.MouseClickCount == 1)
                        drawer.SendParamsOnChange(new object[] { pt });
                    else if (drawer.MouseClickCount == 2)
                    {
                        var pt1 = drawer.FirstMouseDown;    // первая точка нажатия
                        var pt2 = drawer.SecondMouseDown;   // вторая точка нажатия
                        var pt3 = drawer.PrepareMousePosition(drawer.CurrentMousePosition); // третья точка нажатия
                        //var pt2 = new PointF(pt3.X, pt1.Y); // расчётная точка
                        //var vector1 = pt2.Vector(pt1);
                        //var vector2 = pt3.Vector(pt2);
                        //drawer.SendParamsOnChange(new object[] { vector1.Length(), vector2.Length() });
                    }
                }
            }
        }

        public void SetParameters(string[] strings)
        {
            if (drawer.EditorMode == EditorMode.BuildArcThreePoints)
            {
                //if (strings.Length == 2)
                //{
                //    if (drawer.MouseClickCount == 0)
                //    {
                //        if (double.TryParse(strings[0], out double ppX) &&
                //            double.TryParse(strings[1], out double ppY))
                //            drawer.SetFirstPoint(ppX, ppY);
                //    }
                //    else
                //    {
                //        if (double.TryParse(strings[0], out double width) &&
                //            double.TryParse(strings[1], out double height))
                //        {
                //            drawer.SetRectangleWidthAndHeight(
                //                Math.Sign(drawer.CurrentMousePosition.X - drawer.FirstMouseDown.X) * width,
                //                Math.Sign(drawer.CurrentMousePosition.Y - drawer.FirstMouseDown.Y) * height);
                //        }
                //    }
                //}
            }
        }
    }
}
