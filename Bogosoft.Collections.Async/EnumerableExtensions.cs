using System.Collections.Generic;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Extended functionality for <see cref="IEnumerable{T}"/> types.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Convert the current <see cref="IEnumerable{T}"/> type to an <see cref="IEnumerableAsync{T}"/> type.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="source">The current <see cref="IEnumerable{T}"/> implementation.</param>
        /// <returns>An asynchronously enumerable object.</returns>
        public static IEnumerableAsync<T> ToTraversable<T>(this IEnumerable<T> source)
        {
            return new EnumerableSequence<T>(source);
        }
    }
}