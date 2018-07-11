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
#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }


#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T To<T>(this object obj)
            where T : struct
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
            }

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }


#if COREFX || NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }
    }
}
