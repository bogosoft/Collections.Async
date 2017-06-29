using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    class EnumerableSequence<T> : ITraversable<T>
    {
        struct Cursor : ICursor<T>
        {
            private IEnumerator<T> enumerator;

            private bool active, disposed;

            public bool IsDisposed => disposed;

            public bool IsExpended => !active;

            internal Cursor(IEnumerable<T> enumerable)
            {
                active = disposed = false;

                enumerator = enumerable.GetEnumerator();
            }

            public void Dispose()
            {
                if (!disposed)
                {
                    enumerator.Dispose();

                    disposed = true;
                }
            }

            public Task<T> GetCurrentAsync(CancellationToken token)
            {
                token.ThrowIfCancellationRequested();

                if (!active)
                {
                    throw new InvalidOperationException("This cursor has not been initialized.");
                }

                return Task.FromResult(enumerator.Current);
            }

            public Task<bool> MoveNextAsync(CancellationToken token)
            {
                if (token.IsCancellationRequested)
                {
                    return Task.FromResult(false);
                }
                else
                {
                    return Task.FromResult(active = enumerator.MoveNext());
                }
            }
        }

        private IEnumerable<T> enumerable;

        public EnumerableSequence(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        public Task<ICursor<T>> GetCursorAsync(CancellationToken token)
        {
            ICursor<T> cursor;

            if (token.IsCancellationRequested)
            {
                cursor = Cursor<T>.Empty;
            }
            else
            {
                cursor = new Cursor(enumerable);
            }

            return Task.FromResult(cursor);
        }
    }
}