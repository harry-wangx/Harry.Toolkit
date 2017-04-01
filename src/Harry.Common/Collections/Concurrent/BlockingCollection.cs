/*
 * 原地址 https://referencesource.microsoft.com/#System/sys/system/collections/concurrent/BlockingCollection.cs
 */
#if NET35
using Harry.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace Harry.Collections.Concurrent
{
    /// <summary> 
    /// Provides blocking and bounding capabilities for thread-safe collections that 
    /// implement <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}"/>. 
    /// </summary>
    /// <remarks>
    /// <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}"/> represents a collection
    /// that allows for thread-safe adding and removing of data. 
    /// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> is used as a wrapper
    /// for an <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}"/> instance, allowing
    /// removal attempts from the collection to block until data is available to be removed.  Similarly,
    /// a <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> can be created to enforce
    /// an upper-bound on the number of data elements allowed in the 
    /// <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}"/>; addition attempts to the
    /// collection may then block until space is available to store the added items.  In this manner,
    /// <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> is similar to a traditional
    /// blocking queue data structure, except that the underlying data storage mechanism is abstracted
    /// away as an <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection{T}"/>. 
    /// </remarks>
    /// <typeparam name="T">Specifies the type of elements in the collection.</typeparam>
    [ComVisible(false)]
#if !FEATURE_NETCORE
#pragma warning disable 0618
    [HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
#pragma warning restore 0618
#endif
    [DebuggerTypeProxy(typeof(SystemThreadingCollections_BlockingCollectionDebugView<>))]
    [DebuggerDisplay("Count = {Count}, Type = {m_collection}")]
    public class BlockingCollection<T> : IEnumerable<T>, ICollection, IDisposable//, IReadOnlyCollection<T>
    {
        private IProducerConsumerCollection<T> m_collection;
        private int m_boundedCapacity;
        private const int NON_BOUNDED = -1;
        private SemaphoreSlim m_freeNodes;
        private SemaphoreSlim m_occupiedNodes;
        private bool m_isDisposed;

        private volatile int m_currentAdders;
        private const int COMPLETE_ADDING_ON_MASK = unchecked((int)0x80000000);

        private volatile bool m_ConsumersCanceled = false;
        private volatile bool m_ProducersCanceled = false;
        #region Properties

        /// <summary>获取最大容量值</summary>
        public int BoundedCapacity
        {
            get
            {
                CheckDisposed();
                return m_boundedCapacity;
            }
        }

        /// <summary>
        /// 获取添加操作是否完成
        /// </summary>
        public bool IsAddingCompleted
        {
            get
            {
                CheckDisposed();
                return (m_currentAdders == COMPLETE_ADDING_ON_MASK);
            }
        }

        /// <summary>
        /// 获取添加操作是否完成,且集合为空
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                CheckDisposed();
                return (IsAddingCompleted && (m_occupiedNodes.CurrentCount == 0));
            }
        }

        /// <summary>
        /// 获取集合中元素的数量
        /// </summary>
        public int Count
        {
            get
            {
                CheckDisposed();
                return m_occupiedNodes.CurrentCount;
            }
        }

        /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized.</summary>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        bool ICollection.IsSynchronized
        {
            get
            {
                CheckDisposed();
                return false;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see
        /// cref="T:System.Collections.ICollection"/>. This property is not supported.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The SyncRoot property is not supported.</exception>
        object ICollection.SyncRoot
        {
            get
            {
                throw new NotSupportedException(GetString("ConcurrentCollection_SyncRoot_NotSupported"));
            }
        }


        #endregion


        #region 构造函数
        /// <summary>初始化一个
        /// <see cref="T:Harry.Collections.Concurrent.BlockingCollection{T}"/>
        /// 实例(无上限)
        /// </summary>
        /// <remarks>
        /// 默认使用 <see cref="Harry.Collections.Concurrent.ConcurrentQueue{T}">ConcurrentQueue&lt;T&gt;</see>.
        /// </remarks>
        public BlockingCollection()
            : this(new ConcurrentQueue<T>())
        {
        }


        /// <summary>
        /// 初始化一个<see cref="T:Harry.Collections.Concurrent.BlockingCollection{T}"/>实例
        /// </summary>
        /// <param name="boundedCapacity">集合元素数量上限</param>
        /// <remarks>
        /// 默认使用 <see cref="Harry.Collections.Concurrent.ConcurrentQueue{T}">ConcurrentQueue&lt;T&gt;</see>.
        /// </remarks>
        public BlockingCollection(int boundedCapacity)
            : this(new ConcurrentQueue<T>(), boundedCapacity)
        {
        }

        /// <summary>
        /// 初始化一个<see cref="T:Harry.Collections.Concurrent.BlockingCollection{T}"/>实例
        /// </summary>
        /// <param name="collection">默认集合</param>
        /// <param name="boundedCapacity">集合元素数量上限</param>
        public BlockingCollection(IProducerConsumerCollection<T> collection, int boundedCapacity)
        {
            if (boundedCapacity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    "boundedCapacity", boundedCapacity,
                    GetString("BlockingCollection_ctor_BoundedCapacityRange"));
            }
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            int count = collection.Count;
            if (count > boundedCapacity)
            {
                throw new ArgumentException(GetString("BlockingCollection_ctor_CountMoreThanCapacity"));
            }
            Initialize(collection, boundedCapacity, count);
        }

        /// <summary>
        /// 初始化一个<see cref="T:Harry.Collections.Concurrent.BlockingCollection{T}"/>实例(无上限)
        /// </summary>
        /// <param name="collection">默认集合</param>
        public BlockingCollection(IProducerConsumerCollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            Initialize(collection, NON_BOUNDED, collection.Count);
        }
        #endregion

        /// <summary>
        /// 初始化实例
        /// </summary>
        private void Initialize(IProducerConsumerCollection<T> collection, int boundedCapacity, int collectionCount)
        {
            Debug.Assert(boundedCapacity > 0 || boundedCapacity == NON_BOUNDED);

            m_collection = collection;
            m_boundedCapacity = boundedCapacity; ;
            m_isDisposed = false;

            if (boundedCapacity == NON_BOUNDED)
            {
                m_freeNodes = null;
            }
            else
            {
                Debug.Assert(boundedCapacity > 0);
                m_freeNodes = new SemaphoreSlim(boundedCapacity - collectionCount);
            }


            m_occupiedNodes = new SemaphoreSlim(collectionCount);
        }


        /// <summary>
        /// 添加元素
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">当前对象已释放</exception>
        /// <remarks>
        /// 如果指定了元素数量上限,调用此方法时,线程有可能会被塞,直到有可用空间为止
        /// </remarks>
        public void Add(T item)
        {
#if DEBUG
            bool tryAddReturnValue =
#endif
            TryAddWithNoTimeValidation(item, Timeout.Infinite, null);
#if DEBUG
            Debug.Assert(tryAddReturnValue, "TryAdd() was expected to return true.");
#endif
        }


        /// <summary>
        /// 添加元素
        /// </summary>
        /// <exception cref="OperationCanceledException">如果中途<paramref name="isCanceled"/> 返回true,则会停止等待,并抛出此异常</exception>
        /// <exception cref="T:System.ObjectDisposedException">当前对象已释放</exception>
        /// <remarks>
        /// 如果指定了元素数量上限,调用此方法时,线程有可能会被塞,直到有可用空间为止
        /// </remarks>
        public void Add(T item, Func<bool> isCanceled)
        {
#if DEBUG
            bool tryAddReturnValue =
#endif
            TryAddWithNoTimeValidation(item, Timeout.Infinite, isCanceled);
#if DEBUG
            Debug.Assert(tryAddReturnValue, "TryAdd() was expected to return true.");
#endif
        }


        /// <summary>
        /// 偿试添加元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns>添加成功,返回true;否则返回false</returns>
        public bool TryAdd(T item)
        {
            return TryAddWithNoTimeValidation(item, 0, null);
        }

        /// <summary>
        /// 偿试添加元素
        /// </summary>
        /// <param name="item"></param>
        /// <param name="timeout">过期时间</param>
        /// <returns>添加成功,返回true;否则返回false</returns>
        public bool TryAdd(T item, TimeSpan timeout)
        {
            ValidateTimeout(timeout);
            return TryAddWithNoTimeValidation(item, (int)timeout.TotalMilliseconds, null);
        }

        /// <summary>
        /// 偿试添加元素
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout">超时时间(单位:毫秒)</param>
        /// <returns>添加成功,返回true;否则返回false</returns>
        public bool TryAdd(T item, int millisecondsTimeout)
        {
            ValidateMillisecondsTimeout(millisecondsTimeout);
            return TryAddWithNoTimeValidation(item, millisecondsTimeout, null);
        }

        /// <summary>
        /// 偿试添加元素
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout">超时时间(单位:毫秒)</param>
        /// <param name="isCanceled"></param>
        /// <exception cref="OperationCanceledException">如果中途<paramref name="isCanceled"/> 返回true,则会停止等待,并抛出此异常</exception>
        /// <returns>添加成功,返回true;否则返回false</returns>
        public bool TryAdd(T item, int millisecondsTimeout, Func<bool> isCanceled)
        {
            ValidateMillisecondsTimeout(millisecondsTimeout);
            return TryAddWithNoTimeValidation(item, millisecondsTimeout, isCanceled);
        }


        /// <summary>
        /// 偿试添加一个元素到集合中.如果设置了集合元素数量上限,且集合数量已达到上限,线程会被阻塞.
        /// 如果超过millisecondsTimeout所设定的时限,还没能成功添加元素到集合,返回false.
        /// </summary>
        /// <exception cref="OperationCanceledException">如果中途<paramref name="isCanceled"/> 返回true,则会停止等待,并抛出此异常</exception>
        /// <exception cref="T:System.ObjectDisposedException">当前对象已释放</exception>
        /// <returns>添加成功,返回true;否则返回false</returns>
        private bool TryAddWithNoTimeValidation(T item, int millisecondsTimeout, Func<bool> isCanceled)
        {
            CheckDisposed();

            if (isCanceled != null && isCanceled())
                throw new OperationCanceledException(GetString("Common_OperationCanceled"));

            if (IsAddingCompleted)
            {
                throw new InvalidOperationException(GetString("BlockingCollection_Completed"));
            }

            bool waitForSemaphoreWasSuccessful = true;

            if (m_freeNodes != null)
            {
                //If the m_freeNodes semaphore threw OperationCanceledException then this means that CompleteAdding()
                //was called concurrently with Adding which is not supported by BlockingCollection.

                //先不等待,如返回true,说明元素数量未达上限,继续执行;否则再次调用Wait方法,等待一段时间
                waitForSemaphoreWasSuccessful = m_freeNodes.Wait(0);
                if (waitForSemaphoreWasSuccessful == false && millisecondsTimeout != 0)
                {
                    waitForSemaphoreWasSuccessful = m_freeNodes.Wait(millisecondsTimeout, () => m_ProducersCanceled || (isCanceled != null && isCanceled()));
                }
            }
            if (waitForSemaphoreWasSuccessful)
            {
                // Update the adders count if the complete adding was not requested, otherwise
                // spins until all adders finish then throw IOE
                // The idea behind to spin untill all adders finish, is to avoid to return to the caller with IOE while there are still some adders have
                // not been finished yet
                SpinWait spinner = new SpinWait();
                while (true)
                {
                    int observedAdders = m_currentAdders;
                    if ((observedAdders & COMPLETE_ADDING_ON_MASK) != 0)
                    {
                        spinner.Reset();
                        // CompleteAdding is requested, spin then throw
                        while (m_currentAdders != COMPLETE_ADDING_ON_MASK) spinner.SpinOnce();
                        throw new InvalidOperationException(GetString("BlockingCollection_Completed"));
                    }
                    if (Interlocked.CompareExchange(ref m_currentAdders, observedAdders + 1, observedAdders) == observedAdders)
                    {
                        Debug.Assert((observedAdders + 1) <= (~COMPLETE_ADDING_ON_MASK), "The number of concurrent adders thread excceeded the maximum limit.");
                        break;
                    }
                    spinner.SpinOnce();
                }

                // This outer try/finally to workaround of repeating the decrement adders code 3 times, because we should decrement the adders if:
                // 1- m_collection.TryAdd threw an exception
                // 2- m_collection.TryAdd succeeded
                // 3- m_collection.TryAdd returned false
                // so we put the decrement code in the finally block
                try
                {

                    //TryAdd is guaranteed to find a place to add the element. Its return value depends
                    //on the semantics of the underlying store. Some underlying stores will not add an already
                    //existing item and thus TryAdd returns false indicating that the size of the underlying
                    //store did not increase.


                    bool addingSucceeded = false;
                    try
                    {
                        //The token may have been canceled before the collection had space available, so we need a check after the wait has completed.
                        //This fixes 
                        //cancellationToken.ThrowIfCancellationRequested();
                        if (isCanceled != null && isCanceled())
                        {
                            throw new OperationCanceledException();
                        }
                        addingSucceeded = m_collection.TryAdd(item);
                    }
                    catch
                    {
                        //TryAdd did not result in increasing the size of the underlying store and hence we need
                        //to increment back the count of the m_freeNodes semaphore.
                        if (m_freeNodes != null)
                        {
                            m_freeNodes.Release();
                        }
                        throw;
                    }
                    if (addingSucceeded)
                    {
                        //After adding an element to the underlying storage, signal to the consumers 
                        //waiting on m_occupiedNodes that there is a new item added ready to be consumed.
                        m_occupiedNodes.Release();
                    }
                    else
                    {
                        throw new InvalidOperationException(GetString("BlockingCollection_Add_Failed"));
                    }
                }
                finally
                {
                    // decrement the adders count
                    Debug.Assert((m_currentAdders & ~COMPLETE_ADDING_ON_MASK) > 0);
                    Interlocked.Decrement(ref m_currentAdders);
                }


            }
            return waitForSemaphoreWasSuccessful;
        }

        /// <summary>取出一个元素</summary>
        /// <returns></returns>
        /// <exception cref="T:System.OperationCanceledException">The <see
        /// cref="T:Harry.Collections.Concurrent.BlockingCollection{T}"/> is empty and has been marked
        /// as complete with regards to additions.</exception>
        /// <exception cref="T:System.ObjectDisposedException">当前对象 <see
        /// cref="T:Harry.Collections.Concurrent.BlockingCollection{T}"/> 已释放.</exception>
        /// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
        /// outside of this <see
        /// cref="T:Harry.Collections.Concurrent.BlockingCollection{T}"/> instance.</exception>
        /// <remarks>A call to <see cref="Take()"/> may block until an item is available to be removed.</remarks>
        public T Take()
        {
            T item;

            if (!TryTake(out item, Timeout.Infinite, null))
            {
                throw new InvalidOperationException(GetString("BlockingCollection_CantTakeWhenDone"));
            }

            return item;
        }

        /// <summary>Takes an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/>.</summary>
        /// <returns>The item removed from the collection.</returns>
        /// <exception cref="T:System.OperationCanceledException">If the <see cref="CancellationToken"/> is
        /// canceled or the <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> is empty and has been marked
        /// as complete with regards to additions.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        /// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
        /// outside of this <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance.</exception>
        /// <remarks>A call to <see cref="Take(CancellationToken)"/> may block until an item is available to be removed.</remarks>
        public T Take(Func<bool> isCanceled)
        {
            T item;

            if (!TryTake(out item, Timeout.Infinite, isCanceled))
            {
                throw new InvalidOperationException(GetString("BlockingCollection_CantTakeWhenDone"));
            }

            return item;
        }

        /// <summary>
        /// Attempts to remove an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/>.
        /// </summary>
        /// <param name="item">The item removed from the collection.</param>
        /// <returns>true if an item could be removed; otherwise, false.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        /// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
        /// outside of this <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance.</exception>
        public bool TryTake(out T item)
        {
            return TryTake(out item, 0, null);
        }

        /// <summary>
        /// Attempts to remove an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/>.
        /// </summary>
        /// <param name="item">The item removed from the collection.</param>
        /// <param name="timeout">A <see cref="System.TimeSpan"/> that represents the number of milliseconds
        /// to wait, or a <see cref="System.TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
        /// </param>
        /// <returns>true if an item could be removed from the collection within 
        /// the alloted time; otherwise, false.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout"/> is a negative number
        /// other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than
        /// <see cref="System.Int32.MaxValue"/>.</exception>
        /// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
        /// outside of this <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance.</exception>
        public bool TryTake(out T item, TimeSpan timeout)
        {
            ValidateTimeout(timeout);
            return TryTakeWithNoTimeValidation(out item, (int)timeout.TotalMilliseconds, null);
        }

        /// <summary>
        /// Attempts to remove an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/>.
        /// </summary>
        /// <param name="item">The item removed from the collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see
        /// cref="System.Threading.Timeout.Infinite"/> (-1) to wait indefinitely.</param>
        /// <returns>true if an item could be removed from the collection within 
        /// the alloted time; otherwise, false.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is a
        /// negative number other than -1, which represents an infinite time-out.</exception>
        /// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
        /// outside of this <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance.</exception>
        public bool TryTake(out T item, int millisecondsTimeout)
        {
            ValidateMillisecondsTimeout(millisecondsTimeout);
            return TryTakeWithNoTimeValidation(out item, millisecondsTimeout, null);
        }

        /// <summary>
        /// Attempts to remove an item from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/>.
        /// A <see cref="System.OperationCanceledException"/> is thrown if the <see cref="CancellationToken"/> is
        /// canceled.
        /// </summary>
        /// <param name="item">The item removed from the collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see
        /// cref="System.Threading.Timeout.Infinite"/> (-1) to wait indefinitely.</param>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>true if an item could be removed from the collection within 
        /// the alloted time; otherwise, false.</returns>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken"/> is canceled.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is a
        /// negative number other than -1, which represents an infinite time-out.</exception>
        /// <exception cref="T:System.InvalidOperationException">The underlying collection was modified
        /// outside of this <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance.</exception>
        public bool TryTake(out T item, int millisecondsTimeout, Func<bool> isCanceled)
        {
            ValidateMillisecondsTimeout(millisecondsTimeout);
            return TryTakeWithNoTimeValidation(out item, millisecondsTimeout, isCanceled);
        }

        /// <summary>Takes an item from the underlying data store using its IProducerConsumerCollection&lt;T&gt;.Take 
        /// method. If the collection was empty, this method will wait for, at most, the timeout period (if AddingIsCompleted is false)
        /// trying to remove an item. If the timeout period was exhaused before successfully removing an item 
        /// this method will return false.
        /// A <see cref="System.OperationCanceledException"/> is thrown if the <see cref="CancellationToken"/> is
        /// canceled.
        /// </summary>
        /// <param name="item">The item removed from the collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the collection to have an item available 
        /// for removal, or Timeout.Infinite to wait indefinitely.</param>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <param name="combinedTokenSource">A combined cancellation token if created, it is only created by GetConsumingEnumerable to avoid creating the linked token 
        /// multiple times.</param>
        /// <returns>False if the collection remained empty till the timeout period was exhausted. True otherwise.</returns>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken"/> is canceled.</exception>
        /// <exception cref="System.ObjectDisposedException">If the collection has been disposed.</exception>
        private bool TryTakeWithNoTimeValidation(out T item, int millisecondsTimeout, Func<bool> isCanceled)
        {
            CheckDisposed();
            item = default(T);

            if (isCanceled != null && isCanceled())
                throw new OperationCanceledException(GetString("Common_OperationCanceled"));

            //If the collection is completed then there is no need to wait.
            if (IsCompleted)
            {
                return false;
            }
            bool waitForSemaphoreWasSuccessful = false;


            try
            {
                waitForSemaphoreWasSuccessful = m_occupiedNodes.Wait(0);
                if (waitForSemaphoreWasSuccessful == false && millisecondsTimeout != 0)
                {
                    waitForSemaphoreWasSuccessful = m_occupiedNodes.Wait(millisecondsTimeout, () => m_ConsumersCanceled || (isCanceled != null && isCanceled()));
                }
            }
            //The collection became completed while waiting on the semaphore.
            catch (OperationCanceledException)
            {
                if (isCanceled != null && isCanceled())
                    throw new OperationCanceledException(GetString("Common_OperationCanceled"));

                return false;
            }


            if (waitForSemaphoreWasSuccessful)
            {
                bool removeSucceeded = false;
                bool removeFaulted = true;
                try
                {
                    //The token may have been canceled before an item arrived, so we need a check after the wait has completed.
                    //This fixes 
                    //cancellationToken.ThrowIfCancellationRequested();
                    if (isCanceled != null && isCanceled())
                        throw new OperationCanceledException(GetString("Common_OperationCanceled"));

                    //If an item was successfully removed from the underlying collection.
                    removeSucceeded = m_collection.TryTake(out item);
                    removeFaulted = false;
                    if (!removeSucceeded)
                    {
                        // Check if the collection is empty which means that the collection was modified outside BlockingCollection
                        throw new InvalidOperationException
                            (GetString("BlockingCollection_Take_CollectionModified"));
                    }
                }
                finally
                {
                    // removeFaulted implies !removeSucceeded, but the reverse is not true.
                    if (removeSucceeded)
                    {
                        if (m_freeNodes != null)
                        {
                            Debug.Assert(m_boundedCapacity != NON_BOUNDED);
                            m_freeNodes.Release();
                        }
                    }
                    else if (removeFaulted)
                    {
                        m_occupiedNodes.Release();
                    }
                    //Last remover will detect that it has actually removed the last item from the 
                    //collection and that CompleteAdding() was called previously. Thus, it will cancel the semaphores
                    //so that any thread waiting on them wakes up. Note several threads may call CancelWaitingConsumers
                    //but this is not a problem.
                    if (IsCompleted)
                    {
                        CancelWaitingConsumers();
                    }
                }
            }
            return waitForSemaphoreWasSuccessful;
        }



        /// <summary>
        /// Marks the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instances
        /// as not accepting any more additions.  
        /// </summary>
        /// <remarks>
        /// After a collection has been marked as complete for adding, adding to the collection is not permitted 
        /// and attempts to remove from the collection will not wait when the collection is empty.
        /// </remarks>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        public void CompleteAdding()
        {
            CheckDisposed();

            if (IsAddingCompleted)
                return;

            SpinWait spinner = new SpinWait();
            while (true)
            {
                int observedAdders = m_currentAdders;
                if ((observedAdders & COMPLETE_ADDING_ON_MASK) != 0)
                {
                    spinner.Reset();
                    // If there is another COmpleteAdding in progress waiting the current adders, then spin until it finishes
                    while (m_currentAdders != COMPLETE_ADDING_ON_MASK) spinner.SpinOnce();
                    return;
                }

                if (Interlocked.CompareExchange(ref m_currentAdders, observedAdders | COMPLETE_ADDING_ON_MASK, observedAdders) == observedAdders)
                {
                    spinner.Reset();
                    while (m_currentAdders != COMPLETE_ADDING_ON_MASK) spinner.SpinOnce();

                    if (Count == 0)
                    {
                        CancelWaitingConsumers();
                    }

                    // We should always wake waiting producers, and have them throw exceptions as
                    // Add&CompleteAdding should not be used concurrently.
                    CancelWaitingProducers();
                    return;

                }
                spinner.SpinOnce();
            }
        }

        /// <summary>Cancels the semaphores.</summary>
        private void CancelWaitingConsumers()
        {
            m_ConsumersCanceled = true;
        }

        private void CancelWaitingProducers()
        {
            m_ProducersCanceled = true;
        }


        /// <summary>
        /// Releases resources used by the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources used by the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance.
        /// </summary>
        /// <param name="disposing">Whether being disposed explicitly (true) or due to a finalizer (false).</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_isDisposed)
            {
                if (m_freeNodes != null)
                {
                    m_freeNodes.Dispose();
                }

                m_occupiedNodes.Dispose();

                m_isDisposed = true;
            }
        }

        /// <summary>Copies the items from the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance into a new array.</summary>
        /// <returns>An array containing copies of the elements of the collection.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        /// <remarks>
        /// The copied elements are not removed from the collection.
        /// </remarks>
        public T[] ToArray()
        {
            CheckDisposed();
            return m_collection.ToArray();
        }

        /// <summary>Copies all of the items in the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance 
        /// to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from 
        /// the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance. The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="array"/> argument is
        /// null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> argument is less than zero.</exception>
        /// <exception cref="System.ArgumentException">The <paramref name="index"/> argument is equal to or greater 
        /// than the length of the <paramref name="array"/>.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        public void CopyTo(T[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        /// <summary>Copies all of the items in the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance 
        /// to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from 
        /// the <see cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> instance. The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="array"/> argument is
        /// null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> argument is less than zero.</exception>
        /// <exception cref="System.ArgumentException">The <paramref name="index"/> argument is equal to or greater 
        /// than the length of the <paramref name="array"/>, the array is multidimensional, or the type parameter for the collection 
        /// cannot be cast automatically to the type of the destination array.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        void ICollection.CopyTo(Array array, int index)
        {
            CheckDisposed();

            //We don't call m_collection.CopyTo() directly because we rely on Array.Copy method to customize 
            //all array exceptions.  
            T[] collectionSnapShot = m_collection.ToArray();

            try
            {
                Array.Copy(collectionSnapShot, 0, array, index, collectionSnapShot.Length);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("array");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException("index", index, GetString("BlockingCollection_CopyTo_NonNegative"));
            }
            catch (ArgumentException)
            {
                throw new ArgumentException(GetString("BlockingCollection_CopyTo_TooManyElems"), "index");
            }
            catch (RankException)
            {
                throw new ArgumentException(GetString("BlockingCollection_CopyTo_MultiDim"), "array");
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(GetString("BlockingCollection_CopyTo_IncorrectType"), "array");
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException(GetString("BlockingCollection_CopyTo_IncorrectType"), "array");
            }
        }

        /// <summary>Provides a consuming <see cref="T:System.Collections.Generics.IEnumerable{T}"/> for items in the collection.</summary>
        /// <returns>An <see cref="T:System.Collections.Generics.IEnumerable{T}"/> that removes and returns items from the collection.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        public IEnumerable<T> GetConsumingEnumerable()
        {
            return GetConsumingEnumerable(null);
        }

        /// <summary>Provides a consuming <see cref="T:System.Collections.Generics.IEnumerable{T}"/> for items in the collection.
        /// Calling MoveNext on the returned enumerable will block if there is no data available, or will
        /// throw an <see cref="System.OperationCanceledException"/> if the <see cref="CancellationToken"/> is canceled.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>An <see cref="T:System.Collections.Generics.IEnumerable{T}"/> that removes and returns items from the collection.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken"/> is canceled.</exception>
        public IEnumerable<T> GetConsumingEnumerable(Func<bool> isCanceled)
        {

            while (!IsCompleted)
            {
                T item;
                if (TryTakeWithNoTimeValidation(out item, Timeout.Infinite, isCanceled))
                {
                    yield return item;
                }
            }

        }

        /// <summary>Provides an <see cref="T:System.Collections.Generics.IEnumerator{T}"/> for items in the collection.</summary>
        /// <returns>An <see cref="T:System.Collections.Generics.IEnumerator{T}"/> for the items in the collection.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            CheckDisposed();
            return m_collection.GetEnumerator();

        }

        /// <summary>Provides an <see cref="T:System.Collections.IEnumerator"/> for items in the collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> for the items in the collection.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see
        /// cref="T:System.Collections.Concurrent.BlockingCollection{T}"/> has been disposed.</exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        /// <summary>Centralizes the logic for validating the BlockingCollections array passed to TryAddToAny()
        /// and TryTakeFromAny().</summary>
        /// <param name="collections">The collections to/from which an item should be added/removed.</param>
        /// <param name="operationMode">Indicates whether this method is called to Add or Take.</param>
        /// <exception cref="System.ArgumentNullException">If the collections argument is null.</exception>
        /// <exception cref="System.ArgumentException">If the collections argument is a 0-length array or contains a 
        /// null element. Also, if at least one of the collections has been marked complete for adds.</exception>
        /// <exception cref="System.ObjectDisposedException">If at least one of the collections has been disposed.</exception>
        private static void ValidateCollectionsArray(BlockingCollection<T>[] collections, bool isAddOperation)
        {
            if (collections == null)
            {
                throw new ArgumentNullException("collections");
            }
            else if (collections.Length < 1)
            {
                throw new ArgumentException(
                    GetString("BlockingCollection_ValidateCollectionsArray_ZeroSize"), "collections");
            }
            else if ((!IsSTAThread && collections.Length > 63) || (IsSTAThread && collections.Length > 62))
            //The number of WaitHandles must be <= 64 for MTA, and <=63 for STA, and we reserve one for CancellationToken                
            {
                throw new ArgumentOutOfRangeException(
                    "collections", GetString("BlockingCollection_ValidateCollectionsArray_LargeSize"));
            }

            for (int i = 0; i < collections.Length; ++i)
            {
                if (collections[i] == null)
                {
                    throw new ArgumentException(
                        GetString("BlockingCollection_ValidateCollectionsArray_NullElems"), "collections");
                }

                if (collections[i].m_isDisposed)
                    throw new ObjectDisposedException(
                        "collections", GetString("BlockingCollection_ValidateCollectionsArray_DispElems"));

                if (isAddOperation && collections[i].IsAddingCompleted)
                {
                    throw new ArgumentException(
                        GetString("BlockingCollection_CantAddAnyWhenCompleted"), "collections");
                }
            }
        }

        private static bool IsSTAThread
        {
            get
            {
                return Thread.CurrentThread.GetApartmentState() == ApartmentState.STA;
            }
        }


        private static void ValidateTimeout(TimeSpan timeout)
        {
            long totalMilliseconds = (long)timeout.TotalMilliseconds;
            if ((totalMilliseconds < 0 || totalMilliseconds > Int32.MaxValue) && (totalMilliseconds != Timeout.Infinite))
            {
                throw new ArgumentOutOfRangeException("timeout", timeout,
                    String.Format(CultureInfo.InvariantCulture, GetString("BlockingCollection_TimeoutInvalid"), Int32.MaxValue));
            }
        }

        /// <summary>Centralizes the logic of validating the millisecondsTimeout input argument.</summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for to successfully complete an 
        /// operation on the collection.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">If the number of millseconds is less than 0 and not 
        /// equal to Timeout.Infinite.</exception>
        private static void ValidateMillisecondsTimeout(int millisecondsTimeout)
        {
            if ((millisecondsTimeout < 0) && (millisecondsTimeout != Timeout.Infinite))
            {
                throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout,
                    String.Format(CultureInfo.InvariantCulture, GetString("BlockingCollection_TimeoutInvalid"), Int32.MaxValue));
            }
        }

        /// <summary>Throws a System.ObjectDisposedException if the collection was disposed</summary>
        /// <exception cref="System.ObjectDisposedException">If the collection has been disposed.</exception>
        private void CheckDisposed()
        {
            if (m_isDisposed)
            {
                throw new ObjectDisposedException("BlockingCollection", GetString("BlockingCollection_Disposed"));
            }
        }

        private static string GetString(string str)
        {
            return str;
        }
    }

    /// <summary>A debugger view of the blocking collection that makes it simple to browse the
    /// collection's contents at a point in time.</summary>
    /// <typeparam name="T">The type of element that the BlockingCollection will hold.</typeparam>
    internal sealed class SystemThreadingCollections_BlockingCollectionDebugView<T>
    {
        private BlockingCollection<T> m_blockingCollection; // The collection being viewed.

        /// <summary>Constructs a new debugger view object for the provided blocking collection object.</summary>
        /// <param name="collection">A blocking collection to browse in the debugger.</param>
        public SystemThreadingCollections_BlockingCollectionDebugView(BlockingCollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            m_blockingCollection = collection;
        }

        /// <summary>Returns a snapshot of the underlying collection's elements.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                return m_blockingCollection.ToArray();
            }
        }

    }
}

#endif