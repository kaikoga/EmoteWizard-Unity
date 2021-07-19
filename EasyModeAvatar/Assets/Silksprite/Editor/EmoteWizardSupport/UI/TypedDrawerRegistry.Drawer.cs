using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizardSupport.UI.Base;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedDrawerRegistry
    {
        static Dictionary<Type, UntypedDrawer> _untypedDrawers;
        static readonly List<Type> DrawerGenerators = new List<Type>();

        public static Dictionary<Type, UntypedDrawer> UntypedDrawers
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
        [UsedImplicitly]
        public static void AddDrawerGenerator(Type drawerType) => DrawerGenerators.Add(drawerType);

        public static UntypedDrawer Drawer(Type type)
        {
            if (type == null) return Invalid;
            if (UntypedDrawers.TryGetValue(type, out var cached)) return cached;

            // XXX: This mess could be done better
            foreach (var drawerType in DrawerGenerators)
            {
                for (var typeParameter = type; typeParameter != null; typeParameter = typeParameter.GenericTypeArguments.FirstOrDefault())
                {
                    Type concreteDrawerType;
                    try
                    {
                        concreteDrawerType = drawerType.MakeGenericType(typeParameter);
                    }
                    catch (ArgumentException)
                    {
                        continue; // Type constraint failure, try next one...
                    }

                    var iDrawerType = typeof(ITypedDrawer<>).MakeGenericType(type);
                    if (!iDrawerType.IsAssignableFrom(concreteDrawerType))
                    {
                        continue; // Unmatched drawer type, try next one...
                    }

                    var concreteUntypedDrawerType = typeof(UntypedDrawer<>).MakeGenericType(type);
                    var genericDrawer = concreteDrawerType.GetConstructor(new Type[] { }).Invoke(new object[] { });
                    var untypedDrawer = (UntypedDrawer) concreteUntypedDrawerType.GetConstructor(new[] {iDrawerType}).Invoke(new[] {genericDrawer});

                    if (untypedDrawer != null)
                    {
                        return UntypedDrawers[type] = untypedDrawer;
                    }
                }
            }
            if (UntypedDrawers.TryGetValue(type, out var drawer)) return drawer;
            {
                var baseDrawer = Drawer(type.BaseType);
                if (baseDrawer != Invalid)
                {
                    return UntypedDrawers[type] = baseDrawer.Subtype(type);
                }
            }
            foreach (var interfaceType in type.GetInterfaces())
            {
                var baseDrawer = Drawer(interfaceType);
                if (baseDrawer != Invalid)
                {
                    return UntypedDrawers[type] = baseDrawer.Subtype(type);
                }
            }
            return UntypedDrawers[type] = Invalid;
        }
    }

    public static class TypedDrawerRegistry<T>
    {
        static ITypedDrawer<T> _drawer;

        internal static ITypedDrawer<T> Drawer => _drawer = _drawer ?? TypedDrawerRegistry.Drawer(typeof(T)).Typed as ITypedDrawer<T> ?? new InvalidDrawer();

        class InvalidDrawer : TypedDrawerBase<T>, IInvalidTypedDrawer
        {
            public override void OnGUI(Rect position, ref T property, GUIContent label)
            {
                EditorGUI.LabelField(position, label, new GUIContent($"???{typeof(T).Name} Drawer???"));
            }
        }
    }
}