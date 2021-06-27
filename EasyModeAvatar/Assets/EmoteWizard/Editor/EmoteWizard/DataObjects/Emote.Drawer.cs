using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.EditorUITools;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(Emote))]
    public class EmoteDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            var cursor = position.SliceV(0, 1);
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1f;
                EditorGUI.PropertyField(cursor, property.FindPropertyRelative("gesture1"), new GUIContent(" "));
                cursor.y += LineTop(1f);
                EditorGUI.PropertyField(cursor, property.FindPropertyRelative("gesture2"), new GUIContent(" "));
                cursor.y += LineTop(1f);
                EditorGUIUtility.labelWidth = labelWidth;
                var conditions = property.FindPropertyRelative("conditions");
                EditorGUI.PropertyField(cursor, conditions, true);
                cursor.y += EditorGUI.GetPropertyHeight(conditions, true) + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(cursor, property.FindPropertyRelative("clipLeft"));
                cursor.y += LineTop(1f);
                EditorGUI.PropertyField(cursor, property.FindPropertyRelative("clipRight"));
                cursor.y += LineTop(1f);
                var parameter = property.FindPropertyRelative("parameter");
                EditorGUI.PropertyField(cursor, parameter, true);
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return BoxHeight(
                LineHeight(4f) +
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("conditions"), true) +
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("parameter"), true) +
                EditorGUIUtility.standardVerticalSpacing
            );
        }
    }
}