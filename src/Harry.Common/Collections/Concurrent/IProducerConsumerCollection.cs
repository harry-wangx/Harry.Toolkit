#if NET20 || NET35
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Harry.Collections.Concurrent
{
    public interface IProducerConsumerCollection<T> : IEnumerable<T>, ICollection
    {

        void CopyTo(T[] array, int index);

        bool TryAdd(T item);

        bool TryTake(out T item);

        T[] ToArray();

    }

    internal sealed class SystemCollectionsConcurrent_ProducerConsumerCollectionDebugView<T>
    {
        private IProducerConsumerCollection<T> m_collection; // The collection being viewed.

        /// <summary>
        /// Constructs a new debugger view object for the provided collection object.
        /// </summary>
        /// <param name="collection">A collection to browse in the debugger.</param>
        public SystemCollectionsConcurrent_ProducerConsumerCollectionDebugView(IProducerConsumerCollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            m_collection = collection;
        }

        /// <summary>
        /// Returns a snapshot of the underlying collection's elements.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get { return m_collection.ToArray(); }
        }

    }

}

#endif