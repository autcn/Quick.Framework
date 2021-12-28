using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extension methods for Collections.
    /// </summary>
    public static class QCollectionExtensions
    {
        /// <summary>
        /// Checks whatever given collection object is null or has no item.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source.Contains(item))
            {
                return false;
            }

            source.Add(item);
            return true;
        }
    }
}