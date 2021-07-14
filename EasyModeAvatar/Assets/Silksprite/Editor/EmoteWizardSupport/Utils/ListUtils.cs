using System;
using System.Collections.Generic;

namespace Silksprite.EmoteWizardSupport.Utils
{
    public static class ListUtils
    {
        public static void ResizeAndPopulate<T>(ref List<T> list, int size)
        {
            ResizeAndPopulate(ref list, size, v => v);
        }

        public static void ResizeAndPopulate<T>(ref List<T> list, int size, Func<T, T> generator)
        {
            var i = list.Count;
            var lastElement = i == 0 ? default : list[i - 1];
            if (size < i)
            {
                list.RemoveRange(size, i - size);
            }
            for (; i < size; i++)
            {
                list.Add(generator(lastElement));
            }
        }
    }
}