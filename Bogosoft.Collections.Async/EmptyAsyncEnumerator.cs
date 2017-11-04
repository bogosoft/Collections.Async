using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    struct EmptyAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        public T Current
        {
            get { throw new NotSupportedException(); }
        }

        public void Dispose()
        {
        }

        public Task<bool> MoveNextAsync(CancellationToken token) => Task.FromResult(false);
    }
}