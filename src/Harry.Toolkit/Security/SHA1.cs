using Harry.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Security
{
    public static class SHA1
    {
#if  !NET35
        static Lazy<System.Security.Cryptography.SHA1> hasher = new Lazy<System.Security.Cryptography.SHA1>(() => System.Security.Cryptography.SHA1.Create(), true);
#else
        static System.Security.Cryptography.SHA1 hasher = System.Security.Cryptography.SHA1.Create();
#endif
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>返回大写SHA1值</returns>
        public static string ComputeHash(string input, Encoding encoding)
        {
            if (!input.HasValue())
            {
                throw new ArgumentNullException(nameof(input));
            }

            byte[] t =
#if !NET35
                hasher.Value.ComputeHash(encoding.GetBytes(input));
#else
                hasher.ComputeHash(encoding.GetBytes(input));
#endif

            StringBuilder sb = new StringBuilder(40);

            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("X").PadLeft(2, '0'));
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
