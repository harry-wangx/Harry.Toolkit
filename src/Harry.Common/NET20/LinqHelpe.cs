#if NET20
using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqHelper
    {
        public static IEnumerable<TResult> Cast<TResult>(IEnumerable source)
        {
            foreach (object obj in source) yield return (TResult)obj;
        }

        public static IEnumerable<TResult> Where<TResult>(IEnumerable<TResult> source, Func<TResult, bool> predicate)
        {
            foreach (TResult obj in source)
            {
                if (predicate(obj))
                {
                    yield return (TResult)obj;
                }
            }
        }

        public static TResult[] ToArray <TResult>(IEnumerable<TResult> source)
        {
            List<TResult> results = new List<TResult>();
            foreach (TResult obj in source)
            {
                results.Add(obj);
            }
            return results.ToArray();
        }
    }
}

#endif