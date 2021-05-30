using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.EditorUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterState))]
    public class ParameterStateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1f;
                EditorGUI.PropertyField(position.SliceH(0.0f, 0.2f), property.FindPropertyRelative("value"), new GUIContent(" "));
                EditorGUI.PropertyField(position.SliceH(0.2f, 0.4f), property.FindPropertyRelative("gestureClip"), new GUIContent(" "));
                EditorGUI.PropertyField(position.SliceH(0.6f, 0.4f), property.FindPropertyRelative("fxClip"), new GUIContent(" "));
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
    }
}