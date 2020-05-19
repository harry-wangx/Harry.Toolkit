using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions
{
    public static class ByteExtensions
    {
        public static string ToBase64String(this byte[] bytes, Base64FormattingOptions? options = null)
        {
            if (bytes == null || bytes.Length <= 0)
                return string.Empty;

            if (options != null)
                return Convert.ToBase64String(bytes, options.Value);
            else
                return Convert.ToBase64String(bytes);
        }
    }
}

