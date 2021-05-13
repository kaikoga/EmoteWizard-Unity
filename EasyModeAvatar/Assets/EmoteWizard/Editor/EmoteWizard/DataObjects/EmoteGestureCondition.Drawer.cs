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
                EditorGUI.PropertyField(new Rect(position.x + position.width * 0.00f, position.y, position.width * 0.33f, position.height), property.FindPropertyRelative("parameter"), new GUIContent(" "));
                EditorGUI.PropertyField(new Rect(position.x + position.width * 0.33f, position.y, position.width * 0.33f, position.height), property.FindPropertyRelative("mode"), new GUIContent(" "));
                EditorGUI.PropertyField(new Rect(position.x + position.width * 0.66f, position.y, position.width * 0.33f, position.height), property.FindPropertyRelative("handSign"), new GUIContent(" "));
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
    }
}