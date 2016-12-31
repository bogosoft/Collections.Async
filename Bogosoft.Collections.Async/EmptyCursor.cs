using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    internal struct EmptyCursor<T> : ICursor<T>
    {
        public bool IsDisposed { get; private set; }

        public bool IsExpended
        {
            get { return true; }
        }

        public void Dispose()
        {
            IsDisposed = true;
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