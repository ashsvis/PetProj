using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;

namespace PetProj.Common
{
    internal static class ParseHelper
    {
        public static string DecimalSeparator => CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public static TEnum ParseEnumeration<TEnum>(string line, TEnum defaultValue) where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            var value = line;
            if (Enum.TryParse(value, out TEnum x))
                return x;
            return defaultValue;
        }

        /// <summary>
        /// Разбор символьной записи для получения byte значения
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static byte ParseByte(string line, byte defaultValue)
        {
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            var value = line;
            return byte.TryParse(value, out byte x) ? x : defaultValue;
        }

        /// <summary>
        /// Разбор символьной записи для получения int значения
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static int ParseInteger(string line, int defaultValue)
        {
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            var value = line;
            return int.TryParse(value, out int x) ? x : defaultValue;
        }

        /// <summary>
        /// Разбор символьной записи для получения bool значения
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool ParseBoolean(string line, bool defaultValue)
        {
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            var value = line;
            return bool.TryParse(value, out bool x) ? x : defaultValue;
        }

        /// <summary>
        /// Метод для кодирования компонентов цвета в строку
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ColorToString(Color color)
        {
            return $"{color.A};{color.R};{color.G};{color.B}";
        }

        /// <summary>
        /// Разбор символьной записи для получения цвета
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Color ParseColor(string line, Color defaultValue)
        {
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            // Разбиваем по запятым и убираем пустые элементы
            string[] tokens = line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 4)
            {
                string sa = tokens[0];
                string sr = tokens[1];
                string sg = tokens[2];
                string sb = tokens[3];
                // Проверяем, что удалось успешно преобразовать все компоненты
                if (byte.TryParse(sa, out byte a) &&
                    byte.TryParse(sr, out byte r) &&
                    byte.TryParse(sg, out byte g) &&
                    byte.TryParse(sb, out byte b))
                    return Color.FromArgb(a, r, g, b);
            }
            else if (tokens.Length == 3)
            {
                string sr = tokens[0];
                string sg = tokens[1];
                string sb = tokens[2];
                // Проверяем, что удалось успешно преобразовать все компоненты
                if (byte.TryParse(sr, out byte r) &&
                    byte.TryParse(sg, out byte g) &&
                    byte.TryParse(sb, out byte b))
                    return Color.FromArgb(r, g, b);
            }
            return defaultValue;
        }

        /// <summary>
        /// Разбор символьной записи для получения float значения
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static float ParseSingle(string line, float defaultValue)
        {
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            var value = line;
            switch (DecimalSeparator)
            {
                case ".":
                    value = value.Replace(',', '.');
                    break;
                case ",":
                    value = value.Replace('.', ',');
                    break;
            }
            return float.TryParse(value, out float x) ? x : defaultValue;
        }

        /// <summary>
        /// Разбор символьной записи для получения координат точки
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static PointF ParsePointF(string line, PointF defaultValue)
        {
            if (string.IsNullOrWhiteSpace(line)) return defaultValue;
            // Разбиваем по запятым и убираем пустые элементы
            string[] tokens = line.Trim('{', '}').Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 2)
            {
                string valueX = tokens[0].Split('=').Last();
                string valueY = tokens[1].Split('=').Last();

                switch (DecimalSeparator)
                {
                    case ".":
                        valueX = valueX.Replace(',', '.');
                        valueY = valueY.Replace(',', '.');
                        break;
                    case ",":
                        valueX = valueX.Replace('.', ',');
                        valueY = valueY.Replace('.', ',');
                        break;
                }

                // Проверяем, что удалось успешно преобразовать обе координаты
                if (float.TryParse(valueX, out float x) && float.TryParse(valueY, out float y))
                    return new PointF(x, y);
            }
            return defaultValue;
        }
    }
}
