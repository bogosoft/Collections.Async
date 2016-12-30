using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Collections.Async
{
    /// <summary>
    /// Extended functionality for the <see cref="ICursor{T}"/> contract.
    /// </summary>
    public static class CursorExtensions
    {
        /// <summary>
        /// Copy the records from the present position of the current cursor to a dictionary.
        /// </summary>
        /// <typeparam name="TSource">The type of the item being traversed.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="dictionary">A target dictionary to copy records into.</param>
        /// <param name="keySelector">
        /// A mapping strategy responsible for mapping an item of the source type to an item of the key type.
        /// </param>
        /// <param name="valueSelector">
        /// A mapping strategy responsible for mapping an item of the source type to an item of the value type.
        /// </param>
        /// <param name="count">
        /// A value indicating the number of items to copy to the target dictionary.
        /// </param>
        /// <returns>
        /// A value indicating the actual number of records copied.
        /// </returns>
        public static async Task<int> CopyToAsync<TSource, TKey, TValue>(
            this ICursor<TSource> cursor,
            IDictionary<TKey, TValue> dictionary,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector,
            int count = int.MaxValue
            )
        {
            return await cursor.CopyToAsync(
                dictionary,
                keySelector,
                valueSelector,
                CancellationToken.None,
                count
                ).ConfigureAwait(false);
        }

        /// <summary>
        /// Copy the records from the present position of the current cursor to a dictionary.
        /// </summary>
        /// <typeparam name="TSource">The type of the item being traversed.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="dictionary">A target dictionary to copy records into.</param>
        /// <param name="keySelector">
        /// A mapping strategy responsible for mapping an item of the source type to an item of the key type.
        /// </param>
        /// <param name="valueSelector">
        /// A mapping strategy responsible for mapping an item of the source type to an item of the value type.
        /// </param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <param name="count">
        /// A value indicating the number of items to copy to the target dictionary.
        /// </param>
        /// <returns>A value indicating the actual number of records copied.</returns>
        public static async Task<int> CopyToAsync<TSource, TKey, TValue>(
            this ICursor<TSource> cursor,
            IDictionary<TKey, TValue> dictionary,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector,
            CancellationToken token,
            int count = int.MaxValue
            )
        {
            TSource item;

            var copied = 0;

            while(copied < count && await cursor.MoveNextAsync(token).ConfigureAwait(false))
            {
                item = await cursor.GetCurrentAsync(token).ConfigureAwait(false);

                dictionary.Add(keySelector.Invoke(item), valueSelector.Invoke(item));
            }

            return copied;
        }

        /// <summary>
        /// Copy records from the current cursor to a given <see cref="IList{T}"/> implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object being traversed and copied.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="target">A target <see cref="IList{T}"/> implementation.</param>
        /// <returns>A value indicating the actual number of records copied.</returns>
        public static async Task<int> CopyToAsync<T>(this ICursor<T> cursor, IList<T> target)
        {
            return await cursor.CopyToAsync(
                target,
                0,
                target.Count,
                CancellationToken.None
                ).ConfigureAwait(false);
        }

        /// <summary>
        /// Copy records from the current cursor to a given <see cref="IList{T}"/> implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object being traversed and copied.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="target">A target <see cref="IList{T}"/> implementation.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>A value indicating the actual number of records copied.</returns>
        public static async Task<int> CopyToAsync<T>(
            this ICursor<T> cursor,
            IList<T> target,
            CancellationToken token
            )
        {
            return await cursor.CopyToAsync(target, 0, target.Count, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Copy records from the current cursor to a given <see cref="IList{T}"/> implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object being traversed and copied.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="target">A target <see cref="IList{T}"/> implementation.</param>
        /// <param name="start">
        /// A value corresponding to the position in the target with which to begin copying records.
        /// </param>
        /// <param name="count">
        /// A value indicating the number of items to copy to the target dictionary.
        /// </param>
        /// <returns>A value indicating the actual number of records copied.</returns>
        public static async Task<int> CopyToAsync<T>(
            this ICursor<T> cursor,
            IList<T> target,
            int start,
            int count
            )
        {
            return await cursor.CopyToAsync(target, start, count, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Copy records from the current cursor to a given <see cref="IList{T}"/> implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object being traversed and copied.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="target">A target <see cref="IList{T}"/> implementation.</param>
        /// <param name="start">
        /// A value corresponding to the position in the target with which to begin copying records.
        /// </param>
        /// <param name="count">
        /// A value indicating the number of items to copy to the target dictionary.
        /// </param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>A value indicating the actual number of records copied.</returns>
        public static async Task<int> CopyToAsync<T>(
            this ICursor<T> cursor,
            IList<T> target,
            int start,
            int count,
            CancellationToken token
            )
        {
            var copied = 0;

            while(copied < count && await cursor.MoveNextAsync(token).ConfigureAwait(false))
            {
                target[start + copied++] = await cursor.GetCurrentAsync(token).ConfigureAwait(false);
            }

            return copied;
        }

        /// <summary>
        /// Get the item pointed at by the present position of the current cursor.
        /// </summary>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <returns>The item at the present position of the current cursor.</returns>
        public static async Task<T> GetCurrentAsync<T>(this ICursor<T> cursor)
        {
            return await cursor.GetCurrentAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Advance the position of the current cursor and get a value indicating whether or not
        /// the resulting position is a valid record.
        /// </summary>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <returns>
        /// True if the position of the cursor corresponds to a valid record; false otherwise.
        /// </returns>
        public static async Task<bool> MoveNextAsync<T>(this ICursor<T> cursor)
        {
            return await cursor.MoveNextAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert the current cursor to an array.
        /// </summary>
        /// <typeparam name="T">The type of the object being traversed.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="dispose">
        /// A value indicating whether or not to dispose of the current cursor once conversion has finished.
        /// </param>
        /// <returns>An array of objects of the traversed type.</returns>
        public static async Task<T[]> ToArrayAsync<T>(this ICursor<T> cursor, bool dispose = true)
        {
            return await cursor.ToArrayAsync(CancellationToken.None, dispose).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert the current cursor to an array.
        /// </summary>
        /// <typeparam name="T">The type of the object being traversed.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <param name="dispose">
        /// A value indicating whether or not to dispose of the current cursor once conversion has finished.
        /// </param>
        /// <returns>An array of objects of the traversed type.</returns>
        public static async Task<T[]> ToArrayAsync<T>(
            this ICursor<T> cursor,
            CancellationToken token,
            bool dispose = true
            )
        {
            var array = new T[8];

            var index = 0;

            while(await cursor.MoveNextAsync(token).ConfigureAwait(false))
            {
                if(index == array.Length)
                {
                    Array.Resize(ref array, array.Length * 2);
                }

                array[index++] = await cursor.GetCurrentAsync(token).ConfigureAwait(false);
            }

            if (dispose)
            {
                cursor.Dispose();
            }

            if(index < array.Length)
            {
                Array.Resize(ref array, index);
            }

            return array;
        }

        /// <summary>
        /// Convert the current cursor into an in-memory list.
        /// </summary>
        /// <typeparam name="T">The type of the object being traversed.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="dispose">
        /// A value indicating whether or not to dispose of the current cursor once conversion has finished.
        /// </param>
        /// <returns>A list of objects of the traversed type.</returns>
        public static async Task<List<T>> ToListAsync<T>(this ICursor<T> cursor, bool dispose = true)
        {
            return await cursor.ToListAsync(CancellationToken.None, dispose).ConfigureAwait(false);
        }

        /// <summary>
        /// Convert the current cursor into an in-memory list.
        /// </summary>
        /// <typeparam name="T">The type of the object being traversed.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <param name="dispose">
        /// A value indicating whether or not to dispose of the current cursor once conversion has finished.
        /// </param>
        /// <returns>A list of objects of the traversed type.</returns>
        public static async Task<List<T>> ToListAsync<T>(
            this ICursor<T> cursor,
            CancellationToken token,
            bool dispose = true
            )
        {
            var list = new List<T>();

            while(await cursor.MoveNextAsync(token).ConfigureAwait(false))
            {
                list.Add(await cursor.GetCurrentAsync(token).ConfigureAwait(false));
            }

            if(dispose)
            {
                cursor.Dispose();
            }

            return list;
        }

        /// <typeparam name="TSource">The type of the item being traversed.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="keySelector">
        /// A mapping strategy responsible for mapping an item of the source type to an item of the key type.
        /// </param>
        /// <param name="valueSelector">
        /// A mapping strategy responsible for mapping an item of the source type to an item of the value type.
        /// </param>
        /// <param name="dispose">
        /// A value indicating whether or not to dispose of the current cursor once conversion has finished.
        /// </param>
        /// <returns>A dictionary of copied items.</returns>
        public static async Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(
            this ICursor<TSource> cursor,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector,
            bool dispose = true
            )
        {
            return await cursor.ToDictionaryAsync(
                keySelector,
                valueSelector,
                CancellationToken.None,
                dispose
                ).ConfigureAwait(false);
        }

        /// <typeparam name="TSource">The type of the item being traversed.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="cursor">The current <see cref="ICursor{T}"/> implementation.</param>
        /// <param name="keySelector">
        /// A mapping strategy responsible for mapping an item of the source type to an item of the key type.
        /// </param>
        /// <param name="valueSelector">
        /// A mapping strategy responsible for mapping an item of the source type to an item of the value type.
        /// </param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <param name="dispose">
        /// A value indicating whether or not to dispose of the current cursor once conversion has finished.
        /// </param>
        /// <returns>A dictionary of copied items.</returns>
        public static async Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(
            this ICursor<TSource> cursor,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector,
            CancellationToken token,
            bool dispose = true
            )
        {
            TSource item;

            var dictionary = new Dictionary<TKey, TValue>();

            while(await cursor.MoveNextAsync(token).ConfigureAwait(false))
            {
                item = await cursor.GetCurrentAsync(token).ConfigureAwait(false);

                dictionary.Add(keySelector.Invoke(item), valueSelector.Invoke(item));
            }

            return dictionary;
        }
    }
}