namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with <see cref="IEnumerableAsync{T}"/> types.
    /// </summary>
    public static class Traversable<T>
    {
        /// <summary>
        /// Get an empty traversable sequence.
        /// </summary>
        public static IEnumerableAsync<T> Empty => new EmptySequence<T>();

        /// <summary>
        /// Create a traversable sequence from one or more given items.
        /// </summary>
        /// <param name="items">Items to include in a traversable sequence.</param>
        /// <returns>
        /// A traversable sequence consisting of the given items.
        /// </returns>
        public static IEnumerableAsync<T> From(params T[] items)
        {
            return new EnumerableSequence<T>(items);
        }
    }
}