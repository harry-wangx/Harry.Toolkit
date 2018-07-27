using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 尝试获取字典指定key所对应的数据，如果没有，则返回默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// 尝试获取字典指定key所对应的数据，如果没有，则返回指定默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,TValue defaultValue)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
