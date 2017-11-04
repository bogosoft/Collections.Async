using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Extended functionality for the <see cref="IAsyncQueue{T}"/> contract.
    /// </summary>
    public static class IAsyncQueueExtensions
    {
        /// <summary>
        /// Clear the current queue of all of its queued items. Calling this method is equivalent to calling
        /// <see cref="IAsyncQueue{T}.ClearAsync(CancellationToken)"/> with a value of
        /// <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the queued items.</typeparam>
        /// <param name="queue">The current <see cref="IAsyncQueue{T}"/> implementation.</param>
        /// <returns>
        /// A value indicating whether or not the clear operation succeeded.
        /// </returns>
        public static Task<bool> ClearAsync<T>(this IAsyncQueue<T> queue)
        {
            return queue.ClearAsync(CancellationToken.None);
        }

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