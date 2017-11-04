using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Extended functionality for the <see cref="ICountAsync{T}"/> contract.
    /// </summary>
    public static class ICountAsyncExtensions
    {
        /// <summary>
        /// Count the number of objects of the specified type. Calling this method is equivalent to calling
        /// <see cref="ICountAsync{T}.CountAsync(CancellationToken)"/> with a value of
        /// <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects that can be counted.</typeparam>
        /// <param name="target">The current <see cref="ICountAsync{T}"/> implementation.</param>
        /// <returns>
        /// A value corresponding to the number of objects the specified tpye.
        /// </returns>
        public static Task<ulong> CountAsync<T>(this ICountAsync<T> target)
        {
            return target.CountAsync(CancellationToken.None);
        }
    }
}