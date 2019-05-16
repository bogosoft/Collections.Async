using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents a mutable, asynchronous collection of items.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public interface IMutableAsyncCollection<T> : IAsyncCollection<T>
    {
        /// <summary>
        /// Add a given item to the current collection.
        /// </summary>
        /// <param name="item">An item to add to the current collection.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task representing an asynchronous operation.</returns>
        Task AddAsync(T item, CancellationToken token);

        /// <summary>
        /// Clear the current collection of all items.
        /// </summary>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task representing an asynchronous operation.</returns>
        Task ClearAsync(CancellationToken token);

        /// <summary>
        /// Remove a given item from the current collection.
        /// </summary>
        /// <param name="item">An item to remove from the current collection.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A value indicating whether or not the given item was successfully removed.
        /// </returns>
        Task<bool> RemoveAsync(T item, CancellationToken token);
    }

    /// <summary>
    /// Extended functionality for the <see cref="IMutableAsyncCollection{T}"/> contract.
    /// </summary>
    public static class IMutableAsyncCollectionExtensions
    {
        /// <summary>
        /// Add a given item to the current collection.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The current collection.</param>
        /// <param name="item">An item to add to the current collection.</param>
        /// <returns>A task representing an asynchronous operation.</returns>
        public static Task AddAsync<T>(this IMutableAsyncCollection<T> collection, T item)
        {
            return collection.AddAsync(item, CancellationToken.None);
        }

        /// <summary>
        /// Clear the current collection of all items.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The current collection.</param>
        /// <returns>A task representing an asynchronous operation.</returns>
        public static Task ClearAsync<T>(this IMutableAsyncCollection<T> collection)
        {
            return collection.ClearAsync(CancellationToken.None);
        }

        /// <summary>
        /// Remove a given item from the current collecion.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The current collection.</param>
        /// <param name="item">An item to remove from the current collection.</param>
        /// <returns>
        /// A value indicating whether or not the given item was successfully removed.
        /// </returns>
        public static Task<bool> RemoveAsync<T>(this IMutableAsyncCollection<T> collection, T item)
        {
            return collection.RemoveAsync(item, CancellationToken.None);
        }
    }
}