#if !NET20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harry.Extensions
{
    public static class BytesExtensions
    {
        //public static string ToString(this byte[] bytes, string format, string connector = "", string preString = "")
        //{
        //    if (bytes == null || bytes.Length <= 0)
        //        return "";
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        sb.Append(preString);
        //        sb.Append(bytes[i].ToString(format));
        //        if (i <= bytes.Length - 2)
        //        {
        //            sb.Append(connector);
        //        }
        //    }
        //    return sb.ToString();
        //}

        //public static string ToBase64(this byte[] bytes, string format, string connector = "", string preString = "")
        //{
        //    if (bytes == null || bytes.Length <= 0)
        //        return "";
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        sb.Append(preString);
        //        sb.Append(bytes[i].ToString(format));
        //        if (i <= bytes.Length - 2)
        //        {
        //            sb.Append(connector);
        //        }
        //    }
        //    return sb.ToString();
        //}
    }
}

#endif