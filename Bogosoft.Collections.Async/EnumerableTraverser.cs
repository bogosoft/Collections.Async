using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// An <see cref="IEnumerable{T}"/> traversable adapter.
    /// </summary>
    /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
    public sealed class EnumerableTraverser<T> : ITraversable<T>
    {
        /// <summary>
        /// An adapter for an <see cref="IEnumerator{T}"/>.
        /// </summary>
        public struct Cursor : ICursor<T>
        {
            private IEnumerator<T> enumerator;

            private bool active;

            /// <summary>
            /// Get a value indicating whether the current cursor has been disposed of.
            /// </summary>
            public bool IsDisposed { get; private set; }

            /// <summary>
            /// Get a value indicating whether the current cursor has moved past its last record.
            /// </summary>
            public bool IsExpended { get; private set; }

            /// <summary>
            /// Create a new instance of <see cref="Cursor"/> with a given <see cref="IEnumerator{T}"/>.
            /// </summary>
            /// <param name="enumerator">
            /// An instance of <see cref="IEnumerator{TItem}"/> to be adapted.
            /// </param>
            public Cursor(IEnumerator<T> enumerator)
            {
                active = false;

                this.enumerator = enumerator;

                IsDisposed = false;
                IsExpended = false;
            }

            /// <summary>
            /// Dispose of the underlying <see cref="IEnumerator{T}"/> and mark the current cursor as disposed.
            /// </summary>
            public void Dispose()
            {
                if (!IsDisposed)
                {
                    enumerator.Dispose();

                    IsDisposed = true;
                }
            }

            /// <summary>
            /// Get the item being pointed at by the present position of the current cursor.
            /// </summary>
            /// <param name="token">A <see cref="CancellationToken"/> object.</param>
            /// <returns>The item at the present position of the current cursor.</returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown in the event that <see cref="MoveNextAsync(CancellationToken)"/> has not been called
            /// at least once or if calling <see cref="MoveNextAsync(CancellationToken)"/> before this would
            /// return a value of false.
            /// </exception>
            /// <exception cref="OperationCanceledException">
            /// Thrown in the event that a <see cref="CancellationTokenSource"/> has requested a cancellation.
            /// </exception>
            public Task<T> GetCurrentAsync(CancellationToken token)
            {
                token.ThrowIfCancellationRequested();

                if (!active)
                {
                    throw new InvalidOperationException("This cursor has not been initialized.");
                }

                return Task.FromResult(enumerator.Current);
            }

            /// <summary>
            /// Advance the position of the current cursor and get a value indicating whether or not
            /// the resulting position is a valid record.
            /// </summary>
            /// <param name="token">A <see cref="CancellationToken"/> object.</param>
            /// <returns>
            /// True if the position of the cursor corresponds to a valid record; false otherwise.
            /// </returns>
            public Task<bool> MoveNextAsync(CancellationToken token)
            {
                if(!token.IsCancellationRequested && !IsExpended)
                {
                    if(false == (active = enumerator.MoveNext()))
                    {
                        IsExpended = true;
                    }
                }

                return Task.FromResult(active);
            }
        }

        private IEnumerable<T> enumerable;

        /// <summary>
        /// Create a new instance of the <see cref="EnumerableTraverser{T}"/> class with
        /// a given <see cref="IEnumerable{T}"/> to adapt.
        /// </summary>
        /// <param name="enumerable">An instance of <see cref="IEnumerable{T}"/>.</param>
        public EnumerableTraverser(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        /// <summary>
        /// Create and return a new instance of <see cref="ICursor{T}"/>.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> class.</param>
        /// <returns>A new cursor.</returns>
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
}
