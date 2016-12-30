namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with cursors.
    /// </summary>
    /// <typeparam name="T">The type of the object being traversed.</typeparam>
    public static class Cursor<T>
    {
        /// <summary>
        /// Get an empty cursor.
        /// </summary>
        public static ICursor<T> Empty
        {
            get { return new EmptyCursor<T>(); }
        }
    }
}