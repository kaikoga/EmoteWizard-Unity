using Silksprite.EmoteWizardSupport.Collections.Generic.Base;
using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class TypedDrawerWithContext<T, TContext> : TypedDrawerBase<T>
    where TContext : DrawerContextBase<T, TContext>
    {
        protected static TContext StartContext(TContext context) => DrawerContext<T, TContext>.StartContext(context);
        
        protected TContext EnsureContext(SerializedProperty property) => DrawerContext<T, TContext>.EnsureContext(property);

        protected TContext EnsureContext() => DrawerContext<T, TContext>.EnsureContext();
    }
}