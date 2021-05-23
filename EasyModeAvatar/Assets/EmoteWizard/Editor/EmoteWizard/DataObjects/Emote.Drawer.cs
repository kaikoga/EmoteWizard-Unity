using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.EditorUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(Emote))]
    public class EmoteDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Box(position, GUIContent.none);
            position = PropertyDrawerUITools.InsideBox(position);
            var cursor = new Rect(position.x, position.y, position.width, PropertyDrawerUITools.LineHeight(1));
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1f;
                EditorGUI.PropertyField(cursor, property.FindPropertyRelative("gesture1"), new GUIContent(" "));
                cursor.y += PropertyDrawerUITools.LineTop(1f);
                EditorGUI.PropertyField(cursor, property.FindPropertyRelative("gesture2"), new GUIContent(" "));
                cursor.y += PropertyDrawerUITools.LineTop(1f);
                EditorGUIUtility.labelWidth = labelWidth;
                EditorGUI.PropertyField(cursor, property.FindPropertyRelative("conditions"), true);
                cursor.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("conditions"), true) + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(cursor, property.FindPropertyRelative("clipLeft"));
                cursor.y += PropertyDrawerUITools.LineTop(1f);
                EditorGUI.PropertyField(cursor, property.FindPropertyRelative("clipRight"));
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return PropertyDrawerUITools.BoxHeight(
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("gesture1"), true) +
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("gesture2"), true) +
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("conditions"), true) +
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("clipLeft"), true) +
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("clipRight"), true) +
                EditorGUIUtility.standardVerticalSpacing * 4
            );
        }
    }
}