using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    struct EmptyCursor<T> : ICursor<T>
    {
        bool disposed;

        public bool IsDisposed => disposed;

        public bool IsExpended => true;

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
            }
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