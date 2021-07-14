using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class PropertyDrawerWithContext<T, TContext> : PropertyDrawer
    where TContext : DrawerContextBase<T, TContext>
    {
        protected static TContext StartContext(TContext context) => DrawerContext<T, TContext>.StartContext(context);

        internal static void EndContext() => DrawerContext<T, TContext>.EndContext();

        protected TContext EnsureContext(SerializedProperty property) => DrawerContext<T, TContext>.EnsureContext(property);
    }
}