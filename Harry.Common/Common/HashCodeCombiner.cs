/***************************************************
 * 此段代码来源于微软官网,地址为:
 * https://github.com/aspnet/Extensions/blob/master/src/Shared/src/HashCodeCombiner/HashCodeCombiner.cs
 ***************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Harry.Common
{
    /// <summary>
    /// 用于计算HashCode
    /// <code>
    /// var hashCodeCombiner = HashCodeCombiner.Start();
    /// hashCodeCombiner.Add(123);
    /// hashCodeCombiner.Add("xxx", StringComparer.OrdinalIgnoreCase);
    /// return hashCodeCombiner.CombinedHash;
    /// </code>
    /// </summary>
    public struct HashCodeCombiner
    {
        private long _combinedHash64;

        public int CombinedHash
        {
#if !NET40
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get { return _combinedHash64.GetHashCode(); }
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private HashCodeCombiner(long seed)
        {
            _combinedHash64 = seed;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Add(IEnumerable e)
        {
            if (e == null)
            {
                Add(0);
            }
            else
            {
                var count = 0;
                foreach (object o in e)
                {
                    Add(o);
                    count++;
                }
                Add(count);
            }
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Add<TValue>(IEnumerable<TValue> e, IEqualityComparer<TValue> comparer)
        {
            if (e == null)
            {
                Add(0);
            }
            else
            {
                var count = 0;
                foreach (var o in e)
                {
                    Add(comparer.GetHashCode(o));
                    count++;
                }
                Add(count);
            }
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static implicit operator int(HashCodeCombiner self)
        {
            return self.CombinedHash;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Add(int i)
        {
            _combinedHash64 = ((_combinedHash64 << 5) + _combinedHash64) ^ i;
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Add(string s)
        {
            var hashCode = (s != null) ? s.GetHashCode() : 0;
            Add(hashCode);
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Add(object o)
        {
            var hashCode = (o != null) ? o.GetHashCode() : 0;
            Add(hashCode);
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Add<TValue>(TValue value, IEqualityComparer<TValue> comparer)
        {
            var hashCode = value != null ? comparer.GetHashCode(value) : 0;
            Add(hashCode);
        }

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static HashCodeCombiner Start()
        {
            return new HashCodeCombiner(0x1505L);
        }
    }
}
