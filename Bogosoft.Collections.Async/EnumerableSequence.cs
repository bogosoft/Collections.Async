﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    class EnumerableSequence<T> : ITraversable<T>
    {
        struct Cursor : ICursor<T>
        {
            IEnumerator<T> enumerator;

            internal Cursor(IEnumerable<T> enumerable)
            {
                enumerator = enumerable.GetEnumerator();
            }

            public void Dispose()
            {
                enumerator.Dispose();
            }

            public Task<T> GetCurrentAsync(CancellationToken token)
            {
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
                    return Task.FromResult(enumerator.MoveNext());
                }
            }
        }

        IEnumerable<T> enumerable;

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