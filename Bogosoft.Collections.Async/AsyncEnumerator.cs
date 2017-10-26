namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with asynchronous enumerators.
    /// </summary>
    public static class AsyncEnumerator<T>
    {
        /// <summary>
        /// Get an empty asynchronous enumerator.
        /// </summary>
        public static IAsyncEnumerator<T> Empty => new EmptyAsyncEnumerator<T>();
    }
}