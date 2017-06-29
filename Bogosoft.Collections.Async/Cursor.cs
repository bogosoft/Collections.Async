namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Provides a set of static methods for working with cursors.
    /// </summary>
    public static class Cursor<T>
    {
        /// <summary>
        /// Get an empty cursor.
        /// </summary>
        public static ICursor<T> Empty => new EmptyCursor<T>();
    }
}