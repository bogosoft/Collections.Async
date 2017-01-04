using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with <see cref="ITraversable{T}"/> types.
    /// </summary>
    public static class Traversable
    {
        /// <summary>
        /// Copy items from the current traversable structure sequentially to a target indexable collection. Calling this
        /// method is equivalent to calling <see cref="CopyToAsync{T}(ITraversable{T}, IList{T}, CancellationToken)"/>
        /// with a value of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
        /// <param name="source">The current <see cref="ITraversable{T}"/> implementation.</param>
        /// <param name="target">An indexable collection to copy traversed items into.</param>
        /// <returns>
        /// A value corresponding to the number of items copied.
        /// </returns>
        public static async Task<int> CopyToAsync<T>(this ITraversable<T> source, IList<T> target)
        {
            return await source.CopyToAsync(target, 0, target.Count, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Copy items from the current traversable structure sequentially to a target indexable collection.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
        /// <param name="source">The current <see cref="ITraversable{T}"/> implementation.</param>
        /// <param name="target">An indexable collection to copy traversed items into.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value corresponding to the number of items copied.
        /// </returns>
        public static async Task<int> CopyToAsync<T>(
            this ITraversable<T> source,
            IList<T> target,
            CancellationToken token
            )
        {
            return await source.CopyToAsync(target, 0, target.Count, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Copy items from the current traversable structure sequentially to a target indexable collection.
        /// Calling this method is equivalent to calling
        /// <see cref="CopyToAsync{T}(ITraversable{T}, IList{T}, int, int, CancellationToken)"/>
        /// with value of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
        /// <param name="source">The current <see cref="ITraversable{T}"/> implementation.</param>
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
        public static async Task<int> CopyToAsync<T>(
            this ITraversable<T> source,
            IList<T> target,
            int start,
            int count
            )
        {
            return await source.CopyToAsync(target, start, count, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Copy items from the current traversable structure sequentially to a target indexable collection.
        /// </summary>
        /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
        /// <param name="source">The current <see cref="ITraversable{T}"/> implementation.</param>
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
            this ITraversable<T> source,
            IList<T> target,
            int start,
            int count,
            CancellationToken token
            )
        {
            var copied = 0;

            using (var cursor = await source.GetCursorAsync(token).ConfigureAwait(false))
            {
                while(copied < count && await cursor.MoveNextAsync(token).ConfigureAwait(false))
                {
                    target[start + copied++] = await cursor.GetCurrentAsync(token).ConfigureAwait(false);
                }
            }

            return copied;
        }

        /// <summary>
        /// Convert the current traversable type to an array and return it. Calling this method is equivalent to
        /// calling <see cref="Traversable.ToArrayAsync{T}(ITraversable{T}, CancellationToken)"/> with a value
        /// of <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the array.</typeparam>
        /// <param name="source">The current <see cref="ITraversable{T}"/> implementation.</param>
        /// <returns>An array of items of the traversed type.</returns>
        public static async Task<T[]> ToArrayAsync<T>(this ITraversable<T> source)
        {
            return await source.ToArrayAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert the current traversable type to an array and return it.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the array.</typeparam>
        /// <param name="source">The current <see cref="ITraversable{T}"/> implementation.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>An array of items of the traversed type.</returns>
        public static async Task<T[]> ToArrayAsync<T>(this ITraversable<T> source, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return new T[0];
            }

            var target = new T[8];

            var count = 0;

            using (var cursor = await source.GetCursorAsync(token).ConfigureAwait(false))
            {
                while(await cursor.MoveNextAsync(token).ConfigureAwait(false))
                {
                    if(count == target.Length)
                    {
                        Array.Resize(ref target, target.Length * 2);
                    }

                    target[count++] = await cursor.GetCurrentAsync(token).ConfigureAwait(false);
                }
            }

            if(count != target.Length)
            {
                Array.Resize(ref target, count);
            }

            return target;
        }

        /// <summary>
        /// Convert the current traversable type to a list.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the array.</typeparam>
        /// <param name="source">The current <see cref="ITraversable{T}"/> implementation.</param>
        /// <returns>A list of items of the traversed type.</returns>
        public static async Task<List<T>> ToListAsync<T>(this ITraversable<T> source)
        {
            return await source.ToListAsync(CancellationToken.None);
        }

        /// <summary>
        /// Convert the current traversable type to a list.
        /// </summary>
        /// <typeparam name="T">The type of the items to be included in the array.</typeparam>
        /// <param name="source">The current <see cref="ITraversable{T}"/> implementation.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>A list of items of the traversed type.</returns>
        public static async Task<List<T>> ToListAsync<T>(this ITraversable<T> source, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return new List<T>();
            }

            var target = new List<T>();

            using (var cursor = await source.GetCursorAsync(token))
            {
                while(await cursor.MoveNextAsync(token))
                {
                    target.Add(await cursor.GetCurrentAsync(token));
                }
            }

            return target;
        }
    }

    /// <summary>
    /// Provides a set of static methods for working with <see cref="ITraversable{T}"/> types.
    /// </summary>
    public static class Traversable<T>
    {
        internal struct EmptySequence : ITraversable<T>
        {
            internal struct Cursor : ICursor<T>
            {
                public bool IsDisposed { get; private set; }

                public bool IsExpended
                {
                    get { return true; }
                }

                public void Dispose()
                {
                    IsDisposed = true;
                }

                public Task<T> GetCurrentAsync(CancellationToken token)
                {
                    throw new NotSupportedException();
                }

                public Task<bool> MoveNextAsync(CancellationToken token)
                {
                    return Task.FromResult(false);
                }
            }

            public Task<ICursor<T>> GetCursorAsync(CancellationToken token)
            {
                return Task.FromResult<ICursor<T>>(new Cursor());
            }
        }

        internal struct EnumerableSequence : ITraversable<T>
        {
            internal struct Cursor : ICursor<T>
            {
                private IEnumerator<T> enumerator;

                private bool active;

                public bool IsDisposed { get; private set; }

                public bool IsExpended { get; private set; }

                public Cursor(IEnumerator<T> enumerator)
                {
                    active = false;

                    this.enumerator = enumerator;

                    IsDisposed = false;
                    IsExpended = false;
                }

                public void Dispose()
                {
                    if (!IsDisposed)
                    {
                        enumerator.Dispose();

                        IsDisposed = true;
                    }
                }

                public Task<T> GetCurrentAsync(CancellationToken token)
                {
                    token.ThrowIfCancellationRequested();

                    if (!active)
                    {
                        throw new InvalidOperationException("This cursor has not been initialized.");
                    }

                    return Task.FromResult(enumerator.Current);
                }

                public Task<bool> MoveNextAsync(CancellationToken token)
                {
                    if (!token.IsCancellationRequested && !IsExpended)
                    {
                        if (false == (active = enumerator.MoveNext()))
                        {
                            IsExpended = true;
                        }
                    }

                    return Task.FromResult(active);
                }
            }

            private IEnumerable<T> enumerable;

            public EnumerableSequence(IEnumerable<T> enumerable)
            {
                this.enumerable = enumerable;
            }

            public Task<ICursor<T>> GetCursorAsync(CancellationToken token)
            {
                ICursor<T> cursor;

                if (token.IsCancellationRequested)
                {
                    cursor = Cursor<T>.Empty;
                }
                else
                {
                    cursor = new Cursor(enumerable.GetEnumerator());
                }

                return Task.FromResult(cursor);
            }
        }

        /// <summary>
        /// Get an empty traversable sequence.
        /// </summary>
        public static ITraversable<T> Empty
        {
            get { return new EmptySequence(); }
        }
    }
}