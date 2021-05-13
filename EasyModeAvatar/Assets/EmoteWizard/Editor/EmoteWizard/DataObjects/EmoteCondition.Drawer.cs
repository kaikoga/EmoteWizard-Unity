using UnityEditor;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteCondition))]
    public class EmoteConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            using (new EditorGUI.IndentLevelScope())
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1f;
                EditorGUI.PropertyField(new Rect(position.x + position.width * 0.00f, position.y, position.width * 0.50f, position.height), property.FindPropertyRelative("parameter"), new GUIContent(" "));
                EditorGUI.PropertyField(new Rect(position.x + position.width * 0.50f, position.y, position.width * 0.25f, position.height), property.FindPropertyRelative("mode"), new GUIContent(" "));
                EditorGUI.PropertyField(new Rect(position.x + position.width * 0.75f, position.y, position.width * 0.25f, position.height), property.FindPropertyRelative("threshold"), new GUIContent(" "));
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
    }
}