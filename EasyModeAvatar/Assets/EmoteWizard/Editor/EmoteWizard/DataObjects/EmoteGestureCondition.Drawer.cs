using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteGestureCondition))]
    public class EmoteGestureConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            using (new EditorGUI.IndentLevelScope())
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1f;
                EditorGUI.PropertyField(position.SliceH(0.00f, 0.33f), property.FindPropertyRelative("parameter"), new GUIContent(" "));
                EditorGUI.PropertyField(position.SliceH(0.33f, 0.33f), property.FindPropertyRelative("mode"), new GUIContent(" "));
                EditorGUI.PropertyField(position.SliceH(0.66f, 0.33f), property.FindPropertyRelative("handSign"), new GUIContent(" "));
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
    }
}