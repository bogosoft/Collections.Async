using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with <see cref="IEnumerableAsync{T}"/> types.
    /// </summary>
    public static class TraversableExtensions
    {
        /// <summary>
        /// Copy items from the current traversable structure sequentially to a target indexable collection. Calling this
        /// method is equivalent to calling <see cref="CopyToAsync{T}(IEnumerableAsync{T}, IList{T}, CancellationToken)"/>
        /// with a value of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
        /// <param name="source">The current <see cref="IEnumerableAsync{T}"/> implementation.</param>
        /// <param name="target">An indexable collection to copy traversed items into.</param>
        /// <returns>
        /// A value corresponding to the number of items copied.
        /// </returns>
        public static Task<int> CopyToAsync<T>(this IEnumerableAsync<T> source, IList<T> target)
        {
            return source.CopyToAsync(target, 0, target.Count, CancellationToken.None);
        }

        /// <summary>
        /// Copy items from the current traversable structure sequentially to a target indexable collection.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
        /// <param name="source">The current <see cref="IEnumerableAsync{T}"/> implementation.</param>
        /// <param name="target">An indexable collection to copy traversed items into.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value corresponding to the number of items copied.
        /// </returns>
        public static Task<int> CopyToAsync<T>(
            this IEnumerableAsync<T> source,
            IList<T> target,
            CancellationToken token
            )
        {
            return source.CopyToAsync(target, 0, target.Count, token);
        }

        /// <summary>
        /// Copy items from the current traversable structure sequentially to a target indexable collection.
        /// Calling this method is equivalent to calling
        /// <see cref="CopyToAsync{T}(IEnumerableAsync{T}, IList{T}, int, int, CancellationToken)"/>
        /// with value of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
        /// <param name="source">The current <see cref="IEnumerableAsync{T}"/> implementation.</param>
        /// <param name="target">An indexable collection to copy traversed items into.</param>
        /// <param name="start">
        /// A value corresponding to the index of the target to begin copying into.
        /// </param>
        /// <param name="count">
        /// A value corresponding to the maximum number of items to copy.
        /// </param>
        /// <returns>
        /// A value corresponding to the number of items copied.
        /// </returns>
        public static Task<int> CopyToAsync<T>(
            this IEnumerableAsync<T> source,
            IList<T> target,
            int start,
            int count
            )
        {
            return source.CopyToAsync(target, start, count, CancellationToken.None);
        }

        /// <summary>
        /// Copy items from the current traversable structure sequentially to a target indexable collection.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
        /// <param name="source">The current <see cref="IEnumerableAsync{T}"/> implementation.</param>
        /// <param name="target">An indexable collection to copy traversed items into.</param>
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
            this IEnumerableAsync<T> source,
            IList<T> target,
            int start,
            int count,
            CancellationToken token
            )
        {
            var copied = 0;

            using (var cursor = await source.GetEnumeratorAsync(token).ConfigureAwait(false))
            {
                while (copied < count && await cursor.MoveNextAsync(token).ConfigureAwait(false))
                {
                    target[start + copied++] = await cursor.GetCurrentAsync(token).ConfigureAwait(false);
                }
            }

            return copied;
        }

        /// <summary>
        /// Create and return a cursor.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
        /// <param name="sequence">The current <see cref="IEnumerableAsync{T}"/> implementation.</param>
        /// <returns>A readable, forward-only cursor.</returns>
        public static Task<IAsyncEnumerator<T>> GetCursorAsync<T>(this IEnumerableAsync<T> sequence)
        {
            return sequence.GetEnumeratorAsync(CancellationToken.None);
        }

        /// <summary>
        /// Convert the current traversable type to an array and return it. Calling this method is equivalent to
        /// calling <see cref="ToArrayAsync{T}(IEnumerableAsync{T}, CancellationToken)"/> with a value
        /// of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the array.</typeparam>
        /// <param name="source">The current <see cref="IEnumerableAsync{T}"/> implementation.</param>
        /// <returns>An array of items of the traversed type.</returns>
        public static Task<T[]> ToArrayAsync<T>(this IEnumerableAsync<T> source)
        {
            return source.ToArrayAsync(CancellationToken.None);
        }

        /// <summary>
        /// Convert the current traversable type to an array and return it.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the array.</typeparam>
        /// <param name="source">The current <see cref="IEnumerableAsync{T}"/> implementation.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>An array of items of the traversed type.</returns>
        public static async Task<T[]> ToArrayAsync<T>(this IEnumerableAsync<T> source, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return new T[0];
            }

            var target = new T[8];

            var count = 0;

            using (var cursor = await source.GetEnumeratorAsync(token).ConfigureAwait(false))
            {
                while (await cursor.MoveNextAsync(token).ConfigureAwait(false))
                {
                    if (count == target.Length)
                    {
                        Array.Resize(ref target, target.Length * 2);
                    }

                    target[count++] = await cursor.GetCurrentAsync(token).ConfigureAwait(false);
                }
            }

            if (count != target.Length)
            {
                Array.Resize(ref target, count);
            }

            return target;
        }

        /// <summary>
        /// Convert the current traversable type to a list.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the array.</typeparam>
        /// <param name="source">The current <see cref="IEnumerableAsync{T}"/> implementation.</param>
        /// <returns>A list of items of the traversed type.</returns>
        public static Task<List<T>> ToListAsync<T>(this IEnumerableAsync<T> source)
        {
            return source.ToListAsync(CancellationToken.None);
        }

        /// <summary>
        /// Convert the current traversable type to a list.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the array.</typeparam>
        /// <param name="source">The current <see cref="IEnumerableAsync{T}"/> implementation.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>A list of items of the traversed type.</returns>
        public static async Task<List<T>> ToListAsync<T>(this IEnumerableAsync<T> source, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return new List<T>();
            }

            var target = new List<T>();

            using (var cursor = await source.GetEnumeratorAsync(token).ConfigureAwait(false))
            {
                while (await cursor.MoveNextAsync(token).ConfigureAwait(false))
                {
                    target.Add(await cursor.GetCurrentAsync(token).ConfigureAwait(false));
                }
            }

            return target;
        }
    }
}