using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Collections.Base
{
    public abstract class ListDrawerBase : ListHeaderDrawerBase
    {
        public abstract string PagerItemName(SerializedProperty property, int index);
    }
}