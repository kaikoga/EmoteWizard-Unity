using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static float FloatField(string label, ref float value, params GUILayoutOption[] options)
        {
            return value = EditorGUILayout.FloatField(label, value, options);
        }
    }
}