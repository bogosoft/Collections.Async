using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Extended functionality for the <see cref="ICount{T}"/> contract.
    /// </summary>
    public static class CountExtensions
    {
        /// <summary>
        /// Count the number of objects of the specified type. Calling this method is equivalent to calling
        /// <see cref="ICount{T}.CountAsync(CancellationToken)"/> with a value of
        /// <see cref="CancellationToken.None"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects that can be counted.</typeparam>
        /// <param name="target">The current <see cref="ICount{T}"/> implementation.</param>
        /// <returns>
        /// A value corresponding to the number of objects the specified tpye.
        /// </returns>
        public static Task<ulong> CountAsync<T>(this ICount<T> target)
        {
            return target.CountAsync(CancellationToken.None);
        }
    }
}