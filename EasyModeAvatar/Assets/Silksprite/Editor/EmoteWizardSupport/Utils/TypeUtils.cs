using System;

namespace Silksprite.EmoteWizardSupport.Utils
{
    public static class TypeUtils
    {
        public static Func<object, object> Duplicator()
        {
            return v => null;
        }

        public static Func<object, object> Duplicator(Type type)
        {
            if (type.IsValueType) return v => v;
            if (type == typeof(string)) return v => v;
            return v => default;
        }

        public static Func<T, T> Duplicator<T>()
        {
            var type = typeof(T);
            if (type.IsValueType) return v => v;
            if (type == typeof(string)) return v => v;
            return v => default;
        }
    }
}