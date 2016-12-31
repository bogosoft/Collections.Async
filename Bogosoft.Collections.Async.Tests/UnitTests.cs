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
        public void CanConvertEnumerableToCursor()
        {
            var ints = new int[] { 0, 1, 2, 3, 4 };

            (ints is IEnumerable<int>).ShouldBeTrue();

            (ints.ToCursor() is ICursor<int>).ShouldBeTrue();
        }

        [TestCase]
        public async Task CanConvertToArrayAsync()
        {
            var fibonacci = new int[] { 0, 1, 1, 2, 3, 5, 8, 13 };

            var cursor = fibonacci.ToCursor();

            var converted = await cursor.ToArrayAsync();

            (await cursor.MoveNextAsync()).ShouldBeFalse();

            cursor.IsExpended.ShouldBeTrue();

            converted.ShouldBeType<int[]>();

            converted.ShouldBeEqualToSequence(fibonacci);
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

            var cursor = dates.ToCursor();

            var list = await cursor.ToListAsync();

            (await cursor.MoveNextAsync()).ShouldBeFalse();

            cursor.IsExpended.ShouldBeTrue();

            list.ShouldBeType<List<DateTime>>();

            list.ShouldBeEqualToSequence(dates);
        }

        [TestCase]
        public async Task CanCopyToArrayCompletelyAsync()
        {
            var strings = new string[] { "one", "two", "three", "four", "five" };

            var cursor = strings.ToCursor();

            var target = new string[strings.Length];

            (await cursor.CopyToAsync(target, 0, strings.Length)).ShouldEqual(strings.Length);

            (await cursor.MoveNextAsync()).ShouldBeFalse();

            cursor.IsExpended.ShouldBeTrue();

            cursor.IsDisposed.ShouldBeFalse();

            target.ShouldBeEqualToSequence(strings);
        }

        [TestCase]
        public async Task CanCopyToArrayPartiallyAsync()
        {
            var ints = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            var target = new int[ints.Length];

            var cursor = ints.ToCursor();

            (await cursor.CopyToAsync(target, 3, 3)).ShouldEqual(3);

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

            var cursor = guids.ToCursor();

            var list = new List<Guid>(new Guid[count]);

            (await cursor.CopyToAsync(list, 0, count)).ShouldEqual(count);

            (await cursor.MoveNextAsync()).ShouldBeFalse();

            cursor.IsExpended.ShouldBeTrue();

            cursor.IsDisposed.ShouldBeFalse();

            list.ShouldBeEqualToSequence(guids);
        }

        [TestCase]
        public async Task CanTraverseMemoryCursorAsync()
        {
            var odd = new int[] { 1, 3, 5, 7, 9 };

            var cursor = new MemoryCursor<int>(odd);

            cursor.IsDisposed.ShouldBeFalse();

            cursor.IsExpended.ShouldBeFalse();

            var index = 0;

            while(await cursor.MoveNextAsync())
            {
                odd[index++].ShouldEqual(await cursor.GetCurrentAsync());
            }

            (await cursor.MoveNextAsync()).ShouldBeFalse();

            cursor.IsExpended.ShouldBeTrue();

            cursor.IsDisposed.ShouldBeFalse();

            cursor.Dispose();

            cursor.IsDisposed.ShouldBeTrue();
        }

        [TestCase]
        public async Task EmptyCursorThrowsNotSupportedExceptionOnGetCurrentAsync()
        {
            var cursor = Cursor<int>.Empty;

            Exception exception = null;

            try
            {
                await cursor.GetCurrentAsync();
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