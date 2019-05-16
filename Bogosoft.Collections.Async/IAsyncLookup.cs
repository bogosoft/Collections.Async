using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents a data structure capable of asynchronously enumerating sequences of elements grouped by key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TElement">The type of the grouped elements.</typeparam>
    public interface IAsyncLookup<TKey, TElement> : IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>
    {
        /// <summary>
        /// Get a sequence of elements grouped by a given key.
        /// </summary>
        /// <param name="key">An object of the key type.</param>
        /// <returns>A sequence of elements grouped by the given key.</returns>
        IAsyncEnumerable<TElement> this[TKey key] { get; }

        /// <summary>
        /// Get the number of key-element sequence pairs in the current lookup.
        /// </summary>
        AsyncValue<ulong> Count { get; }

        /// <summary>
        /// Determine whether the current lookup contains a given key.
        /// </summary>
        /// <param name="key">An object of the key type.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A value indicating whether or not the current lookup contains the given key.
        /// </returns>
        Task<bool> ContainsAsync(TKey key, CancellationToken token);

        /// <summary>
        /// Count the number of key-element sequence pairs in the current lookup.
        /// </summary>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A value correspoding to the number of key-element sequence pairs in the current lookup.
        /// </returns>
        Task<ulong> CountAsync(CancellationToken token);
    }

    /// <summary>
    /// Extended functionality for the <see cref="IAsyncLookup{TKey, TElement}"/> contract.
    /// </summary>
    public static class IAsyncLookupExtensions
    {
        /// <summary>
        /// Determine whether the current lookup contains a given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TElement">The type of the grouped elements.</typeparam>
        /// <param name="lookup">The current lookup.</param>
        /// <param name="key">An object of the key type.</param>
        /// <returns>
        /// A value indicating whether or not the current lookup contains the given key.
        /// </returns>
        public static Task<bool> ContainsAsync<TKey, TElement>(this IAsyncLookup<TKey, TElement> lookup, TKey key)
        {
            return lookup.ContainsAsync(key, CancellationToken.None);
        }

        /// <summary>
        /// Count the number of key-element sequence pairs in the current lookup.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TElement">The type of the grouped elements.</typeparam>
        /// <param name="lookup">The current lookup.</param>
        /// <returns>
        /// A value correspoding to the number of key-element sequence pairs in the current lookup.
        /// </returns>
        public static Task<ulong> CountAsync<TKey, TElement>(this IAsyncLookup<TKey, TElement> lookup)
        {
            return lookup.CountAsync(CancellationToken.None);
        }
    }
}