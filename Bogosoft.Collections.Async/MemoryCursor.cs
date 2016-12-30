using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// An in-memory implementation of the <see cref="ICursor{T}"/> contract.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MemoryCursor<T> : ICursor<T>
    {
        private IEnumerator<T> enumerator;

        /// <summary>
        /// Get a value indicating whether the current cursor has not been disposed of.
        /// </summary>
        public bool IsActive
        {
            get { return !IsExpended; }
            set { IsExpended = !value; }
        }

        /// <summary>
        /// Get a value indicating whether the current cursor has been disposed of.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Get a value indicating whether the current cursor has finished traversing.
        /// </summary>
        public bool IsExpended { get; private set; }

        /// <summary>
        /// Create a new instance of the <see cref="MemoryCursor{T}"/> class.
        /// </summary>
        /// <param name="enumerator">A typed enumerator.</param>
        public MemoryCursor(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        /// <summary>
        /// Create a new instance of the <see cref="MemoryCursor{T}"/> class.
        /// </summary>
        /// <param name="enumerable">An enumerable of items.</param>
        public MemoryCursor(IEnumerable<T> enumerable)
            : this(enumerable.GetEnumerator())
        {
        }

        /// <summary>
        /// Dispose of the underlying enumerator.
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
        /// Get the item pointed at by the present position of the current cursor.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>The item at the present position of the current cursor.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown in the case that calling <see cref="MoveNextAsync(CancellationToken)"/>
        /// before this method would result in a value of false or if
        /// <see cref="MoveNextAsync(CancellationToken)"/> has not been called for the first time.
        /// </exception>
        public Task<T> GetCurrentAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (IsExpended)
            {
                throw new InvalidOperationException("Cursor has moved past its last record.");
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
            return Task.FromResult(!token.IsCancellationRequested && (IsActive = enumerator.MoveNext()));
        }
    }
}