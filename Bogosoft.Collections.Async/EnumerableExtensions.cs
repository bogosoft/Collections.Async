using System.Collections.Generic;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Extended functionality for <see cref="IEnumerable{T}"/> types.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Convert the current enumerable type to a cursor.
        /// </summary>
        /// <typeparam name="T">The type of the objects to enumerate.</typeparam>
        /// <param name="items">The current <see cref="IEnumerable{T}"/> implementation.</param>
        /// <returns>A cursor.</returns>
        public static ICursor<T> ToCursor<T>(this IEnumerable<T> items)
        {
            return new MemoryCursor<T>(items);
        }
    }
}