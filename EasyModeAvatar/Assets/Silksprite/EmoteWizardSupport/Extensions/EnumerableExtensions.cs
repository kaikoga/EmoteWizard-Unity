using System;
using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.DistinctBy(keySelector, values => values.First());
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector, Func<IEnumerable<T>, T> valueSelector)
        {
            return enumerable.GroupBy(keySelector).Select(valueSelector);
        }
    }
}