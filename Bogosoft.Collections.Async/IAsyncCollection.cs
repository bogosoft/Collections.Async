using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents a basic, asynchronous, read-only collection of items of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public interface IAsyncCollection<T> : IAsyncEnumerable<T>
    {
        /// <summary>
        /// Get the number of items in the current collection.
        /// </summary>
        AsyncValue<ulong> Count { get; }

        /// <summary>
        /// Determine whether the current collection contains a given item.
        /// </summary>
        /// <param name="item">An item to search for in the current collection.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A value indicating whether or not the current collection contains the given item.
        /// </returns>
        Task<bool> ContainsAsync(T item, CancellationToken token);

        /// <summary>
        /// Count the number of items in the current collection.
        /// </summary>
        /// <param name="token">A cancellation instruction.</param>
        /// <returns>A value corresponding to the number of items in the current collection.</returns>
        Task<ulong> CountAsync(CancellationToken token);
    }

    /// <summary>
    /// Extended functionality for the <see cref="IAsyncCollection{T}"/> contract.
    /// </summary>
    public static class IAsyncCollectionExtensions
    {
        /// <summary>
        /// Determine whether the current collection contains a given item.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The current collection.</param>
        /// <param name="item">An item to search for in the current collection.</param>
        /// <returns>
        /// A value indicating whether or not the current collection contains the given item.
        /// </returns>
        public static Task<bool> ContainsAsync<T>(this IAsyncCollection<T> collection, T item)
        {
            return collection.ContainsAsync(item, CancellationToken.None);
        }

        /// <summary>
        /// Count the number of items in the current collection.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The current collection.</param>
        /// <returns>A value corresponding to the number of items in the current collection.</returns>
        public static Task<ulong> CountAsync<T>(this IAsyncCollection<T> collection)
        {
            return collection.CountAsync(CancellationToken.None);
        }
    }
}