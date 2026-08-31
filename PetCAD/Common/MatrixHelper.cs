using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;

namespace PetCAD.Common
{
    internal static class MatrixHelper
    {
        private static System.Windows.Media.Matrix CreateWindowsMatrix(Matrix matrix)
        {
            return new System.Windows.Media.Matrix(matrix.Elements[0], matrix.Elements[1], matrix.Elements[2],
                                                       matrix.Elements[3], matrix.Elements[4], matrix.Elements[5]);
        }

        public static float GetAngle(this Matrix matrix)
        {
            var x = new Vector(1, 0);
            var winMatrix = CreateWindowsMatrix(matrix);
            var rotated = Vector.Multiply(x, winMatrix);
            var angleBetween = Vector.AngleBetween(x, rotated);
            return (float)angleBetween;
        }

        public static SizeF GetScale(this Matrix matrix)
        {
            var x = new Vector(1, 0);
            var y = new Vector(0, 1);
            var winMatrix = CreateWindowsMatrix(matrix);
            var scaledX = Vector.Multiply(x, winMatrix);
            var scaledY = Vector.Multiply(y, winMatrix);
            return new SizeF((float)scaledX.Length, (float)scaledY.Length);
        }

        public static float GetSkewAngle(this Matrix matrix)
        {
            var x = new Vector(1, 0);
            var y = new Vector(0, 1);
            var winMatrix = CreateWindowsMatrix(matrix);
            var skewX = Vector.Multiply(x, winMatrix);
            var skewY = Vector.Multiply(y, winMatrix);
            var angleBetween = Vector.AngleBetween(skewX, skewY);
            return (float)angleBetween;
        }

        public static PointF GetOffset(this Matrix matrix)
        {
            var winMatrix = CreateWindowsMatrix(matrix);

            return new PointF((float)winMatrix.OffsetX, (float)winMatrix.OffsetY);
        }

    }
}
