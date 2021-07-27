using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class PropertyDrawerWithContext<T, TContext> : PropertyDrawer
    where TContext : DrawerContextBase<T, TContext>
    {
        protected TContext EnsureContext(SerializedProperty property) => DrawerContextBase<T, TContext>.EnsureContext(property);
    }
}