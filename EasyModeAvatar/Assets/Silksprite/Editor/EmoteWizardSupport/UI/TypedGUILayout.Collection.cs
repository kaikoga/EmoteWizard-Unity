using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static List<T> ListField<T>(string label, ref List<T> value, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label, "Not Implemented");
            return value;
        }
    }
}