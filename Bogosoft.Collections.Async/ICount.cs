using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Indicates that an implementation is capable of counting objects of the given type.
    /// </summary>
    /// <typeparam name="T">The type of the objects that can be counted.</typeparam>
    public interface ICount<T>
    {
        /// <summary>
        /// Count the number of objects of the specified type.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value corresponding to the number of objects the specified tpye.
        /// </returns>
        Task<ulong> CountAsync(CancellationToken token);
    }
}