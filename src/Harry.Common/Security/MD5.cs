using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Harry.Security
{
    public static class MD5
    {
#if !NET20 && !NET35
        static Lazy<System.Security.Cryptography.MD5> _md5 = new Lazy<System.Security.Cryptography.MD5>(() => System.Security.Cryptography.MD5.Create(), true);
#else
        static System.Security.Cryptography.MD5 _md5 = System.Security.Cryptography.MD5.Create();
#endif
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="sFile">文件流</param>
        /// <returns>返回大写MD5值</returns>
        public static string ComputeHash(Stream sFile)
        {
            byte[] t =
#if !NET20 && !NET35
                _md5.Value.ComputeHash(sFile);
#else
                _md5.ComputeHash(sFile);
#endif
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <returns>返回大写MD5值</returns>
        public static string ComputeHash(byte[] data)
        {
            byte[] t =
#if !NET20 && !NET35
                _md5.Value.ComputeHash(data);
#else
                _md5.ComputeHash(data);
#endif
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string ComputeHash(string input, Encoding encoding)
        {
            return ComputeHash(encoding.GetBytes(input));
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="charset">字符集</param>
        /// <returns></returns>
        public static string ComputeHash(string input, string charset)
        {
            return ComputeHash(input, Encoding.GetEncoding(charset));
        }

        /// <summary>
        /// MD5加密(默认使用UTF8)
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string ComputeHash(string input)
        {
            return ComputeHash(input, Encoding.UTF8);
        }
    }
}
