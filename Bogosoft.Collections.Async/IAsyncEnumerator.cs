using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents an asynchronous enumerator.
    /// </summary>
    /// <typeparam name="T">The type of the object which can be enumerated.</typeparam>
    public interface IAsyncEnumerator<T> : IDisposable
    {
        /// <summary>
        /// Get the item pointed at by the present position of the current enumerator.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// The item at the present position of the current enumerator.
        /// </returns>
        Task<T> GetCurrentAsync(CancellationToken token);

        /// <summary>
        /// Advance the position of the current enumerator and get a value indicating whether or not
        /// the resulting position is a valid record.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// True if the position of the enumerator corresponds to a valid record; false otherwise.
        /// </returns>
        Task<bool> MoveNextAsync(CancellationToken token);
    }
}