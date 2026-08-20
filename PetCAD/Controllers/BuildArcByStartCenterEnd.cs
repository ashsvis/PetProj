using PetCAD.Common;
using PetCAD.Renderers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PetCAD.Controllers
{
    public class BuildArcByStartCenterEnd : IBuildFigure
    {
        private readonly DrawControl drawer;
        private readonly Control zoomer;

        public BuildArcByStartCenterEnd(DrawControl drawer, Control zoomer)
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
            if (drawer.EditorMode == EditorMode.BuildArcStartCenterEnd)
            {
                var zoom = drawer.Zoom;
                if (drawer.IsDynamicalEnter)
                {
                    var pt = drawer.PrepareMousePosition(drawer.CurrentMousePosition);
                    var text = (drawer.MouseClickCount == 0
                        ? $"Начальная точка дуги " : drawer.MouseClickCount == 1 
                              ? $"Центральная точка дуги " : "Конечная точка дуги (удерживайте CTRL для изменения направления)") + $" X:{pt.X} Y:{pt.Y}";
                    using (var pen = new Pen(Color.Black, zoom))
                    using (var font = new Font("Arial", (float)(10f / zoom)))
                        e.Graphics.DrawString(text, font, Brushes.Black, drawer.PrepareMousePosition(PointF.Add(drawer.CurrentMousePosition, new SizeF(1f, 1f))));
                }
                if (drawer.MouseClickCount == 1)
                {
                    using (var pen = new Pen(Color.Orange, (float)(2.6f / zoom)) { DashStyle = DashStyle.Dash })
                        drawer.DrawRibbonLine(e.Graphics, pen, drawer.FirstMouseDown, drawer.CurrentMousePosition);
                }
                else if (drawer.MouseClickCount == 2)
                {
                    using (var pen = new Pen(Color.Orange, (float)(2.6f / zoom)) { DashStyle = DashStyle.Dash })
                        drawer.DrawRibbonLine(e.Graphics, pen, drawer.SecondMouseDown, drawer.CurrentMousePosition, false);
                    using (var pen = new Pen(Color.LightPink, (float)(2.6f / zoom)))
                        drawer.DrawRibbonArc(e.Graphics, pen, drawer.FirstMouseDown, drawer.SecondMouseDown, drawer.CurrentMousePosition);
                }
            }
        }

        public void Container_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildArcStartCenterEnd)
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
                    // поиск ближайшей точки привязки, если включен режим объектной привязки
                    pt3 = drawer.FindBindingPoint(pt3);

                    drawer.AddArcByStartCenterEnd(pt1, pt2, pt3);

                    drawer.SelectionController.Selection.Clear();
                    drawer.ClearMouseCount();
                    drawer.Changed = true;
                }
            }
        }

        public void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawer.EditorMode == EditorMode.BuildArcStartCenterEnd)
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
                        drawer.SendParamsOnChange(new object[] { pt });
                        //var pt1 = drawer.FirstMouseDown;                                    // первая точка нажатия
                        //var pt2 = drawer.SecondMouseDown;                                   // вторая точка нажатия
                        //var pt3 = drawer.PrepareMousePosition(drawer.CurrentMousePosition); // третья точка нажатия
                        //drawer.SendParamsOnChange(new object[] { pt1, pt2, pt3 });
                    }
                }
            }
        }

        public void SetParameters(string[] strings)
        {
            if (drawer.EditorMode == EditorMode.BuildArcStartCenterEnd)
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
