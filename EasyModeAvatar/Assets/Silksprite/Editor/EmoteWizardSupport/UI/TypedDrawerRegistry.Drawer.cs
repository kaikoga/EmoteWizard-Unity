using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Silksprite.EmoteWizardSupport.Base;
using UnityEngine;

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
                    _untypedDrawers = new Dictionary<Type, UntypedDrawer>();
                    CollectFromAssembly();
                }
                return _untypedDrawers;
            }
        }

        static readonly UntypedDrawer Invalid = new UntypedDrawer<object>(new InvalidDrawer());

        [UsedImplicitly]
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
    }

    public static class TypedDrawerRegistry<T>
    {
        static ITypedDrawer<T> _drawer;

        internal static ITypedDrawer<T> Drawer => _drawer = _drawer ?? (ITypedDrawer<T>) TypedDrawerRegistry.Drawer(typeof(T)).typed;
    }
}