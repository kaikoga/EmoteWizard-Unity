using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteParameter))]
    public class EmoteParameterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var normalizedTimeEnabled = property.FindPropertyRelative("normalizedTimeEnabled");
                EditorGUI.PropertyField(position.SliceV(0), normalizedTimeEnabled, new GUIContent("Normalized Time"));
                var labelWidth = EditorGUIUtility.labelWidth;
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(!normalizedTimeEnabled.boolValue))
                {
                    EditorGUI.PropertyField(position.SliceV( 1), property.FindPropertyRelative("normalizedTimeLeft"), new GUIContent("Parameter Left"));
                    EditorGUI.PropertyField(position.SliceV(2), property.FindPropertyRelative("normalizedTimeRight"), new GUIContent("Parameter Right"));
                }
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return LineHeight(3f);
        }
    }
}