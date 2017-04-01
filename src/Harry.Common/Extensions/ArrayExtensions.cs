﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Harry.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// 判断两个数据是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Equals<T>(this T[] left, T[] right)
        {
            if (left == right)
            {
                return true;
            }
            if (left == null || right == null)
            {
                return false;
            }
            if (left.Length != right.Length)
            {
                return false;
            }
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < left.Length; i++)
            {
                if (!comparer.Equals(left[i], right[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Contains(this IEnumerable<string> array, string value, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (array == null || array.Count() <= 0)
            {
                return false;
            }

            foreach (var item in array)
            {
                if (item.Equals(value, comparison))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

