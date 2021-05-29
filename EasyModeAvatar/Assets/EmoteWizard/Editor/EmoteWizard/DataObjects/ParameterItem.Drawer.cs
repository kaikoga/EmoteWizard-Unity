using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterItem))]
    public class ParameterItemDrawer : PropertyDrawer
    {
        public static void DrawHeader()
        {
            var position = GUILayoutUtility.GetRect(0, BoxHeight(LineHeight(1)));
            GUI.Box(position, GUIContent.none);
            position = InsideBox(position);
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(new Rect(position.x + position.width * 0.00f, position.y + LineTop(0), position.width * 0.40f, LineHeight(1)), "Name");
                GUI.Label(new Rect(position.x + position.width * 0.40f, position.y + LineTop(0), position.width * 0.25f, LineHeight(1)), "Type");
                GUI.Label(new Rect(position.x + position.width * 0.65f, position.y + LineTop(0), position.width * 0.20f, LineHeight(1)), "Default");
                GUI.Label(new Rect(position.x + position.width * 0.85f, position.y + LineTop(0), position.width * 0.15f, LineHeight(1)), "Saved");
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defaultParameter = property.FindPropertyRelative("defaultParameter").boolValue; 
            // GUI.backgroundColor = defaultParameter ? Color.gray : Color.white;  
            GUI.Box(position, GUIContent.none);
            // GUI.backgroundColor = Color.white;

            position = InsideBox(position);
            using (new EditorGUI.PropertyScope(position, label, property))
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1f;
                using (new EditorGUI.DisabledScope(defaultParameter))
                {
                    EditorGUI.PropertyField(new Rect(position.x + position.width * 0.00f, position.y + LineTop(0), position.width * 0.40f, LineHeight(1)), property.FindPropertyRelative("name"), new GUIContent(" "));
                    EditorGUI.PropertyField(new Rect(position.x + position.width * 0.40f, position.y + LineTop(0), position.width * 0.25f, LineHeight(1)), property.FindPropertyRelative("valueType"), new GUIContent(" "));
                    EditorGUI.PropertyField(new Rect(position.x + position.width * 0.65f, position.y + LineTop(0), position.width * 0.20f, LineHeight(1)), property.FindPropertyRelative("defaultValue"), new GUIContent(" "));
                    EditorGUI.PropertyField(new Rect(position.x + position.width * 0.85f, position.y + LineTop(0), position.width * 0.15f, LineHeight(1)), property.FindPropertyRelative("saved"));
                }
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return BoxHeight(LineHeight(1f));
        }
    }
}