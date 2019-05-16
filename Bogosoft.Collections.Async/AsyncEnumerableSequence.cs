using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    class AsyncEnumerableSequence<T> : IAsyncEnumerable<T>
    {
        class Enumerator : IAsyncEnumerator<T>
        {
            readonly IEnumerator<T> enumerator;

            public T Current => enumerator.Current;

            internal Enumerator(IEnumerable<T> enumerable)
            {
                enumerator = enumerable.GetEnumerator();
            }

            public void Dispose()
            {
                enumerator.Dispose();
            }

            public Task<bool> MoveNextAsync(CancellationToken token)
            {
                if (token.IsCancellationRequested)
                {
                    return Task.FromResult(false);
                }
                else
                {
                    return Task.FromResult(enumerator.MoveNext());
                }
            }
        }

        readonly IEnumerable<T> enumerable;

        public AsyncEnumerableSequence(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        public IAsyncEnumerator<T> GetEnumerator() => new Enumerator(enumerable);
    }
}