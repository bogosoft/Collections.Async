using System.Collections.Generic;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Extended functionality for <see cref="IEnumerable{T}"/> types.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Convert the current enumerable type to an <see cref="ITraversable{T}"/> type.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="source">The current <see cref="IEnumerable{T}"/> implementation.</param>
        /// <returns>A traversable data structure.</returns>
        public static ITraversable<T> ToTraversable<T>(this IEnumerable<T> source)
        {
            return new EnumerableTraverser<T>(source);
        }
    }
}