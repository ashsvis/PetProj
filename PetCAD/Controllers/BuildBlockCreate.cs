using PetCAD.Commands;
using PetCAD.Common;
using PetCAD.Figures;
using PetCAD.Renderers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PetCAD.Controllers
{
    public class BuildBlockCreate : IBuildFigure
    {
        private readonly DrawControl drawer;

        public BuildBlockCreate(DrawControl drawer, Control zoomer)
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
                        e.Graphics.DrawString(text, font, Brushes.Black, 
                            drawer.PrepareMousePosition(PointF.Add(drawer.CurrentMousePosition, new SizeF(1f, 1f))));
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
                    var ptBaseInsert = drawer.FirstMouseDown; // первая точка нажатия (базовая точка вставки блока)
                    var ptFirstSelectCorner = drawer.SecondMouseDown; // вторая точка нажатия (первый угол рамки выделения)
                    var ptSecondSelectCorner = drawer.PrepareMousePosition(mousePosition); // третья точка нажатия (другой угол рамки выделения)
                    // поиск ближайшей точки привязки, если включен режим объектной привязки
                    ptSecondSelectCorner = drawer.FindBindingPoint(ptSecondSelectCorner);

                    var selMode = ptFirstSelectCorner.X > ptSecondSelectCorner.X;
                    var rectangle = new RectangleF(
                        Math.Min(ptFirstSelectCorner.X, ptSecondSelectCorner.X), Math.Min(ptFirstSelectCorner.Y, ptSecondSelectCorner.Y),
                        Math.Abs(ptFirstSelectCorner.X - ptSecondSelectCorner.X), Math.Abs(ptFirstSelectCorner.Y - ptSecondSelectCorner.Y));
                    var figures = new List<Figure>();
                    drawer.SelectionController.Selection.Clear();
                    drawer.SelectionController.SelectUnselectByFrame(drawer.Figures, drawer.ShiftPressed,
                        selMode, rectangle, (manager, fig) =>
                            {
                                if (!figures.Contains(fig))
                                    figures.Add(fig);
                            }, (manager, fig) =>
                            {
                                if (figures.Contains(fig))
                                    figures.Remove(fig);
                            }
                        );
                    // создание блока здесь
                    if (figures.Count > 0)
                    {
                        if (drawer.AddBlock("Block", ptBaseInsert, figures))
                        {
                            drawer.InsertBlock(ptBaseInsert, "Block");
                            // удаляем фигуры, которые теперь вошли в блок
                            foreach (Figure fig in figures)
                                drawer.UndoRedoManager.Execute(new RemoveFigureCommand(drawer.Figures, fig));
                        }
                    }
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
