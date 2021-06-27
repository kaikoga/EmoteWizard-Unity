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
            property.isExpanded = EditorGUI.Foldout(position.SliceV(0), property.isExpanded, label);
            if (!property.isExpanded) return;

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var normalizedTimeEnabled = property.FindPropertyRelative("normalizedTimeEnabled");
                EditorGUI.PropertyField(position.SliceV(1), normalizedTimeEnabled, new GUIContent("Normalized Time"));
                var labelWidth = EditorGUIUtility.labelWidth;
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(!normalizedTimeEnabled.boolValue))
                {
                    EditorGUI.PropertyField(position.SliceV( 2), property.FindPropertyRelative("normalizedTimeLeft"), new GUIContent("Parameter Left"));
                    EditorGUI.PropertyField(position.SliceV(3), property.FindPropertyRelative("normalizedTimeRight"), new GUIContent("Parameter Right"));
                }
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded ? LineHeight(4f) : LineHeight(1f);
        }
    }
}