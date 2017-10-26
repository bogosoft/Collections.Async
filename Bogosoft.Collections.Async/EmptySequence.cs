using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    class EmptySequence<T> : IEnumerableAsync<T>
    {
        public Task<IAsyncEnumerator<T>> GetEnumeratorAsync(CancellationToken token)
        {
            return Task.FromResult<IAsyncEnumerator<T>>(new EmptyCursor<T>());
        }
    }
}