using Silksprite.EmoteWizard.DataObjects;
using Silksprite.Loch;
using UnityEditor;

namespace Silksprite.EmoteWizard.UI
{
    public static class EmoteWizardGUILayout
    {
        public static void PropAsParameterValue(LocalizedProperty lop, ParameterItemKind itemKind)
        {
            EmoteWizardGUI.PropAsParameterValue(EditorGUILayout.GetControlRect(), lop, itemKind);
        }
    }
}
