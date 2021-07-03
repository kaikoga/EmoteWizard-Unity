using UnityEditor;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterState))]
    public class ParameterStateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUI.PropertyField(position, property.FindPropertyRelative("value"));
            }
        }
    }
}