using System;
using System.Collections;
using System.Collections.Generic;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedDrawerRegistry
    {
        static Dictionary<Type, UntypedDrawer> _untypedDrawers;

        static Dictionary<Type, UntypedDrawer> UntypedDrawers
        {
            get
            {
                if (_untypedDrawers == null)
                {
                    _untypedDrawers = new Dictionary<Type, UntypedDrawer>
                    {
                        {typeof(int), new UntypedDrawer<int>(new IntDrawer())},
                        {typeof(float), new UntypedDrawer<float>(new FloatDrawer())},
                        {typeof(bool), new UntypedDrawer<bool>(new BoolDrawer())},
                        {typeof(string), new UntypedDrawer<string>(new StringDrawer())},
                        {typeof(Enum), new UntypedDrawer<Enum>(new EnumDrawer())},
                        {typeof(IList), new UntypedDrawer<IList>(new ListDrawer())}
                    };
                }
                return _untypedDrawers;
            }
        }

        static readonly UntypedDrawer Invalid = new UntypedDrawer<object>(new InvalidDrawer());
        public static void AddDrawer<T>(ITypedDrawer<T> typedDrawer) => UntypedDrawers[typeof(T)] = new UntypedDrawer<T>(typedDrawer);

        public static UntypedDrawer Drawer(Type type)
        {
            if (type == null) return Invalid;
            if (UntypedDrawers.TryGetValue(type, out var drawer)) return drawer;
            {
                var baseDrawer = Drawer(type.BaseType);
                if (baseDrawer != Invalid)
                {
                    return UntypedDrawers[type] = baseDrawer;
                }
            }
            foreach (var interfaceType in type.GetInterfaces())
            {
                var baseDrawer = Drawer(interfaceType);
                if (baseDrawer != Invalid)
                {
                    return UntypedDrawers[type] = baseDrawer;
                }
            }

            return UntypedDrawers[type] = Invalid;
        }

        public static ITypedDrawer<T> Drawer<T>()
        {
            return (ITypedDrawer<T>) Drawer(typeof(T)).typed;
        }
    }
}