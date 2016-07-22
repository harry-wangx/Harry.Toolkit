using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Security
{
    public static class SHA1
    {
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string ComputeHash(string input, Encoding encoding)
        {
            if (!Common.Utils.HasValue(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            System.Security.Cryptography.SHA1 hasher = System.Security.Cryptography.SHA1.Create();

            byte[] t = hasher.ComputeHash(encoding.GetBytes(input));

            StringBuilder sb = new StringBuilder(32);

            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="charset">字符集</param>
        /// <returns></returns>
        public static string ComputeHash(string input, string charset)
        {
            return ComputeHash(input, Encoding.GetEncoding(charset));
        }

        /// <summary>
        /// SHA1加密(默认使用UTF8)
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string ComputeHash(string input)
        {
            return ComputeHash(input, Encoding.UTF8);
        }
    }
}
