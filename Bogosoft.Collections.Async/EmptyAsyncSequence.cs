using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    class EmptyAsyncSequence<T> : IAsyncEnumerable<T>
    {
        internal class Enumerator : IAsyncEnumerator<T>
        {
            public T Current => throw new NotSupportedException();

            public void Dispose()
            {
            }

            public Task<bool> MoveNextAsync(CancellationToken token) => Task.FromResult(false);
        }

        public IAsyncEnumerator<T> GetEnumerator() => new Enumerator();
    }
}