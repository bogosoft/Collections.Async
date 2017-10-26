using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    struct EmptyAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        public void Dispose()
        {
        }

        public Task<T> GetCurrentAsync(CancellationToken token)
        {
            throw new NotSupportedException();
        }

        public Task<bool> MoveNextAsync(CancellationToken token) => Task.FromResult(false);
    }
}