using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// A locking, in-memory implementation of the <see cref="IAsyncQueue{T}"/> contract.
    /// </summary>
    /// <typeparam name="T">The type of the queued items.</typeparam>
    public sealed class AsyncQueue<T> : IAsyncQueue<T>
    {
        struct Cursor : IAsyncEnumerator<T>
        {
            bool active;

            Queue<T>.Enumerator enumerator;

            ReaderWriterLockSlim @lock;

            internal Cursor(Queue<T> queue)
            {
                active = false;

                enumerator = queue.GetEnumerator();

                @lock = new ReaderWriterLockSlim();
            }

            public void Dispose()
            {
                @lock.EnterUpgradeableReadLock();

                try
                {
                    @lock.EnterWriteLock();

                    try
                    {
                        enumerator.Dispose();
                    }
                    finally
                    {
                        @lock.ExitWriteLock();
                    }
                }
                finally
                {
                    @lock.ExitUpgradeableReadLock();
                }
            }

            public Task<T> GetCurrentAsync(CancellationToken token)
            {
                token.ThrowIfCancellationRequested();

                @lock.EnterReadLock();

                try
                {
                    if (!active)
                    {
                        throw new InvalidOperationException(Message.CursorNotInitialized);
                    }

                    return Task.FromResult(enumerator.Current);
                }
                finally
                {
                    @lock.ExitReadLock();
                }
            }

            public Task<bool> MoveNextAsync(CancellationToken token)
            {
                if (token.IsCancellationRequested)
                {
                    return Task.FromResult(false);
                }

                @lock.EnterReadLock();

                try
                {
                    return Task.FromResult(active = enumerator.MoveNext());
                }
                finally
                {
                    @lock.ExitReadLock();
                }
            }
        }

        ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();
        Queue<T> queue;

        /// <summary>
        /// Create a new instance of the <see cref="AsyncQueue{T}"/> class.
        /// </summary>
        public AsyncQueue()
        {
            queue = new Queue<T>();
        }

        /// <summary>
        /// Create a new instance of the <see cref="AsyncQueue{T}"/> class.
        /// </summary>
        /// <param name="source">
        /// An enumerable type to be used as the initial source for the current queue.
        /// </param>
        public AsyncQueue(IEnumerable<T> source)
        {
            queue = source is Queue<T> ? source as Queue<T> : new Queue<T>(source);
        }

        /// <summary>
        /// Clear the current queue of all of its queued items.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the clear operation succeeded. This method will return false
        /// in the event that a <see cref="CancellationTokenSource"/> has requested a cancellation.
        /// </returns>
        public Task<bool> ClearAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return Task.FromResult(false);
            }

            @lock.EnterUpgradeableReadLock();

            try
            {
                if(queue.Count > 0)
                {
                    @lock.EnterWriteLock();

                    try
                    {
                        queue.Clear();
                    }
                    finally
                    {
                        @lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                @lock.ExitUpgradeableReadLock();
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Count the number of currently queue items.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value corresponding to the number of currently queued items.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// Thrown if a <see cref="CancellationTokenSource"/> has requested a cancellation.
        /// </exception>
        public Task<ulong> CountAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            @lock.EnterReadLock();

            try
            {
                return Task.FromResult((ulong)queue.Count);
            }
            finally
            {
                @lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Remove and return the next item in the current queue.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// An object of the queue type.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown in the event that this method is called on an empty queue.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown if a <see cref="CancellationTokenSource"/> has requested a cancellation.
        /// </exception>
        public Task<T> DequeueAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            @lock.EnterUpgradeableReadLock();

            try
            {
                if(queue.Count == 0)
                {
                    throw new InvalidOperationException(Message.EmptyQueue);
                }

                @lock.EnterWriteLock();

                try
                {
                    return Task.FromResult(queue.Dequeue());
                }
                finally
                {
                    @lock.ExitWriteLock();
                }
            }
            finally
            {
                @lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Insert an item into the current queue.
        /// </summary>
        /// <param name="item">An object of the queued type.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the enqueue operation was successful.
        /// </returns>
        public Task<bool> EnqueueAsync(T item, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return Task.FromResult(false);
            }

            @lock.EnterWriteLock();

            try
            {
                queue.Enqueue(item);

                return Task.FromResult(true);
            }
            finally
            {
                @lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Create and return a cursor.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>A readable, forward-only cursor.</returns>
        public Task<IAsyncEnumerator<T>> GetEnumeratorAsync(CancellationToken token)
        {
            IAsyncEnumerator<T> cursor;

            if (token.IsCancellationRequested)
            {
                cursor = Cursor<T>.Empty;
            }
            else
            {
                cursor = new Cursor(queue);
            }

            return Task.FromResult(cursor);
        }

        /// <summary>
        /// Return the next item in the current queue without removing it from the queue.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// An object of the queue type.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown in the event that this method is called on an empty queue.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown if a <see cref="CancellationTokenSource"/> has requested a cancellation.
        /// </exception>
        public Task<T> PeekAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            @lock.EnterReadLock();

            try
            {
                if (queue.Count == 0)
                {
                    throw new InvalidOperationException(Message.EmptyQueue);
                }

                return Task.FromResult(queue.Peek());
            }
            finally
            {
                @lock.ExitReadLock();
            }
        }
    }
}