using Should;
using System.Collections.Generic;
using System.Linq;

namespace Bogosoft.Collections.Async.Tests
{
    internal static class EnumerableExtensions
    {
        internal static void ShouldBeEqualToSequence<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            target.SequenceEqual(source).ShouldBeTrue();
        }
    }
}