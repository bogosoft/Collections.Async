using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Extended functionality for the <see cref="IAsyncQueue{T}"/> contract.
    /// </summary>
    public static class QueueExtensions
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
        public static async Task<bool> ClearAsync<T>(this IAsyncQueue<T> queue)
        {
            return await queue.ClearAsync(CancellationToken.None).ConfigureAwait(false);
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
        public static async Task<T> DequeueAsync<T>(this IAsyncQueue<T> queue)
        {
            return await queue.DequeueAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Insert an item into the current queue.
        /// </summary>
        /// <typeparam name="T">The type of the queued items.</typeparam>
        /// <param name="queue">The current <see cref="IAsyncQueue{T}"/> implementation.</param>
        /// <param name="item">An object of the queued type.</param>
        /// <returns>
        /// A value indicating whether or not the enqueue operation was successful.
        /// </returns>
        public static async Task<bool> EnqueueAsync<T>(this IAsyncQueue<T> queue, T item)
        {
            return await queue.EnqueueAsync(item, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Return the next item in the current queue without removing it.
        /// </summary>
        /// <typeparam name="T">The type of the queued items.</typeparam>
        /// <param name="queue">The current <see cref="IAsyncQueue{T}"/> implementation.</param>
        /// <returns>
        /// An object of the queued item type.
        /// </returns>
        public static async Task<T> PeekAsync<T>(this IAsyncQueue<T> queue)
        {
            return await queue.PeekAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}