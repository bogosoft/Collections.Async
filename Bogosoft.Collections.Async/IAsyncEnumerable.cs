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
}