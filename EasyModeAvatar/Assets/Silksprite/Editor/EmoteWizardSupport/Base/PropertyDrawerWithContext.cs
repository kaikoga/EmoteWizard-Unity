using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class PropertyDrawerWithContext<T, TContext> : PropertyDrawer
    where TContext : DrawerContextBase<T, TContext>
    {
        protected static TContext StartContext(TContext context) => DrawerContext<T, TContext>.StartContext(context);

        protected TContext EnsureContext(SerializedProperty property) => DrawerContext<T, TContext>.EnsureContext(property);
    }
}