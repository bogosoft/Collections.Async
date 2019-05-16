using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Indicates that an implementation is capable of performing queue operations asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the queued items.</typeparam>
    public interface IAsyncQueue<T> : IAsyncCollection<T>
    {
        /// <summary>
        /// Remove and return the next item in the current queue.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// An object of the queued item type.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Implementations SHOULD throw an <see cref="InvalidOperationException"/> in the event
        /// that this method is called on an empty queue.
        /// </exception>
        Task<T> DequeueAsync(CancellationToken token);

        /// <summary>
        /// Insert an item into the current queue.
        /// </summary>
        /// <param name="item">An object of the queued type.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the enqueue operation was successful.
        /// </returns>
        Task<bool> EnqueueAsync(T item, CancellationToken token);

        /// <summary>
        /// Return the next item in the current queue without removing it.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// An object of the queued item type.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Implementations SHOULD throw an <see cref="InvalidOperationException"/> in the event
        /// that this method is called on an empty queue.
        /// </exception>
        Task<T> PeekAsync(CancellationToken token);
    }

    /// <summary>
    /// Extended functionality for the <see cref="IAsyncQueue{T}"/> class.
    /// </summary>
    public static class IAsyncQueueExtensions
    {
        /// <summary>
        /// Remove and return the next item in the current queue. Calling this method is equivalent to calling
        /// <see cref="IAsyncQueue{T}.EnqueueAsync(T, CancellationToken)"/> with a value of
        /// <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the queued items.</typeparam>
        /// <param name="queue">The current <see cref="IAsyncQueue{T}"/> implementation.</param>
        /// <returns>
        /// An object of the queued item type.
        /// </returns>
        public static Task<T> DequeueAsync<T>(this IAsyncQueue<T> queue)
        {
            return queue.DequeueAsync(CancellationToken.None);
        }

        /// <summary>
        /// Insert an item into the current queue. Calling this method is equivalent to calling
        /// <see cref="IAsyncQueue{T}.EnqueueAsync(T, CancellationToken)"/> with a value of
        /// <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the queued items.</typeparam>
        /// <param name="queue">The current <see cref="IAsyncQueue{T}"/> implementation.</param>
        /// <param name="item">An object of the queued type.</param>
        /// <returns>
        /// A value indicating whether or not the enqueue operation was successful.
        /// </returns>
        public static Task<bool> EnqueueAsync<T>(this IAsyncQueue<T> queue, T item)
        {
            return queue.EnqueueAsync(item, CancellationToken.None);
        }

        /// <summary>
        /// Return the next item in the current queue without removing it. Calling this method is equivalent
        /// to calling <see cref="IAsyncQueue{T}.PeekAsync(CancellationToken)"/> with a value of
        /// <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the queued items.</typeparam>
        /// <param name="queue">The current <see cref="IAsyncQueue{T}"/> implementation.</param>
        /// <returns>
        /// An object of the queued item type.
        /// </returns>
        public static Task<T> PeekAsync<T>(this IAsyncQueue<T> queue)
        {
            return queue.PeekAsync(CancellationToken.None);
        }
    }
}