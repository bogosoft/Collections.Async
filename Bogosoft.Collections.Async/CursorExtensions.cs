using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with cursors.
    /// </summary>
    public static class CursorExtensions
    {
        /// <summary>
        /// Get the item pointed at by the present position of the current cursor.
        /// </summary>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <returns>The item at the present position of the current cursor.</returns>
        public static async Task<T> GetCurrentAsync<T>(this ICursor<T> cursor)
        {
            return await cursor.GetCurrentAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Advance the position of the current cursor and get a value indicating whether or not
        /// the resulting position is a valid record.
        /// </summary>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <returns>
        /// True if the position of the cursor corresponds to a valid record; false otherwise.
        /// </returns>
        public static async Task<bool> MoveNextAsync<T>(this ICursor<T> cursor)
        {
            return await cursor.MoveNextAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}