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
        internal struct Cursor : ICursor<T>
        {
            private Queue<T>.Enumerator enumerator;

            private object @lock;

            public bool IsDisposed { get; private set; }

            public bool IsExpended { get; private set; }

            internal Cursor(Queue<T>.Enumerator enumerator)
            {
                this.enumerator = enumerator;

                @lock = new object();

                IsDisposed = false;
                IsExpended = false;
            }

            public void Dispose()
            {
                if (!IsDisposed)
                {
                    lock (@lock)
                    {
                        enumerator.Dispose();

                        IsDisposed = true;
                    }
                }
            }

            public Task<T> GetCurrentAsync(CancellationToken token)
            {
                lock (@lock)
                {
                    return Task.FromResult(enumerator.Current);
                }
            }

            public Task<bool> MoveNextAsync(CancellationToken token)
            {
                if (token.IsCancellationRequested)
                {
                    return Task.FromResult(false);
                }

                lock (@lock)
                {
                    if (IsExpended)
                    {
                        return Task.FromResult(false);
                    }

                    if (enumerator.MoveNext())
                    {
                        return Task.FromResult(true);
                    }
                    else
                    {
                        IsExpended = true;

                        return Task.FromResult(false);
                    }
                }
            }
        }

        private Queue<T> queue;

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

            lock (queue)
            {
                queue.Clear();

                return Task.FromResult(true);
            }
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

            lock (queue)
            {
                return Task.FromResult((ulong)queue.Count);
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

            lock (queue)
            {
                if (queue.Count == 0)
                {
                    throw new InvalidOperationException("Queue is empty.");
                }

                return Task.FromResult(queue.Dequeue());
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

            lock (queue)
            {
                queue.Enqueue(item);

                return Task.FromResult(true);
            }
        }

        /// <summary>
        /// Create and return a cursor.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>A readable, forward-only cursor.</returns>
        public Task<ICursor<T>> GetCursorAsync(CancellationToken token)
        {
            ICursor<T> cursor;

            if (token.IsCancellationRequested)
            {
                cursor = Cursor<T>.Empty;
            }
            else
            {
                cursor = new Cursor(queue.GetEnumerator());
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

            lock (queue)
            {
                if (queue.Count == 0)
                {
                    throw new InvalidOperationException("Queue is empty.");
                }

                return Task.FromResult(queue.Peek());
            }
        }
    }
}