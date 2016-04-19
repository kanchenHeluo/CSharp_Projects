using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Common.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// return an empty collection instead if the source is null
        /// </summary>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> src)
        {
            return src ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Convert a collection to a HashSet
        /// </summary>
        public static HashSet<TKey> ToHashSet<TSource, TKey>(this IEnumerable<TSource> src, Func<TSource, TKey> keySelector)
        {
            var ret = new HashSet<TKey>();
            foreach (var item in src.EmptyIfNull())
            {
                ret.Add(keySelector(item));
            }
            return ret;
        }

        /// <summary>
        /// To dictionary with ignoring duplications
        /// </summary>
        public static Dictionary<TKey, TSource> ToDictionarySafe<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var ret = new Dictionary<TKey, TSource>();
            foreach (var item in source.EmptyIfNull())
            {
                var key = keySelector(item);
                if (!ret.ContainsKey(key))
                {
                    ret.Add(key, item);
                }
            }
            return ret;
        }

        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null && action != null)
            {
                foreach (var item in source)
                {
                    action(item);
                    yield return item;
                }
            }
        }
    }
}
