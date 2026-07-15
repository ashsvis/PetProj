using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace PetProj
{
    public abstract class Primitive
    {
        public Primitive() 
        { 
         
        }

        public PointF Origin { get; set; } = new PointF(0, 0);

        public List<SizeF> Offsets { get; set; } = new List<SizeF>();

        public abstract void DrawAt(Graphics graphics, Color forecolor);
        public abstract void DrawHighlightAt(Graphics graphics, Color forecolor);
        public abstract bool Contains(PointF point);
        public abstract XElement GetData();
        public abstract void DrawSelectedAt(Graphics graphics, Color forecolor);
    }
}
