#if NET20
using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqHelper
    {
        //public static IEnumerable<TResult> Cast<TResult>(IEnumerable source)
        //{
        //    foreach (object obj in source) yield return (TResult)obj;
        //}

        //public static IEnumerable<TResult> Where<TResult>(IEnumerable<TResult> source, Func<TResult, bool> predicate)
        //{
        //    foreach (TResult obj in source)
        //    {
        //        if (predicate(obj))
        //        {
        //            yield return (TResult)obj;
        //        }
        //    }
        //}

        //public static TResult[] ToArray <TResult>(IEnumerable<TResult> source)
        //{
        //    List<TResult> results = new List<TResult>();
        //    foreach (TResult obj in source)
        //    {
        //        results.Add(obj);
        //    }
        //    return results.ToArray();
        //}

        public static bool SequenceEqual<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer=null)
        {
            if (comparer == null) comparer = EqualityComparer<TSource>.Default;
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            using (IEnumerator<TSource> e1 = first.GetEnumerator())
            using (IEnumerator<TSource> e2 = second.GetEnumerator())
            {
                while (e1.MoveNext())
                {
                    if (!(e2.MoveNext() && comparer.Equals(e1.Current, e2.Current))) return false;
                }
                if (e2.MoveNext()) return false;
            }
            return true;
        }
    }


}


#endif