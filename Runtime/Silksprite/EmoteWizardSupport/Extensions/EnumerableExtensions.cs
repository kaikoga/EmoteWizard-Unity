using System;
using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector, (key, values) => values.First());
        }
    }
}