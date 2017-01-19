#if !NET20
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions
{
    public static class ByteExtensions
    {
        public static string ToBase64String(this byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
                return "";
            return Convert.ToBase64String(bytes);
        }
    }
}

#endif