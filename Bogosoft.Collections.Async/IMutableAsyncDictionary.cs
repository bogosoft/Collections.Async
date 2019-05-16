using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents a mutable, asynchronous dictionary of values indexed by key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    public interface IMutableAsyncDictionary<TKey, TValue> :
        IAsyncDictionary<TKey, TValue>,
        IMutableAsyncCollection<(TKey Key, TValue Value)>
    {
        /// <summary>
        /// Remove an entry from the current dictionary by a given key.
        /// </summary>
        /// <param name="key">The key of the associated entry to be removed.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A value indicating whether or not an entry associated with the given key was successfully removed.
        /// </returns>
        Task<bool> RemoveAsync(TKey key, CancellationToken token);
    }

    /// <summary>
    /// Extended functionality for the <see cref="IMutableAsyncDictionary{TKey, TValue}"/> contract.
    /// </summary>
    public static class IMutableAsyncDictionaryExtensions
    {
        /// <summary>
        /// Add an entry to the current dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <param name="dictionary">The current dictionary.</param>
        /// <param name="key">A key associated with the new entry.</param>
        /// <param name="value">A value associated with the new entry.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task representing an asynchronous operation.</returns>
        public static Task AddAsync<TKey, TValue>(
            this IMutableAsyncDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value,
            CancellationToken token = default(CancellationToken)
            )
        {
            return dictionary.AddAsync((key, value), token);
        }

        /// <summary>
        /// Remove an entry from the current dictionary by a given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <param name="dictionary">The current dictionary.</param>
        /// <param name="key">The key of the associated entry to be removed.</param>
        /// <returns>
        /// A value indicating whether or not an entry associated with the given key was successfully removed.
        /// </returns>
        public static Task<bool> RemoveAsync<TKey, TValue>(this IMutableAsyncDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.RemoveAsync(key, CancellationToken.None);
        }
    }
}