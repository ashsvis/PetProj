using System;
using System.Drawing;
using System.Windows.Forms;

namespace PetProj
{
    //public class MmsPoint
    //{
    //    public float X { get; private set; }
    //    public float Y { get; private set; }

    //    public MmsPoint(Control control, PointF point)
    //    {
    //        int dpi = control.DeviceDpi;
    //        var kf = 25.4f / dpi;
    //        X = point.X * kf;
    //        Y = point.Y * kf;
    //    }

    //    public override string ToString()
    //    {
    //        return "{" + X + ", " + Y + "}";
    //    }

    //    public static float GetLength(Control control, PointF pt1, PointF pt2)
    //    {
    //        int dpi = control.DeviceDpi;
    //        var kf = 25.4f / dpi;
    //        var len = Math.Sqrt((pt2.X - pt1.X) * (pt2.X - pt1.X) + (pt2.Y - pt1.Y) * (pt2.Y - pt1.Y)) * kf;
    //        return (float)len;
    //    }

    //    public static float GetAngle(PointF pt1, PointF pt2)
    //    {
    //        var dx = pt2.X - pt1.X;
    //        var dy = pt2.Y - pt1.Y;
    //        var andle = Math.Atan2(dy, dx) * 180 / Math.PI;
    //        return (float)andle;
    //    }

    //    public static string GetAngleString(PointF pt1, PointF pt2)
    //    {
    //        var dx = pt2.X - pt1.X;
    //        var dy = pt2.Y - pt1.Y;
    //        var andle = Math.Abs(Math.Atan2(dy, dx) * 180 / Math.PI);
    //        return $"{(float)andle}°";
    //    }
    //}
}
