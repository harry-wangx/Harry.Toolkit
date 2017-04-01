#if NET35
using System;
using System.Collections;
using System.Collections.Generic;


namespace Harry.Collections.Concurrent
{

    public class ConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly object _sync = new object();
        private readonly System.Threading.ReaderWriterLockSlim m_lock_slim = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.NoRecursion);

        #region 构造函数
        /// <summary>
        /// 初始化 <see cref="Harry.Common.Collections.Concurrent.ConcurrentDictionary{TKey,TValue}"/> 类的新实例，
        /// 该实例为空且具有默认的初始容量，并使用键类型的默认相等比较器
        /// </summary>
        public ConcurrentDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 初始化 <see cref="Harry.Common.Collections.Concurrent.ConcurrentDictionary{TKey,TValue}"/> 类的新实例，
        /// 该实例为空且具有指定的初始容量，并为键类型使用默认的相等比较器。
        /// </summary>
        /// <param name="capacity"><see cref="Harry.Common.Collections.Concurrent.ConcurrentDictionary{TKey,TValue}"/> 可包含的初始元素数。</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> 小于0
        /// </exception>
        public ConcurrentDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary>
        /// 初始化<see cref="Harry.Common.Collections.Concurrent.ConcurrentDictionary{TKey,TValue}"/> 类的新实例，
        /// 该实例为空且具有默认的初始容量，并使用指定的 <see cref="System.Collections.Generic.IEqualityComparer{T}"。
        /// </summary>
        /// <param name="comparer">比较键时要使用的 <see cref="System.Collections.Generic.IEqualityComparer{T}"/> 实现，
        /// 或者为 null，以便为键类型使用默认的 <see cref="System.Collections.Generic.IEqualityComparer{T}。
        /// </param>
        public ConcurrentDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// 初始化<see cref="Harry.Common.Collections.Concurrent.ConcurrentDictionary{TKey,TValue}"/>类的新实例，
        /// 该实例为空且具有指定的初始容量，并使用指定的<see cref="System.Collections.Generic.IEqualityComparer{T}">。
        /// </summary>
        /// <param name="capacity">可包含的初始元素数</param>
        /// <param name="comparer">比较键时要使用的<see cref="System.Collections.Generic.IEqualityComparer{T}"/> 实现，
        /// 或者为 null，以便为键类型使用默认的<see cref="System.Collections.Generic.EqualityComparer{T}"/></param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> 小于0
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="comparer"/>为null</exception>
        public ConcurrentDictionary(int capacity, IEqualityComparer<TKey> comparer )
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        #endregion


        /// <summary>
        /// 尝试添加元素到 <see cref="ConcurrentDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <returns>如果添加元素到 <see cref="ConcurrentDictionary{TKey, TValue}"/>成功,则返回true,否则返回false</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为null</exception>
        /// <exception cref="T:System.OverflowException"><see cref="ConcurrentDictionary{TKey, TValue}"/>包含了太多的元素</exception>
        public bool TryAdd(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException("key");
            TValue dummy;
            return TryAddInternal(key, value, false, out dummy);
        }

        /// <summary>
        /// 检查 <see cref="ConcurrentDictionary{TKey, TValue}"/> 是否包含指定元素
        /// </summary>
        /// <returns>如果 <see cref="ConcurrentDictionary{TKey, TValue}"/>包含<paramref name="key"/>,返回true,否则返回false </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为null</exception>
        public bool ContainsKey(TKey key)
        {
            if (key == null) throw new ArgumentNullException("key");

            TValue throwAwayValue;
            return TryGetValue(key, out throwAwayValue);
        }

        /// <summary>
        /// 尝试从<see cref="ConcurrentDictionary{TKey, TValue}"/>中移除指定项
        /// </summary>
        /// <param name="value">如移除成功,返回被移除的value,否则返回<typeparamref name="TValue"/>的默认值</param>
        /// <returns>移除成功返回true,否则返回false</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为null</exception>
        public bool TryRemove(TKey key, out TValue value)
        {
            if (key == null) throw new ArgumentNullException("key");

            return TryRemoveInternal(key, out value);
        }

        /// <summary>
        /// 尝试移除元素
        /// </summary>
        private bool TryRemoveInternal(TKey key, out TValue value)
        {
            TValue tmpValue;

            m_lock_slim.EnterWriteLock();
            //先获取元素
            bool result = _dictionary.TryGetValue(key, out tmpValue);
            if (result)
            {
                //移除操作
                result = _dictionary.Remove(key);
            }

            m_lock_slim.ExitWriteLock();


            if (result)
            {
                value = tmpValue;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }

        }


        /// <summary>
        /// 尝试从<see cref="ConcurrentDictionary{TKey,TValue}"/>获取一个元素
        /// </summary>
        /// <returns>获取成功返回true,否则返回false</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为null</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            TValue tmpValue;
            //进入读锁
            m_lock_slim.EnterReadLock();

            //获取元素
            bool result = _dictionary.TryGetValue(key, out tmpValue);

            //离开读锁
            m_lock_slim.ExitReadLock();

            if (result)
            {
                value = tmpValue;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }


        /// <summary>
        /// 如果字典中已经存在<paramref name="key"/>,且对应的value值与<paramref name="comparisonValue"/>的值相等,
        /// 则将<paramref name="newValue"/>的值更新到字典中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue">用来替换与 <paramref name="key"/> 匹配的 value </param>
        /// <param name="comparisonValue">用来与<paramref name="key"/>对应的值进行比较,如果相等,则用<param name="newValue">来替换旧值.</param>
        /// <returns>如果<paramref name="comparisonValue"/> 与 <paramref name="key"/> 对应的值相等,且成功替换,返回 true ;否则返回 false </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为 null.</exception>
        public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
        {
            if (key == null) throw new ArgumentNullException("key");

            IEqualityComparer<TValue> valueComparer = EqualityComparer<TValue>.Default;

            //进入写锁
            m_lock_slim.EnterWriteLock();

            TValue tmpValue;
            var result = _dictionary.TryGetValue(key, out tmpValue);
            if (result && valueComparer.Equals(tmpValue, comparisonValue))
            {
                _dictionary[key] = newValue;
            }
            else
            {
                result = false;
            }

            //离开写锁
            m_lock_slim.ExitWriteLock();
            return result;
        }

        /// <summary>
        /// 从 <see cref="ConcurrentDictionary{TKey,TValue}"/> 中移除所有的键和值.
        /// </summary>
        public void Clear()
        {
            //进入写锁
            m_lock_slim.EnterWriteLock();

            _dictionary.Clear();

            //退出写锁
            m_lock_slim.ExitWriteLock();
        }


        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (index < 0) throw new ArgumentOutOfRangeException("index", GetResourceString("ConcurrentDictionary_IndexIsNegative"));

            //进入读锁
            m_lock_slim.EnterReadLock();

            if (array.Length - _dictionary.Count < index || _dictionary.Count < 0) //"count" itself or "count + index" can overflow
            {
                throw new ArgumentException(GetResourceString("ConcurrentDictionary_ArrayNotLargeEnough"));
            }

            CopyToPairs(array, index);

            //退出读锁
            m_lock_slim.ExitReadLock();
        }


        public KeyValuePair<TKey, TValue>[] ToArray()
        {
            //进入读锁
            m_lock_slim.EnterReadLock();

            KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[_dictionary.Count];
            CopyToPairs(array, 0);

            //退出读锁
            m_lock_slim.ExitReadLock();

            return array;
        }

        /// <summary>
        /// 注意调用此方法时要加同步锁
        /// </summary>
        private void CopyToPairs(KeyValuePair<TKey, TValue>[] array, int index)
        {
            foreach (var item in _dictionary)
            {
                array[index] = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
                index++; //this should never flow, CopyToPairs is only called when there's no overflow risk

            }
        }

        /// <summary>
        /// 注意调用此方法时要加同步锁
        /// </summary>
        private void CopyToEntries(DictionaryEntry[] array, int index)
        {
            foreach (var item in _dictionary)
            {
                array[index] = new DictionaryEntry(item.Key, item.Value);
                index++;  //this should never flow, CopyToEntries is only called when there's no overflow risk
            }
        }

        /// <summary>
        /// 注意调用此方法时要加同步锁
        /// </summary>
        private void CopyToObjects(object[] array, int index)
        {
            foreach (var item in _dictionary)
            {
                array[index] = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
                index++; //this should never flow, CopyToObjects is only called when there's no overflow risk
            }
        }

        /// <summary>
        /// 返回循环访问 <see cref="ConcurrentDictionary{TKey,TValue}"/> 的枚举数。
        /// </summary>
        /// <returns>用于 <see cref="ConcurrentDictionary{TKey,TValue}"/> <see cref="IEnumerator{KeyValuePair{TKey, TValue}}"/> 结构。</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var arrays = this.ToArray();
            foreach (var item in arrays)
            {
                yield return new KeyValuePair<TKey, TValue>(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 如果key存在, 我们总是返回 false; 如果 updateIfExists == true,则更新对应的值;
        /// 如果key不存在,我们总是添加元素,并返回true;
        /// </summary>
        /// <param name="resultingValue">如果添加/更新成功,resultingValue值为传入的value;如果是更新成功,返回的是原字典中与key对应的值</param>
        private bool TryAddInternal(TKey key, TValue value, bool updateIfExists, out TValue resultingValue)
        {
            bool result = false;
            //进入写锁
            m_lock_slim.EnterWriteLock();
            TValue tmpValue;
            try
            {
                if (_dictionary.TryGetValue(key, out tmpValue))
                {
                    if (updateIfExists)
                    {
                        _dictionary[key] = value;
                        resultingValue = value;
                    }
                    else
                    {
                        resultingValue = tmpValue;
                    }

                    result = false;
                }
                else
                {
                    //之前不存在该key
                    _dictionary.Add(key, value);
                    resultingValue = value;
                    result = true;
                }
            }
            finally
            {
                //退出写锁
                m_lock_slim.ExitWriteLock();
            }
            return result;
        }

        /// <summary>
        /// 获取或设置与指定的键相关联的值。
        /// </summary>
        /// <param name="key">要获取或设置的值的键。</param>
        /// <value>与指定的键相关联的值。如果找不到指定的键，get 操作便会引发 <see cref="System.Collections.Generic.KeyNotFoundException"/>，
        /// 而 set 操作会创建一个具有指定键的新元素。.</value>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为null .</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">已检索该属性，并且集合中不存在 <paramref name="key"/>。</exception>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!TryGetValue(key, out value))
                {
                    throw new KeyNotFoundException();
                }
                return value;
            }
            set
            {
                if (key == null) throw new ArgumentNullException("key");
                TValue dummy;
                TryAddInternal(key, value, true, out dummy);
            }
        }

        /// <summary>
        /// 获取包含在 <see cref="ConcurrentDictionary{TKey,TValue}"/> 中的键/值对的数目。
        /// </summary>
        /// <exception cref="T:System.OverflowException">字典添加了太多的元素</exception>
        /// <value>包含在 <see cref="ConcurrentDictionary{TKey,TValue}"/> 中的键/值对的数目。 </value>
        /// <remarks>Count has snapshot semantics and represents the number of items in the <see
        /// cref="ConcurrentDictionary{TKey,TValue}"/>
        /// at the moment when Count was accessed.</remarks>
        public int Count
        {
            get
            {
                //进入读锁
                m_lock_slim.EnterReadLock();

                int count = _dictionary.Count;

                //退出读锁
                m_lock_slim.ExitReadLock();
                return count;
            }
        }

        /// <summary>
        /// 获取或添加一个键/值对
        /// </summary>
        /// <param name="valueFactory">如果字典中不存在与<paramref name="key"/>对应的值,则调用此方法,生成一个新的value,并存到字典中</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为 null.</exception>
        /// <exception cref="T:System.OverflowException">字典添加元素过多.</exception>
        /// <returns>如果对应的key已存在,直接返回字典中与其对应的value;否则添加新的<paramref name="value"/>到字典中,并返回该值</returns>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (valueFactory == null) throw new ArgumentNullException("valueFactory");

            TValue resultingValue;
            if (TryGetValue(key, out resultingValue))
            {
                return resultingValue;
            }
            TryAddInternal(key, valueFactory(key), false, out resultingValue);
            return resultingValue;
        }

        /// <summary>
        /// 获取或添加一个键/值对
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为 null.</exception>
        /// <exception cref="T:System.OverflowException">字典添加元素过多.</exception>
        /// <returns>如果对应的key已存在,直接返回字典中与其对应的value;否则添加新的<paramref name="value"/>到字典中,并返回该值</returns>
        public TValue GetOrAdd(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException("key");

            TValue resultingValue;
            TryAddInternal(key, value, false, out resultingValue);
            return resultingValue;
        }


        /// <summary>
        /// 如果key不存在于字典中,添加该键值对到 <see cref="ConcurrentDictionary{TKey,TValue}"/> . 
        /// 如果key已经存在于字典中,则更新value值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="addValueFactory">此方法用于生成新的value</param>
        /// <param name="updateValueFactory">当key已经存在时,调用此方法生成新的值,用来更新.
        /// 方法参数为key和旧的value</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为 null .</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="addValueFactory"/> 为 null .</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="updateValueFactory"/> 为 null .</exception>
        /// <exception cref="T:System.OverflowException">当前字典添加元素过多.</exception>
        /// <returns>始终返回字典中与key对应的最新值.</returns>
        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (addValueFactory == null) throw new ArgumentNullException("addValueFactory");
            if (updateValueFactory == null) throw new ArgumentNullException("updateValueFactory");

            TValue newValue, resultingValue;
            while (true)
            {
                TValue oldValue;
                if (TryGetValue(key, out oldValue))
                //key exists, try to update
                {
                    newValue = updateValueFactory(key, oldValue);
                    if (TryUpdate(key, newValue, oldValue))
                    {
                        return newValue;
                    }
                }
                else //try add
                {
                    newValue = addValueFactory(key);
                    if (TryAddInternal(key, newValue, false, out resultingValue))
                    {
                        return resultingValue;
                    }
                }
            }
        }

        /// <summary>
        /// 如果key不存在于字典中,添加该键值对到 <see cref="ConcurrentDictionary{TKey,TValue}"/> .
        /// 如果key已经存在于字典中,则更新value值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="updateValueFactory">当key已经存在时,调用此方法生成新的值,用来更新.
        /// 方法参数为key和旧的value</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为 null .</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="addValueFactory"/> 为 null .</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="updateValueFactory"/> 为 null .</exception>
        /// <exception cref="T:System.OverflowException">当前字典添加元素过多.</exception>
        /// <returns>始终返回字典中与key对应的最新值.</returns>
        public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (updateValueFactory == null) throw new ArgumentNullException("updateValueFactory");
            TValue newValue, resultingValue;
            while (true)
            {
                TValue oldValue;
                if (TryGetValue(key, out oldValue))
                //key exists, try to update
                {
                    newValue = updateValueFactory(key, oldValue);
                    if (TryUpdate(key, newValue, oldValue))
                    {
                        return newValue;
                    }
                }
                else //try add
                {
                    if (TryAddInternal(key, addValue, false, out resultingValue))
                    {
                        return resultingValue;
                    }
                }
            }
        }



        /// <summary>
        /// 获取 <see cref="ConcurrentDictionary{TKey,TValue}"/> 是否为空
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                //进入读锁
                m_lock_slim.EnterReadLock();

                var result = _dictionary.Count == 0;

                //退出读锁
                m_lock_slim.ExitReadLock();
                return result;
            }
        }

        #region IDictionary<TKey,TValue> members

        /// <summary>
        /// 将指定的键和值添加到字典中。
        /// <param name="key">要添加的元素的键。</param>
        /// <param name="value">要添加的元素的值。对于引用类型，该值可以为 null。</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为 null.</exception>
        /// <exception cref="T:System.OverflowException">字典添加元素过多</exception>
        /// <exception cref="T:System.ArgumentException"><see cref="ConcurrentDictionary{TKey,TValue}"/> 中已存在具有相同键的元素。.</exception>
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            if (!TryAdd(key, value))
            {
                throw new ArgumentException(GetResourceString("ConcurrentDictionary_KeyAlreadyExisted"));
            }
        }

        /// <summary>
        /// 从 <see cref="ConcurrentDictionary{TKey, TValue}"/> 中移除所指定的键的值。
        /// </summary>
        /// <param name="key">要移除的元素的键。</param>
        /// <returns>如果成功找到并移除该元素，则为 true；否则为 false。如果在 <see cref="ConcurrentDictionary{TKey, TValue}"/>
        /// 中没有找到<paramref name="key"/>，此方法则返回 false。
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> 为 null.</exception>
        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            TValue throwAwayValue;
            return TryRemove(key, out throwAwayValue);
        }

        /// <summary>
        /// 获取包含 <see cref="ConcurrentDictionary{TKey,TValue}"/> 中的键的集合。 .
        /// </summary>
        /// <value>包含 <see cref="ConcurrentDictionary{TKey,TValue}"/> 中的键的 <see cref="ICollection{TKey}"/>。 </value>
        public ICollection<TKey> Keys
        {
            get
            {
                TKey[] keys = null;

                //进入读锁
                m_lock_slim.EnterReadLock();

                try
                {
                    keys = new TKey[_dictionary.Count];
                    _dictionary.Keys.CopyTo(keys, 0);
                }
                finally
                {
                    //退出读锁
                    m_lock_slim.ExitReadLock();
                }
                return new List<TKey>(keys);
            }
        }



        /// <summary>
        /// 获取包含 <see cref="ConcurrentDictionary{TKey,TValue}"/> 中的值的集合。 .
        /// </summary>
        /// <value包含 <see cref="ConcurrentDictionary{TKey,TValue}"/> 中的值的 <see cref="ICollection{TValue}"/>。 </value>
        public ICollection<TValue> Values
        {
            get
            {
                TValue[] values = null;
                //进入读锁
                m_lock_slim.EnterReadLock();

                try
                {
                    values = new TValue[_dictionary.Count];
                    _dictionary.Values.CopyTo(values, 0);
                }
                finally
                {
                    //退出读锁
                    m_lock_slim.ExitReadLock();
                }

                return new List<TValue>(values);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// 添加元素到 <see cref="ConcurrentDictionary{TKey, TValue}"/> 中。
        /// </summary>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            ((IDictionary<TKey, TValue>)this).Add(keyValuePair.Key, keyValuePair.Value);
        }

        /// <summary>
        /// 确定某元素是否在 <see cref="ConcurrentDictionary{TKey, TValue}"/> 中。
        /// </summary>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            TValue value;
            if (!TryGetValue(keyValuePair.Key, out value))
            {
                return false;
            }
            return EqualityComparer<TValue>.Default.Equals(value, keyValuePair.Value);
        }

        /// <summary>
        /// 获取字典是否为只读
        /// </summary>
        /// <value>只读返回true,否则反回false.  <see cref="ConcurrentDictionary{TKey,TValue}"/>始终返回false.</value>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 从字典中,移除一个<typeparamref name="keyValuePair"/> 对象
        /// </summary>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (keyValuePair.Key == null) throw new ArgumentNullException(GetResourceString("ConcurrentDictionary_ItemKeyIsNull"));

            TValue throwAwayValue;
            return TryRemoveInternal(keyValuePair.Key, out throwAwayValue);
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// 返回循环访问 <see cref="ConcurrentDictionary{TKey,TValue}"/> 的枚举数。
        /// </summary>
        /// <returns>用于 <see cref="ConcurrentDictionary{TKey,TValue}"/> 的 <see cref="IEnumerator"/>。 </returns>
        /// <remarks>
        /// 此 enumerator 是线程安全的.调用GetEnumerator()方法时,会返回一个字典的集合副本.
        /// </remarks>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ConcurrentDictionary<TKey, TValue>)this).GetEnumerator();
        }

        #endregion

        #region ICollection Members
        /// <summary>
        /// 将整个 <see cref="ConcurrentDictionary{TKey, TValue}"/>. 复制到兼容的一维数组中，从目标数组的指定索引位置开始放置。
        /// </summary>
        /// <param name="array">作为从 <see cref="ConcurrentDictionary{TKey, TValue}"/> 
        /// 复制的元素的目标位置的一维 <typeparamref name="array"/>。<typeparamref name="array"/>必须具有从零开始的索引。</param>
        /// <param name="index"><paramref name="array"/> 中从零开始的索引，从此处开始复制。 </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> 为 null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> 小于0.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="index"/> 等于或大于 array 的长度。 
        /// - 或 - 源 <see cref="ConcurrentDictionary{TKey, TValue}"/> 中的元素数目大于从 <paramref name="index"/> 到目标 
        /// <paramref name="array"/> 末尾之间的可用空间。
        ///</exception>
        void CopyTo(Array array, int index)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (index < 0) throw new ArgumentOutOfRangeException("index", GetResourceString("ConcurrentDictionary_IndexIsNegative"));

            //进入读锁
            m_lock_slim.EnterReadLock();
            try
            {
                if (array.Length - _dictionary.Count < index || _dictionary.Count < 0) //"count" itself or "count + index" can overflow
                {
                    throw new ArgumentException(GetResourceString("ConcurrentDictionary_ArrayNotLargeEnough"));
                }

                KeyValuePair<TKey, TValue>[] pairs = null;
                DictionaryEntry[] entries = null;
                object[] objects = null;

                if ((pairs = array as KeyValuePair<TKey, TValue>[]) != null)
                {
                    CopyToPairs(pairs, index);
                }
                else if ((entries = array as DictionaryEntry[]) != null)
                {
                    CopyToEntries(entries, index);
                }
                else if ((objects = array as object[]) != null)
                {
                    CopyToObjects(objects, index);
                }
                else
                {
                    throw new ArgumentException(GetResourceString("ConcurrentDictionary_ArrayIncorrectType"), "array");
                }
            }
            finally
            {
                //退出读锁
                m_lock_slim.ExitReadLock();
            }
        }

        /// <summary>
        /// 获取一个值,表示<see cref="ConcurrentDictionary{TKey, TValue}"/>是否支持使用SyncRoot属性进行同步操作.
        /// </summary>
        /// <value>支持返回true,否则返回false. <see cref="ConcurrentDictionary{TKey, TValue}"/>始终返回 false.</value>
        bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// 获取一个同步锁对象. 该字典类不支持此属性.
        /// </summary>
        /// <exception cref="System.NotSupportedException">同步锁对象属性不受支持.</exception>
        object SyncRoot
        {
            get
            {
                throw new NotSupportedException(GetResourceString("ConcurrentCollection_SyncRoot_NotSupported"));
            }
        }


        #endregion

        //偷懒,原样返回
        private static string GetResourceString(string str)
        {
            return str;
        }

    }
}

#endif