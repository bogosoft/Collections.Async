using System;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Extended functionality for the <see cref="EventHandler"/> class.
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Equivalent to calling <see cref="EventHandler.Invoke(object, EventArgs)"/>
        /// with a null sender and a value of <see cref="EventArgs.Empty"/>.
        /// </summary>
        /// <param name="handler"></param>
        public static void Invoke(this EventHandler handler)
        {
            handler.Invoke(null, EventArgs.Empty);
        }
    }
}