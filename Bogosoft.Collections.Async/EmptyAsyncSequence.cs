namespace Bogosoft.Collections.Async
{
    class EmptyAsyncSequence<T> : IAsyncEnumerable<T>
    {
        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new EmptyAsyncEnumerator<T>();
        }
    }
}