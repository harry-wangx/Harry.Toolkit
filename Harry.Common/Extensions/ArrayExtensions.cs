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
        public static bool Equals<T>(this T[] sourceArray, T[] destinationArray, IEqualityComparer<T> comparer = null)
        {
            //如果两个引用相等,返回true.都是null,也返回true
            if (ReferenceEquals(sourceArray, destinationArray))
            {
                return true;
            }

            //如果有一方为null,返回false
            if (sourceArray is null || destinationArray is null)
            {
                return false;
            }

            //如果数组长度不一样,也返回false
            if (sourceArray.Length != destinationArray.Length)
            {
                return false;
            }

            //挨个比较元素
            if (comparer == null) comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < sourceArray.Length; i++)
            {
                //如果T是值类型,此处比较方法效率小于直接使用"=="号
                if (!comparer.Equals(sourceArray[i], destinationArray[i]))
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
        /// <param name="sourceArray">源数组</param>
        /// <param name="destinationArray">目标数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="endIndex">结束索引</param>
        /// <returns></returns>
        public static bool Equals<T>(this T[] sourceArray, T[] destinationArray, int startIndex, int endIndex, IEqualityComparer<T> comparer = null)
        {
            Check.NotNull(sourceArray, nameof(sourceArray));
            Check.NotNull(destinationArray, nameof(destinationArray));

            if (startIndex < 0 || startIndex >= destinationArray.Length) throw new ArgumentOutOfRangeException("开始索引超范围");
            if (endIndex < 0 || endIndex >= destinationArray.Length) throw new ArgumentOutOfRangeException("结束索引超范围");
            if (endIndex < startIndex) throw new ArgumentOutOfRangeException("结束索引小于开始索引");

            //如果长度不相等,直接返回false
            if (sourceArray.Length != endIndex - startIndex + 1)
            {
                return false;
            }

            //挨个比较元素
            if (comparer == null) comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < sourceArray.Length; i++)
            {
                var rightIndex = startIndex + i;
                //if (rightIndex > endIndex)
                //{
                //    return false;
                //}
                //如果T是值类型,此处比较方法效率小于直接使用"=="号
                if (!comparer.Equals(sourceArray[i], destinationArray[rightIndex]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 源数组与目标数组指定索引处开始向后比较,元素是否都相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray">源数组</param>
        /// <param name="destinationArray">目标数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="comparer">比较器</param>
        /// <returns></returns>
        public static bool StartsFor<T>(this T[] sourceArray, T[] destinationArray, int? startIndex = null, IEqualityComparer<T> comparer = null)
        {
            Check.NotNull(sourceArray, nameof(sourceArray));
            Check.NotNull(destinationArray, nameof(destinationArray));

            if (startIndex == null)
            {
                startIndex = 0;
            }
            else if (startIndex.Value < 0)
            {
                throw new ArgumentOutOfRangeException("开始索引超范围");
            }

            //空数组直接返回False
            if (sourceArray.Length == 0 || destinationArray.Length == 0) return false;

            int endIndex = startIndex.Value + sourceArray.Length - 1;
            //如果索引超范围,直接返回False
            if (endIndex >= destinationArray.Length) return false;

            return sourceArray.Equals<T>(destinationArray, startIndex.Value, endIndex, comparer);
        }

        /// <summary>
        /// 源数组与目标数组指定索引处开始向前比较,元素是否都相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="destinationArray"></param>
        /// <param name="endIndex"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static bool EndsFor<T>(this T[] sourceArray, T[] destinationArray, int? endIndex = null, IEqualityComparer<T> comparer = null)
        {
            Check.NotNull(sourceArray, nameof(sourceArray));
            Check.NotNull(destinationArray, nameof(destinationArray));

            if (endIndex == null)
            {
                endIndex = destinationArray.Length - 1;
            }
            else if (endIndex.Value < 0)
            {
                throw new ArgumentOutOfRangeException("结束索引超范围");
            }

            //空数组直接返回False
            if (sourceArray.Length == 0 || destinationArray.Length == 0) return false;

            int startIndex = endIndex.Value - sourceArray.Length + 1;
            //如果索引超范围,直接返回False
            if (startIndex < 0 || endIndex.Value >= destinationArray.Length) return false;

            return sourceArray.Equals<T>(destinationArray, startIndex, endIndex.Value, comparer);
        }

    }
}

