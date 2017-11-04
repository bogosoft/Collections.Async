using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    class EmptySequence<T> : IAsyncEnumerable<T>
    {
        public Task<IAsyncEnumerator<T>> GetEnumeratorAsync(CancellationToken token)
        {
            return Task.FromResult<IAsyncEnumerator<T>>(new EmptyAsyncEnumerator<T>());
        }
    }
}