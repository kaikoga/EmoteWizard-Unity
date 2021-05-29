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
                EditorGUI.PropertyField(new Rect(position.x + position.width * 0.00f, position.y + PropertyDrawerUITools.LineTop(0), position.width * 0.2f, PropertyDrawerUITools.LineHeight(1)), property.FindPropertyRelative("value"), new GUIContent(" "));
                EditorGUI.PropertyField(new Rect(position.x + position.width * 0.20f, position.y + PropertyDrawerUITools.LineTop(0), position.width * 0.8f, PropertyDrawerUITools.LineHeight(1)), property.FindPropertyRelative("clip"), new GUIContent(" "));
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
    }
}