using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions
{
    public static class ByteExtensions
    {
        /// <summary>
        /// 反转高低位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte ReverseHighLow(this byte input)
        {
            int result = 0;
            for (int i = 0; i < 8; i++)
            {
                result |= (((input & (1 << i)) >> i) << (7 - i));
            }
            return (byte)result;
        }
    }
}

