namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with <see cref="IAsyncEnumerable{T}"/> types.
    /// </summary>
    public static class AsyncEnumerable<T>
    {
        /// <summary>
        /// Get an empty asynchronously enumerable sequence.
        /// </summary>
        public static IAsyncEnumerable<T> Empty => new EmptySequence<T>();

        /// <summary>
        /// Create an asynchronously enumerable sequence from one or more given items.
        /// </summary>
        /// <param name="items">Items to include in an asynchronously enumerable sequence.</param>
        /// <returns>
        /// An asynchronously enumerable sequence consisting of the given items.
        /// </returns>
        public static IAsyncEnumerable<T> From(params T[] items)
        {
            return new AsyncEnumerableSequence<T>(items);
        }
    }
}