using Silksprite.EmoteWizard.DataObjects;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.UI
{
    public static class EmoteWizardGUILayout
    {
        public static void PropAsParameterValue(LocalizedProperty lop, ParameterItemKind itemKind)
        {
            EmoteWizardGUI.PropAsParameterValue(EditorGUILayout.GetControlRect(), lop, itemKind);
        }

        public static void DummyController(LocalizedProperty lop, RuntimeAnimatorController? dummyController)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                LEditorGUILayout.ObjectField(lop.Loc, dummyController, false);
            }
        }
    }
}
