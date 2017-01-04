using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Indicates that an implementation is capable of performing queue operations asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the queued items.</typeparam>
    public interface IAsyncQueue<T> : ITraversable<T>
    {
        /// <summary>
        /// Clear the current queue of all of its queued items.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the clear operation succeeded.
        /// </returns>
        Task<bool> ClearAsync(CancellationToken token);

        /// <summary>
        /// Remove and return the next item in the current queue.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// An object of the queue type.
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
        /// Return the next item in the current queue without removing it from the queue.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// An object of the queue type.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Implementations SHOULD throw an <see cref="InvalidOperationException"/> in the event
        /// that this method is called on an empty queue.
        /// </exception>
        Task<T> PeekAsync(CancellationToken token);
    }
}