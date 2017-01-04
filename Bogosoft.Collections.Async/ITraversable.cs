using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Identifies an implementer as capable of creating a cursor to traverse items of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the items capable of being traversed.</typeparam>
    public interface ITraversable<T>
    {
        /// <summary>
        /// Create and return a cursor.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>A readable, forward-only cursor.</returns>
        Task<ICursor<T>> GetCursorAsync(CancellationToken token);
    }
}