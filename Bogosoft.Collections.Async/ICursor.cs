using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents a forward-only, iterable data structure.
    /// </summary>
    /// <typeparam name="T">The type of the object being traversed.</typeparam>
    public interface ICursor<T> : IDisposable
    {
        /// <summary>
        /// Get a value indicating whether the current cursor has been disposed of.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Get a value indicating whether the current cursor has moved past its last record.
        /// </summary>
        bool IsExpended { get; }

        /// <summary>
        /// Get the item pointed at by the present position of the current cursor.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// The item at the present position of the current cursor.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Implementations SHOULD throw an <see cref="InvalidOperationException"/> in the case that
        /// calling <see cref="MoveNextAsync(CancellationToken)"/> before this method would result in
        /// a value of false.
        /// </exception>
        Task<T> GetCurrentAsync(CancellationToken token);

        /// <summary>
        /// Advance the position of the current cursor and get a value indicating whether or not
        /// the resulting position is a valid record.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// True if the position of the cursor corresponds to a valid record; false otherwise.
        /// </returns>
        Task<bool> MoveNextAsync(CancellationToken token);
    }
}