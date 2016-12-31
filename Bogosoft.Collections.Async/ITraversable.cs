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
        /// Get a traversable data structure.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>An instance of <see cref="ICursor{T}"/>.</returns>
        Task<ICursor<T>> GetCursorAsync(CancellationToken token);
    }
}