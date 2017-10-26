using NUnit.Framework;
using Should;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async.Tests
{
    [TestFixture, Category("Unit")]
    public class AsyncQueueTests
    {
        [TestCase]
        public async Task CanClearAsyncQueue()
        {
            var ints = Enumerable.Range(0, 16).ToArray();

            var queue = new AsyncQueue<int>(ints);

            (await queue.CountAsync()).ShouldEqual((ulong)ints.Length);

            (await queue.ClearAsync()).ShouldBeTrue();

            (await queue.CountAsync()).ShouldEqual(0u);
        }

        [TestCase]
        public async Task CanCountItemsInAsyncQueue()
        {
            var ints = Enumerable.Range(0, 16).ToArray();

            var queue = new AsyncQueue<int>(ints);

            (await queue.CountAsync()).ShouldEqual((ulong)ints.Length);
        }

        [TestCase]
        public async Task CanDequeueFromAsyncQueue()
        {
            var ints = Enumerable.Range(0, 16).ToArray();

            var queue = new AsyncQueue<int>(ints);

            (await queue.CountAsync()).ShouldEqual((ulong)ints.Length);

            (await queue.DequeueAsync()).ShouldEqual(ints[0]);

            (await queue.CountAsync()).ShouldEqual((ulong)ints.Length - 1);
        }

        [TestCase]
        public async Task CanEnqueueToAsyncQueue()
        {
            var queue = new AsyncQueue<int>();

            (await queue.CountAsync()).ShouldEqual(0u);

            (await queue.EnqueueAsync(1337)).ShouldBeTrue();

            (await queue.CountAsync()).ShouldEqual(1u);
        }

        [TestCase]
        public async Task CanPeekIntoAsyncQueue()
        {
            var ints = Enumerable.Range(0, 16).ToArray();

            var queue = new AsyncQueue<int>(ints);

            (await queue.CountAsync()).ShouldEqual((ulong)ints.Length);

            (await queue.PeekAsync()).ShouldEqual(ints[0]);

            (await queue.CountAsync()).ShouldEqual((ulong)ints.Length);
        }

        [TestCase]
        public async Task CountOnAsyncQueueThrowsOperationCancelledExceptionOnCancelledToken()
        {
            var queue = new AsyncQueue<object>();

            Exception exception = null;

            using (var source = new CancellationTokenSource())
            {
                var token = source.Token;

                source.Cancel();

                try
                {
                    await queue.CountAsync(token);
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }

            exception.ShouldNotBeNull();

            exception.ShouldBeType<OperationCanceledException>();
        }

        [TestCase]
        public async Task DequeueFromAsyncQueueThrowsOperationCancelledExceptionOnCancelledToken()
        {
            var size = 16;

            var queue = new AsyncQueue<int>(Enumerable.Range(0, size));

            (await queue.CountAsync()).ShouldEqual((ulong)size);

            Exception exception = null;

            using (var source = new CancellationTokenSource())
            {
                var token = source.Token;

                source.Cancel();

                try
                {
                    await queue.DequeueAsync(token);
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }

            exception.ShouldNotBeNull();

            exception.ShouldBeType<OperationCanceledException>();

            (await queue.CountAsync()).ShouldEqual((ulong)size);
        }

        [TestCase]
        public async Task EmptyAsyncQueueThrowsInvalidOperationExceptionOnDequeue()
        {
            var queue = new AsyncQueue<object>();

            Exception exception = null;

            try
            {
                await queue.DequeueAsync();
            }
            catch(Exception e)
            {
                exception = e;
            }

            exception.ShouldNotBeNull();

            exception.ShouldBeType<InvalidOperationException>();
        }

        [TestCase]
        public async Task EmptyAsyncQueueThrowsInvalidOperationExceptionOnPeek()
        {
            var queue = new AsyncQueue<object>();

            Exception exception = null;

            try
            {
                await queue.PeekAsync();
            }
            catch (Exception e)
            {
                exception = e;
            }

            exception.ShouldNotBeNull();

            exception.ShouldBeType<InvalidOperationException>();
        }

        [TestCase]
        public async Task EmptyCursorReturnedFromAsyncQueueOnCancelledToken()
        {
            var size = 16;

            var queue = new AsyncQueue<int>(Enumerable.Range(0, size));

            using (var cursor = await queue.GetEnumeratorAsync())
            {
                (await cursor.MoveNextAsync()).ShouldBeTrue();
            }

            (await queue.CountAsync()).ShouldEqual((ulong)size);

            using (var source = new CancellationTokenSource())
            {
                var token = source.Token;

                source.Cancel();

                using (var cursor = await queue.GetEnumeratorAsync(token))
                {
                    (await cursor.MoveNextAsync()).ShouldBeFalse();
                }
            }
        }

        [TestCase]
        public async Task EnqueueOntoAsyncQueueFailsOnCancelledToken()
        {
            var queue = new AsyncQueue<int>();

            (await queue.CountAsync()).ShouldEqual(0u);

            using (var source = new CancellationTokenSource())
            {
                var token = source.Token;

                source.Cancel();

                (await queue.EnqueueAsync(1337, token)).ShouldBeFalse();
            }

            (await queue.CountAsync()).ShouldEqual(0u);
        }

        [TestCase]
        public async Task PeekIntoAsyncQueueThrowsOperationCancelledExceptionOnCancelledToken()
        {
            var size = 16;

            var queue = new AsyncQueue<int>(Enumerable.Range(0, size));

            (await queue.CountAsync()).ShouldEqual((ulong)size);

            Exception exception = null;

            using (var source = new CancellationTokenSource())
            {
                var token = source.Token;

                source.Cancel();

                try
                {
                    await queue.DequeueAsync(token);
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }

            exception.ShouldNotBeNull();

            exception.ShouldBeType<OperationCanceledException>();
        }
    }
}