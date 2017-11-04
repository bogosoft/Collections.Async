namespace Bogosoft.Collections.Async
{
    class EmptySequence<T> : IAsyncEnumerable<T>
    {
        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new EmptyAsyncEnumerator<T>();
        }
    }
}