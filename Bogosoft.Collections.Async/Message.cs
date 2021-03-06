﻿namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// A set of predefined messages, intended mostly for uniform exception messages.
    /// </summary>
    public static class Message
    {
        /// <summary>
        /// Get a message indicating that an associated asynchronoys enumerator has not been initialized.
        /// </summary>
        public const string EnumeratorNotInitialized = "This cursor has not been initialized.";

        /// <summary>
        /// Get a message indicating that a queue is empty.
        /// </summary>
        public const string EmptyQueue = "Queue is empty.";
    }
}