using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Tools;
using Silksprite.EmoteWizardSupport.UI.Base;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedDrawerRegistry
    {
        static void CollectFromAssembly()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && !type.IsGenericType)
                .Where(type => type.FullName != "Silksprite.EmoteWizardSupport.UI.TypedDrawerRegistry+InvalidDrawer")
                .Where(type => type.FullName != "Silksprite.EmoteWizardSupport.UI.TypedDrawerRegistry`1+InvalidDrawer")
                .Where(type => typeof(ITypedDrawer).IsAssignableFrom(type))
                .ToArray();
            foreach (var type in types)
            {
                var typedDrawerInterfaces = type.FindInterfaces((t, _) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ITypedDrawer<>), null);
                foreach (var @interface in typedDrawerInterfaces)
                {
                    var targetType = @interface.GenericTypeArguments[0];
                    // Expression<Action> a = () => AddDrawer(new IntDrawer());
                    var e = Expression.Lambda<Action>(Expression.Call(
                        typeof(TypedDrawerRegistry)
                            .GetMethod("AddDrawer", BindingFlags.Public | BindingFlags.Static)
                            .MakeGenericMethod(targetType),
                        Expression.New(type)
                    )).Compile();
                    e();
                }
            }
        }
    }
}