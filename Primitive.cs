using System.Collections.Generic;
using System.Drawing;

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
    }
}
