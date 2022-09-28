using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static int IntField(string label, ref int value, params GUILayoutOption[] options)
        {
            return value = EditorGUILayout.IntField(label, value, options);
        }

        public static int DelayedIntField(string label, ref int value, params GUILayoutOption[] options)
        {
            return value = EditorGUILayout.DelayedIntField(label, value, options);
        }
    }
}