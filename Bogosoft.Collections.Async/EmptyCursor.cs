using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    struct EmptyCursor<T> : ICursor<T>
    {
        public void Dispose()
        {
        }

        public Task<T> GetCurrentAsync(CancellationToken token)
        {
            throw new NotSupportedException();
        }

        public Task<bool> MoveNextAsync(CancellationToken token)
        {
            return Task.FromResult(false);
        }
    }
}