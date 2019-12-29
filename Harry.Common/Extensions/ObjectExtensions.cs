using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Harry.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 等同于:obj as T;
        /// </summary>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T As<TSource, T>(this TSource obj)
            where T : class, TSource
        {
            return obj as T;
        }


        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static T To<T>(this object obj)
        //    where T : struct
        //{
        //    if (typeof(T) == typeof(Guid))
        //    {
        //        return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
        //    }

        //    return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        //}


        /// <summary>
        /// 对象是否包含于list列表中
        /// </summary>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }
    }
}
