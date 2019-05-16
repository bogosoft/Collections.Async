namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents an asynchronously enumerable sequence of elements grouped by key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TElement">The type of the grouped elements.</typeparam>
    public interface IAsyncGrouping<TKey, TElement> : IAsyncEnumerable<TElement>
    {
        /// <summary>
        /// Get the key of the current grouping.
        /// </summary>
        TKey Key { get; }
    }
}