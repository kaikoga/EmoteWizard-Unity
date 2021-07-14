using System;

namespace Silksprite.EmoteWizardSupport.Utils
{
    public static class ArrayUtils
    {
        public static void ResizeAndPopulate<T>(ref T[] array, int size)
        {
            ResizeAndPopulate(ref array, size, v => v);
        }

        public static void ResizeAndPopulate<T>(ref T[] array, int size, Func<T, T> generator)
        {
            var i = array.Length;
            var lastElement = i == 0 ? default : array[i - 1];
            Array.Resize(ref array, size);
            for (; i < size; i++)
            {
                array[i] = generator(lastElement);
            }
        }
    }
}