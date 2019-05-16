using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents an asynchronous enumerable data structure.
    /// </summary>
    /// <typeparam name="T">The type of the items capable of being enumerated.</typeparam>
    public interface IAsyncEnumerable<out T>
    {
        /// <summary>
        /// Get an asynchronous enumerator.
        /// </summary>
        /// <returns>An asynchronous enumerator.</returns>
        IAsyncEnumerator<T> GetEnumerator();
    }

    /// <summary>
    /// Provides a set of static methods for working with <see cref="IAsyncEnumerable{T}"/> types.
    /// </summary>
    public static class IAsyncEnumerableExtensions
    {
        /// <summary>
        /// Copy items from the current enumerable structure sequentially to a target indexable collection.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being enumerated.</typeparam>
        /// <param name="source">The current <see cref="IAsyncEnumerable{T}"/> implementation.</param>
        /// <param name="target">An indexable collection to copy enumerated items into.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value corresponding to the number of items copied.
        /// </returns>
        public static Task<int> CopyToAsync<T>(
            this IAsyncEnumerable<T> source,
            IList<T> target,
            CancellationToken token = default(CancellationToken)
            )
        {
            return source.CopyToAsync(target, 0, target.Count, token);
        }

        /// <summary>
        /// Copy items from the current enumerable structure sequentially to a target indexable collection.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being enumerated.</typeparam>
        /// <param name="source">The current <see cref="IAsyncEnumerable{T}"/> implementation.</param>
        /// <param name="target">An indexable collection to copy enumerated items into.</param>
        /// <param name="start">
        /// A value corresponding to the index of the target to begin copying into.
        /// </param>
        /// <param name="count">
        /// A value corresponding to the maximum number of items to copy.
        /// </param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value corresponding to the number of items copied.
        /// </returns>
        public static async Task<int> CopyToAsync<T>(
            this IAsyncEnumerable<T> source,
            IList<T> target,
            int start,
            int count,
            CancellationToken token = default(CancellationToken)
            )
        {
            var copied = 0;

            using (var enumerator = source.GetEnumerator())
            {
                while (copied < count && await enumerator.MoveNextAsync(token).ConfigureAwait(false))
                {
                    target[start + copied++] = enumerator.Current;
                }
            }

            return copied;
        }

        /// <summary>
        /// Convert the current asynchronously enumerable type to an array.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the array.</typeparam>
        /// <param name="source">The current <see cref="IAsyncEnumerable{T}"/> implementation.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>An array of items of the enumerated type.</returns>
        public static async Task<T[]> ToArrayAsync<T>(
            this IAsyncEnumerable<T> source,
            CancellationToken token = default(CancellationToken)
            )
        {
            if (token.IsCancellationRequested)
            {
                return new T[0];
            }

            var target = new T[8];

            var count = 0;

            using (var enumerator = source.GetEnumerator())
            {
                while (await enumerator.MoveNextAsync(token).ConfigureAwait(false))
                {
                    if (count == target.Length)
                    {
                        Array.Resize(ref target, target.Length * 2);
                    }

                    target[count++] = enumerator.Current;
                }
            }

            if (count != target.Length)
            {
                Array.Resize(ref target, count);
            }

            return target;
        }

        /// <summary>
        /// Convert the current asynchronously enumerable type to a list.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the list.</typeparam>
        /// <param name="source">The current <see cref="IAsyncEnumerable{T}"/> implementation.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>A list of items of the enumerated type.</returns>
        public static async Task<List<T>> ToListAsync<T>(
            this IAsyncEnumerable<T> source,
            CancellationToken token = default(CancellationToken)
            )
        {
            if (token.IsCancellationRequested)
            {
                return new List<T>();
            }

            var target = new List<T>();

            using (var enumerator = source.GetEnumerator())
            {
                while (await enumerator.MoveNextAsync(token).ConfigureAwait(false))
                {
                    target.Add(enumerator.Current);
                }
            }

            return target;
        }
    }
}