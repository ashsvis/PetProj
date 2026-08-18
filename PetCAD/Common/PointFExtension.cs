using PetCAD.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace PetCAD.Common
{
    public static class PointFExtension
    {
        public const float PI = (float)Math.PI;
        public const float PI2 = 2 * PI;
        public const float TO_DEGREES = 180 / (float)Math.PI;
        public const float TO_RADIANS = (float)Math.PI / 180;

        /// <summary>
        /// Длина (модуль) вектора
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>число</returns>
        public static float Length(this PointF vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        /// <summary>
        /// Квадрат длины вектора
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>число</returns>
        public static float LengthSqr(this PointF vector)
        {
            return vector.X * vector.X + vector.Y * vector.Y;
        }

        /// <summary>
        /// Скалярное произведение векторов
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns>число - длина вектора, соединяющего концы умножаемых векторов</returns>
        public static float DotScalar(this PointF vector1, PointF vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        /// <summary>
        /// Сложение двух векторов
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns>Вектор</returns>
        public static PointF Add(this PointF vector1, PointF vector2)
        {
            return new PointF(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        /// <summary>
        /// Вычитание двух векторов
        /// </summary>
        /// <param name="vector2"></param>
        /// <param name="vector1"></param>
        /// <returns>Вектор</returns>
        public static PointF Sub(this PointF vector1, PointF vector2)
        {
            return new PointF(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        /// <summary>
        /// Минимальный угол между векторами, в рад
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns>число - угол</returns>
        public static float Angle(this PointF vector1, PointF vector2)
        {
            var a = vector1.Angle() - vector2.Angle();
            a += (a > PI) ? -2 * PI : (a < -PI) ? 2 * PI : 0;

            return a;
        }

        /// <summary>
        /// Angle (-PI ; PI]
        /// </summary>
        private static float Angle(this PointF c)
        {
            return (float)Math.Atan2(c.Y, c.X);
        }

        /// <summary>
        /// Angle (-180° ; 180°]
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float AngleDegree(this PointF c)
        {
            return (float)(Math.Atan2(c.Y, c.X) * 180 / Math.PI);
        }


        /// <summary>
        /// Поворот вектора на угол
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="angle">угол поворота</param>
        /// <returns></returns>
        public static PointF Rotate(this PointF vector, float angle)
        {
            return new PointF((float) (vector.X * Math.Cos(angle) - vector.Y * Math.Sin(angle)),
                              (float) (vector.X * Math.Sin(angle) + vector.Y * Math.Cos(angle)));
        }

        /// <summary>
        /// Строим вектор на координатах двух точек
        /// </summary>
        /// <param name="point2"></param>
        /// <param name="point1"></param>
        /// <returns>Вектор</returns>
        public static PointF Vector(this PointF point2, PointF point1)
        {
            return new PointF(point2.X - point1.X, point2.Y - point1.Y);
        }

        /// <summary>
        /// Строим вектор на координатах двух точек
        /// </summary>
        /// <param name="point2"></param>
        /// <param name="point1"></param>
        /// <returns>Вектор</returns>
        public static PointF Vector(this Point point2, Point point1)
        {
            return new PointF(point2.X - point1.X, point2.Y - point1.Y);
        }

        /// <summary>
        /// Ортогональная проекция Вектора 1 на Вектор 2
        /// </summary>
        /// <param name="vector1">Вектор 1</param>
        /// <param name="vector2">Вектор 2</param>
        /// <returns>число - длина проекции</returns>
        public static float OrtoProjection(this PointF vector1, PointF vector2)
        {
            return vector1.DotScalar(vector2)/vector2.Length();
        }

        /// <summary>
        /// Умножение вектора на скаляр
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">множитель</param>
        /// <returns>Вектор</returns>
        public static PointF MulTo(this PointF vector, float value)
        {
            return new PointF(vector.X * value, vector.Y * value);
        }

        /// <summary>
        /// Деление вектора на скаляр
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">делитель</param>
        /// <returns>Вектор</returns>
        public static PointF DivTo(this PointF vector, float value)
        {
            if (Math.Abs(value) < float.Epsilon) throw new DivideByZeroException();
            return new PointF(vector.X / value, vector.Y / value);
        }

        /// <summary>
        /// Нахождение коеффициента масштабирования
        /// </summary>
        /// <param name="vector1">Вектор 1</param>
        /// <param name="vector2">Вектор 2</param>
        /// <returns>Вектор</returns>
        public static PointF ScaleKoeff(this PointF vector1, PointF vector2)
        {
            if (vector2.Length() < float.Epsilon) return vector2.Clone().MulTo(float.Epsilon);
            return new PointF(Math.Abs(vector1.X / vector2.X), Math.Abs(vector1.Y / vector2.Y));
        }

        /// <summary>
        /// Нормализация вектора
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>Нормализованный вектор</returns>
        public static PointF Norm(this PointF vector)
        {
            var length = vector.Length();
            if (Math.Abs(length - 0f) < float.Epsilon) throw new DivideByZeroException();
            return new PointF(vector.X / length, vector.Y / length);
        }

        public static PointF Clone(this PointF vector)
        {
            return new PointF(vector.X, vector.Y);
        }

        /// <summary>
        /// Проекция точки c на отрезок (a, b)
        /// </summary>
        /// <param name="a">Начало отрезка</param>
        /// <param name="b">Конец отрезка</param>
        /// <param name="c">Проецируемая точка</param>
        /// <param name="norm">точка проекции</param>
        /// <returns>true - если проекция точки попадает на отрезок</returns>
        public static bool ProjectPointOnSegment(PointF a, PointF b, PointF c, out PointF norm)
        {
            norm = PointF.Empty;
            // Вектор AB
            double abX = b.X - a.X;
            double abY = b.Y - a.Y;
            // Скалярное произведение (AC · AB)
            double acX = c.X - a.X;
            double acY = c.Y - a.Y;
            double dotProduct = acX * abX + acY * abY;
            // Если отрезок вырожден (точки A и B совпадают)
            if (abX == 0 && abY == 0)
                return false;
            // Параметр t, показывающий положение проекции на прямой AB
            double t = dotProduct / (abX * abX + abY * abY);
            // Если проекция вне отрезка [0, 1], возвращаем ближайшую конечную точку
            if (t <= 0)
                return false;
            if (t >= 1)
                return false;
            // Иначе вычисляем и возвращаем проекцию внутри отрезка
            double projX = a.X + abX * t;
            double projY = a.Y + abY * t;
            norm = new PointF((float)projX, (float)projY);
            return true;
        }

        public static bool NormalPointOnArc(ArcGeometry arc, PointF basePoint, out PointF[] norms)
        {
            norms = new PointF[] { };
            if (basePoint == arc.CenterPoint || arc.Radius <= 0)
                return false;

            var x1 = basePoint.X;
            var y1 = basePoint.Y;
            var xc = arc.CenterPoint.X;
            var yc = arc.CenterPoint.Y;

            var arr = new PointF[] { basePoint };
            using (var m = new Matrix())
            {
                m.RotateAt(180f, arc.CenterPoint);
                m.TransformPoints(arr);
            }
            var x2 = arr[0].X;
            var y2 = arr[0].Y;
            var radius = arc.Radius;
            // квадрат длины отрезка
            double a = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            // двойное скалярное произведение
            double b = 2 * ((x2 - x1) * (x1 - xc) + (y2 - y1) * (y1 - yc));
            // квадрат расстояния от начала отрезка до центра минус квадрат радиуса
            double c = (x1 - xc) * (x1 - xc) + (y1 - yc) * (y1 - yc) - radius * radius;

            double discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
                return false;

            double t1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            double t2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            var list = new List<PointF>();
            if (t1 >= 0 && t1 <= 1)
            {
                PointF point1 = new PointF((float)(x1 + t1 * (x2 - x1)), (float)(y1 + t1 * (y2 - y1)));
                list.Add(point1);
            }

            if (t2 >= 0 && t2 <= 1 && t2 != t1)
            {
                PointF point2 = new PointF((float)(x1 + t2 * (x2 - x1)), (float)(y1 + t2 * (y2 - y1)));
                list.Add(point2);
            }
            norms = list.ToArray();
            return (t1 >= 0 && t1 <= 1) || (t2 >= 0 && t2 <= 1);
        }

        public static bool TangentPointOnArc(ArcGeometry arc, PointF basePoint, out PointF[] tangents)
        {
            tangents = new PointF[] { };
            if (basePoint == arc.CenterPoint || arc.Radius <= 0)
                return false;
            var ox = basePoint.X;
            var oy = basePoint.Y;
            var cx = arc.CenterPoint.X;
            var cy = arc.CenterPoint.Y;
            var radius = arc.Radius;

            // Шаг 1: Проверяем, не внутри ли точка
            double distanceOC = Math.Sqrt((cx - ox) * (cx - ox) + (cy - oy) * (cy - oy));
            if (distanceOC < radius)
            {
                // Точка P лежит внутри окружности.
                return false;
            }

            // Шаг 2: Вычисляем угол вектора OC
            double angleOC = Math.Atan2(oy - cy, ox - cx);

            // Шаг 3: Вычисляем угол для одной из точек касания
            double angle = Math.Acos(radius / distanceOC);

            // Шаг 4: Координаты точек касания
            double tx1 = cx + radius * Math.Cos(angleOC + angle);
            double ty1 = cy + radius * Math.Sin(angleOC + angle);
            var point1 = new PointF((float)tx1, (float)ty1);

            double tx2 = cx + radius * Math.Cos(angleOC - angle);
            double ty2 = cy + radius * Math.Sin(angleOC - angle);
            var point2 = new PointF((float)tx2, (float)ty2);

            var list = new List<PointF>
            {
                point1,
                point2
            };

            tangents = list.ToArray();
            return true;
        }
    }
}
