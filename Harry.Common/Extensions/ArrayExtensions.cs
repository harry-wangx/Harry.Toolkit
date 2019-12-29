using System;
using System.Collections.Generic;
using System.Linq;

namespace Harry.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// 判断两个数组是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Equals<T>(this T[] left, T[] right)
        {
            //如果两个引用相等,返回true.都是null,也返回true
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            //如果有一方为null,返回false
            if (left is null || right is null)
            {
                return false;
            }

            //如果数组长度不一样,也返回false
            if (left.Length != right.Length)
            {
                return false;
            }

            //挨个比较元素
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < left.Length; i++)
            {
                //如果T是值类型,此处比较方法效率小于直接使用"=="号
                if (!comparer.Equals(left[i], right[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断源数组是否与目标数组指定范围内数据相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left">源数组</param>
        /// <param name="right">目标数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static bool Equals<T>(this T[] left, T[] right, int startIndex, int length)
        {
            Check.NotNull(left, nameof(left));
            Check.NotNull(right, nameof(right));

            if (startIndex < 0 || startIndex >= right.Length) throw new ArgumentOutOfRangeException("开始索引超范围");
            if (length < 0 || startIndex + length > right.Length) throw new ArgumentOutOfRangeException("长度超范围");

            //如果长度不相等,直接返回false
            if (left.Length != length)
            {
                return false;
            }

            //挨个比较元素
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < left.Length; i++)
            {
                var rightIndex = startIndex + i;
                if (rightIndex >= right.Length)
                {
                    return false;
                }
                //如果T是值类型,此处比较方法效率小于直接使用"=="号
                if (!comparer.Equals(left[i], right[rightIndex]))
                {
                    return false;
                }
            }
            return true;
        }

    }
}

