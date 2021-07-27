using System;
using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class PropertyDrawerWithContext<T, TContext> : PropertyDrawer
    where TContext : DrawerContextBase<T, TContext>
    {
        protected static IDisposable StartContext(TContext context) => DrawerContextBase<T, TContext>.StartContext(context);

        protected TContext EnsureContext(SerializedProperty property) => DrawerContextBase<T, TContext>.EnsureContext(property);
    }
}