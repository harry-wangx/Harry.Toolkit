using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Harry.Extensions
{
    public static partial class StringExtensions
    {
#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool HasValue(this string value)
        {
#if NET35
            return !string.IsNullOrEmpty(value);
#else
            return !string.IsNullOrWhiteSpace(value);
#endif
        }

        public static bool Contains(this string value, char c)
        {
            return value.IndexOf(c) >= 0;
        }

        /// <summary>
        /// 是否IPv4
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPv4(this string ip)
        {
            const string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
            return Regex.IsMatch(ip, pattern, options);
        }


        #region System.ComponentModel.DataAnnotations

        /// <summary>
        /// 是否EMAIL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string input)
        {
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// 是否URL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrl(this string input)
        {
            const string pattern = @"^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$";

            const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
            return Regex.IsMatch(input, pattern, options);
        }
        #endregion

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

        public static Int32 ToInt32(this string s)
        {
            return Int32.Parse(s);
        }
        public static Int32 ToInt32(this string s, Int32 defaultValue)
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
        public static UInt32 ToUInt32(this string s)
        {
            return UInt32.Parse(s);
        }
        public static UInt32 ToUInt32(this string s, UInt32 defaultValue)
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

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (!value.HasValue())
            {
                return defaultValue;
            }
#if NET35
            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch
            {

            }
#else
            T result;
            if (Enum.TryParse<T>(value, out result))
            {
                return result;
            }
#endif
            return defaultValue;
        }

        /// <summary>
        /// 获取字符串的字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string value, Encoding encoding)
        {
            Throws.IfNull(value, nameof(value));
            Throws.IfNull(encoding, nameof(encoding));

            return encoding.GetBytes(value);
        }

        /// <summary>
        /// 获取字符串的字节数组(默认格式为UTF8)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string value)
        {
            Throws.IfNull(value, nameof(value));
            return Encoding.UTF8.GetBytes(value);
        }
        #endregion

    }
}

