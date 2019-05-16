using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Represents a template for any method capable of generating an awaitable value of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the awaitable value.</typeparam>
    /// <param name="token">A cancellation token.</param>
    /// <returns>An awaitable value of the specified type.</returns>
    public delegate Task<T> AsyncValue<T>(CancellationToken token);

    /// <summary>
    /// Extended functionality for the <see cref="AsyncValue{T}"/> contract.
    /// </summary>
    public static class AsyncValueExtensions
    {
        /// <summary>
        /// Resolve the current asynchronous value into an awaitable value.
        /// </summary>
        /// <typeparam name="T">The type of the awaitable value.</typeparam>
        /// <param name="self">The current asynchronous value provider.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>An awaitable value of the specified type.</returns>
        public static Task<T> ResolveAsync<T>(this AsyncValue<T> self, CancellationToken token = default(CancellationToken))
        {
            return self.Invoke(token);
        }
    }
}