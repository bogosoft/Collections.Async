using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with <see cref="ITraversable{T}"/> types.
    /// </summary>
    public static class Traversable<T>
    {
        internal class EnumerableSequence : ITraversable<T>
        {
            internal struct Cursor : ICursor<T>
            {
                private IEnumerator<T> enumerator;

                private bool active, disposed;

                public bool IsDisposed => disposed;

                public bool IsExpended => !active;

                public Cursor(IEnumerable<T> enumerable)
                {
                    active = disposed = false;

                    enumerator = enumerable.GetEnumerator();
                }

                public void Dispose()
                {
                    if (!disposed)
                    {
                        enumerator.Dispose();

                        disposed = true;
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
                    if (token.IsCancellationRequested)
                    {
                        return Task.FromResult(false);
                    }
                    else
                    {
                        return Task.FromResult(active = enumerator.MoveNext());
                    }
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
                    cursor = new Cursor(enumerable);
                }

                return Task.FromResult(cursor);
            }
        }

        /// <summary>
        /// Get an empty traversable sequence.
        /// </summary>
        public static ITraversable<T> Empty => new EmptySequence<T>();

        /// <summary>
        /// Create a traversable sequence from one or more given items.
        /// </summary>
        /// <param name="items">Items to include in a traversable sequence.</param>
        /// <returns>
        /// A traversable sequence consisting of the given items.
        /// </returns>
        public static ITraversable<T> From(params T[] items)
        {
            return new EnumerableSequence(items);
        }
    }
}