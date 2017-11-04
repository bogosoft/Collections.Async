﻿using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with asynchronous enumerators.
    /// </summary>
    public static class AsyncEnumeratorExtensions
    {
        /// <summary>
        /// Advance the position of the current enumerator and get a value indicating whether or not
        /// the resulting position is a valid record.
        /// </summary>
        /// <param name="cursor">The current <see cref="IAsyncEnumerator{T}"/> implementation.</param>
        /// <returns>
        /// True if the position of the current enumerator corresponds to a valid record; false otherwise.
        /// </returns>
        public static Task<bool> MoveNextAsync<T>(this IAsyncEnumerator<T> cursor)
        {
            return cursor.MoveNextAsync(CancellationToken.None);
        }
    }
}