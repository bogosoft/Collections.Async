using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents an asynchronous enumerable data structure.
    /// </summary>
    /// <typeparam name="T">The type of the items capable of being enumerated.</typeparam>
    public interface IAsyncEnumerable<T>
    {
        /// <summary>
        /// Get an asynchronous enumerator.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>An asynchronous enumerator.</returns>
        Task<IAsyncEnumerator<T>> GetEnumeratorAsync(CancellationToken token);
    }
}