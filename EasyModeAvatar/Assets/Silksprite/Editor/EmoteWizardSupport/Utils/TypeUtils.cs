using System;
using System.Collections.Generic;

namespace Silksprite.EmoteWizardSupport.Utils
{
    public static class TypeUtils
    {
        static readonly Dictionary<Type, Func<object, object>> Duplicators = new Dictionary<Type, Func<object, object>>();
        
        public static Func<object, object> Duplicator()
        {
            return a => Duplicator(a?.GetType())(a);
        }

        static Func<object, object> Duplicator(Type type)
        {
            return Duplicators.TryGetValue(type, out var value) ? value : Duplicators[type] = CreateDuplicator(type);
        }

        static Func<object, object> CreateDuplicator(Type type)
        {
            if (type == null) return v => default;
            if (type.IsValueType) return v => v;
            if (type == typeof(string)) return v => v;
            if (typeof(UnityEngine.Object).IsAssignableFrom(type)) return v => v;
            var constructor = type.GetConstructor(new Type[] { });
            if (constructor != null) { return v => constructor.Invoke(new object[]{}); }
            return v => default;
        }

        public static Func<T, T> Duplicator<T>() => TypeUtils<T>.Duplicator;
    }

    public static class TypeUtils<T>
    {
        static Func<T, T> _duplicator;
        
        public static Func<T, T> Duplicator => _duplicator ?? (_duplicator = CreateDuplicator());

        static Func<T, T> CreateDuplicator()
        {
            var type = typeof(T);
            if (type.IsValueType) return v => v;
            if (type == typeof(string)) return v => v;
            if (typeof(UnityEngine.Object).IsAssignableFrom(type)) return v => v;
            var constructor = type.GetConstructor(new Type[] { });
            if (constructor != null) { return v => Activator.CreateInstance<T>(); }
            return v => default;
        }
    }
}