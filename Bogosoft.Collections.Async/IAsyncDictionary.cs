using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents an asynchronous dictionary of values indexed by key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    public interface IAsyncDictionary<TKey, TValue> : IAsyncCollection<(TKey Key, TValue Value)>
    {
        /// <summary>
        /// Get a value by a given key.
        /// </summary>
        /// <param name="key">An object of the key type.</param>
        /// <returns>An object of the value type.</returns>
        AsyncValue<TValue> this[TKey key] { get; }

        /// <summary>
        /// Get an asynchronous sequence of the keys of the current dictionary.
        /// </summary>
        IAsyncEnumerable<TKey> Keys { get; }

        /// <summary>
        /// Get an asynchronous sequence of the values of the current dictionary.
        /// </summary>
        IAsyncEnumerable<TValue> Values { get; }

        /// <summary>
        /// Determine whether the current dictionary contains a given key.
        /// </summary>
        /// <param name="key">A key to search for in the current dictionary.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>
        /// A value indicating whether or not the current dictionary contains the given key.
        /// </returns>
        Task<bool> ContainsKeyAsync(TKey key, CancellationToken token);

        /// <summary>
        /// Get a value from the current dictionary by its key.
        /// </summary>
        /// <param name="key">An object of the key type.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>An object of the value type.</returns>
        Task<TValue> GetAsync(TKey key, CancellationToken token);
    }

    /// <summary>
    /// Extended functionality for the <see cref="IAsyncDictionary{TKey, TValue}"/> contract.
    /// </summary>
    public static class IAsyncDictionaryExtensions
    {
        /// <summary>
        /// Determine whether the current dictionary contains a given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <param name="dictionary">The current dictionary.</param>
        /// <param name="key">A key to search for in the current dictionary.</param>
        /// <returns>
        /// A value indicating whether or not the current dictionary contains the given key.
        /// </returns>
        public static Task<bool> ContainsKeyAsync<TKey, TValue>(this IAsyncDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.ContainsKeyAsync(key, CancellationToken.None);
        }

        /// <summary>
        /// Get a value from the current dictionary by its key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <param name="dictionary">The current dictionary.</param>
        /// <param name="key">An object of the key type.</param>
        /// <returns>An object of the value type.</returns>
        public static Task<TValue> GetAsync<TKey, TValue>(this IAsyncDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetAsync(key, CancellationToken.None);
        }
    }
}