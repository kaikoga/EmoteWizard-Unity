using System;
using Silksprite.EmoteWizardSupport.UI.Base;
using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class TypedDrawerWithContext<T, TContext> : TypedDrawerBase<T>
    where TContext : DrawerContextBase<T, TContext>
    {
        protected static IDisposable StartContext(TContext context) => DrawerContextBase<T, TContext>.StartContext(context);
        
        protected TContext EnsureContext(SerializedProperty property) => DrawerContextBase<T, TContext>.EnsureContext(property);

        protected TContext EnsureContext() => DrawerContextBase<T, TContext>.EnsureContext();
    }
}