using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    class EmptySequence<T> : ITraversable<T>
    {
        public Task<ICursor<T>> GetCursorAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return Task.FromResult<ICursor<T>>(new EmptyCursor<T>());
        }
    }
}