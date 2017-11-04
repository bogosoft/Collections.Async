using NUnit.Framework;
using Should;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async.Tests
{
    [TestFixture, Category("Unit")]
    public class UnitTests
    {
        protected IEnumerable<int> RandomIntegers
        {
            get
            {
                var random = new Random();

                var count = random.Next(32768, 65535);

                for(var i = 0; i < count; i++)
                {
                    yield return random.Next(-32768, 32767);
                }
            }
        }

        [TestCase]
        public void CanConvertEnumerableToAsyncEnumerable()
        {
            var ints = new int[] { 0, 1, 2, 3, 4 };

            (ints is IEnumerable<int>).ShouldBeTrue();

            (ints.ToTraversable() is IAsyncEnumerable<int>).ShouldBeTrue();
        }

        [TestCase]
        public async Task CanConvertToArrayAsync()
        {
            var fibonacci = new int[] { 0, 1, 1, 2, 3, 5, 8, 13 };

            var traversable = fibonacci.ToTraversable();

            (traversable is IAsyncEnumerable<int>).ShouldBeTrue();

            var converted = await traversable.ToArrayAsync();

            converted.ShouldBeType<int[]>();

            converted.SequenceEqual(fibonacci).ShouldBeTrue();
        }

        [TestCase]
        public async Task CanConvertToListAsync()
        {
            var dates = new DateTime[]
            {
                DateTime.Now,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            };

            var traversable = dates.ToTraversable();

            (traversable is IAsyncEnumerable<DateTime>).ShouldBeTrue();

            var list = await traversable.ToListAsync();

            list.ShouldBeType<List<DateTime>>();

            list.SequenceEqual(dates).ShouldBeTrue();
        }

        [TestCase]
        public async Task CanConvertAsyncEnumerableToArrayAsync()
        {
            var ints = RandomIntegers.ToArray();

            var traversable = ints.ToTraversable();

            ints.SequenceEqual(await traversable.ToArrayAsync()).ShouldBeTrue();
        }

        [TestCase]
        public async Task CanCopyToArrayCompletelyAsync()
        {
            var strings = new string[] { "one", "two", "three", "four", "five" };

            var traversable = strings.ToTraversable();

            var target = new string[strings.Length];

            (await traversable.CopyToAsync(target, 0, strings.Length)).ShouldEqual(strings.Length);

            target.SequenceEqual(strings).ShouldBeTrue();
        }

        [TestCase]
        public async Task CanCopyToArrayPartiallyAsync()
        {
            var ints = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            var target = new int[ints.Length];

            var traversable = ints.ToTraversable();

            (await traversable.CopyToAsync(target, 3, 3)).ShouldEqual(3);

            for(var i = 0; i < 4; i++)
            {
                target[i].ShouldEqual(0);
            }

            target[4].ShouldEqual(1);
            target[5].ShouldEqual(2);
        }

        [TestCase]
        public async Task CanCopyToListCompletelyAsync()
        {
            var count = new Random().Next(32, 64);

            var guids = new Guid[count];

            for(var i = 0; i < count; i++)
            {
                guids[i] = Guid.NewGuid();
            }

            var traversable = guids.ToTraversable();

            var list = new List<Guid>(new Guid[count]);

            (await traversable.CopyToAsync(list, 0, count)).ShouldEqual(count);

            list.SequenceEqual(guids).ShouldBeTrue();
        }

        [TestCase]
        public async Task CanCreateSequenceFromVariadicArgumentsAsync()
        {
            var sequence = AsyncEnumerable<int>.From(0, 1, 2, 3, 4);

            (sequence is IAsyncEnumerable<int>).ShouldBeTrue();

            using (var enumerator = sequence.GetEnumerator())
            {
                (await enumerator.MoveNextAsync()).ShouldBeTrue();

                enumerator.Current.ShouldEqual(0);
            }
        }

        [TestCase]
        public void EmptyAsyncEnumeratorThrowsNotSupportedExceptionOnGetCurrent()
        {
            var cursor = AsyncEnumerator<int>.Empty;

            Exception exception = null;

            try
            {
                var ignored = cursor.Current;
            }
            catch (NotSupportedException e)
            {
                exception = e;
            }

            exception.ShouldNotBeNull();

            exception.ShouldBeType<NotSupportedException>();
        }
    }
}