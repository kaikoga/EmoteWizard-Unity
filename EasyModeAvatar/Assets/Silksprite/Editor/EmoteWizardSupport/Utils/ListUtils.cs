using System;
using System.Collections;
using System.Collections.Generic;

namespace Silksprite.EmoteWizardSupport.Utils
{
    public static class ListUtils
    {
        public static void ResizeAndPopulate<T>(ref List<T> list, int size)
        {
            ResizeAndPopulate(ref list, size, TypeUtils.Duplicator<T>());
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

        public static void ResizeAndPopulate(ref IList list, int size)
        {
            ResizeAndPopulate(ref list, size, TypeUtils.Duplicator());
        }

        public static void ResizeAndPopulate(ref IList list, int size, Func<object, object> generator)
        {
            var i = list.Count;
            var lastElement = i == 0 ? default : list[i - 1];
            for (; size < i; i--)
            {
                list.RemoveAt(size);
            }
            for (; i < size; i++)
            {
                list.Add(generator(lastElement));
            }
        }
    }
}