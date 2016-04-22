using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Harry.Extensions
{
    public static partial class StringExtensions
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        #region 字符串转其它格式
        public static Int16 ToInt16(this string s)
        {
            return Int16.Parse(s);
        }
        public static Int16 ToInt16(this string s, Int16 defaultValue)
        {
            if (!s.HasValue())
                return defaultValue;
            Int16 result = 0;
            if (Int16.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
        public static Int32 ToInt(this string s)
        {
            return Int32.Parse(s);
        }
        public static Int32 ToInt(this string s, Int32 defaultValue)
        {
            if (!s.HasValue())
                return defaultValue;
            Int32 result = 0;
            if (Int32.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
        public static Int64 ToInt64(this string s)
        {
            return Int64.Parse(s);
        }
        public static Int64 ToInt64(this string s, Int64 defaultValue)
        {
            if (!s.HasValue())
                return defaultValue;
            Int64 result = 0;
            if (Int64.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static UInt16 ToUInt16(this string s)
        {
            return UInt16.Parse(s);
        }
        public static UInt16 ToUInt16(this string s, UInt16 defaultValue)
        {
            if (!s.HasValue())
                return defaultValue;
            UInt16 result = 0;
            if (UInt16.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
        public static UInt32 ToUInt(this string s)
        {
            return UInt32.Parse(s);
        }
        public static UInt32 ToUInt(this string s, UInt32 defaultValue)
        {
            if (!s.HasValue())
                return defaultValue;
            UInt32 result = 0;
            if (UInt32.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
        public static UInt64 ToUInt64(this string s)
        {
            return UInt64.Parse(s);
        }
        public static UInt64 ToUInt64(this string s, UInt64 defaultValue)
        {
            if (!s.HasValue())
                return defaultValue;
            UInt64 result = 0;
            if (UInt64.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static decimal ToDecimal(this string s)
        {
            return decimal.Parse(s);
        }
        public static decimal ToDecimal(this string s, decimal defaultValue)
        {
            if (!s.HasValue())
                return defaultValue;
            decimal result = 0;
            if (decimal.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static float ToSingle(this string s)
        {
            return float.Parse(s);
        }
        public static float ToSingle(this string s, float defaultValue)
        {
            if (!s.HasValue())
                return defaultValue;
            float result = 0f;
            if (float.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static double ToDouble(this string s)
        {
            return double.Parse(s);
        }
        public static double ToDouble(this string s, double defaultValue)
        {
            if (!s.HasValue())
                return defaultValue;
            double result = 0f;
            if (double.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static T ToEnum<T>(this string value, T defaultValue)
        {
            if (!value.HasValue())
            {
                return defaultValue;
            }
            T result;
            try
            {
                result = (T)((object)Enum.Parse(typeof(T), value, true));
            }
            catch (ArgumentException)
            {
                result = defaultValue;
            }
            return result;
        }
        #endregion

    }
}
