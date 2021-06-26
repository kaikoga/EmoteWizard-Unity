using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ExpressionItem))]
    public class ExpressionItemDrawer : PropertyDrawer
    {
        public static void DrawHeader(bool useReorderUI)
        {
            var position = GUILayoutUtility.GetRect(0, BoxHeight(LineHeight(2f)));
            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            position.xMin += useReorderUI ? 20f : 6f;
            position.xMax -= 6f;
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.Slice(0.0f, 0.4f, 0), "Icon");
                GUI.Label(position.Slice(0.4f, 0.6f, 0), "Path");
                
                GUI.Label(position.Slice(0.0f, 0.4f, 1), "Parameter");
                GUI.Label(position.Slice(0.4f, 0.2f, 1), "Value");
                GUI.Label(position.Slice(0.6f, 0.4f, 1), "ControlType");
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new EditorGUI.PropertyScope(position, label, property))
            using (new EditorGUI.IndentLevelScope())
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1f;
                EditorGUI.PropertyField(position.Slice(0.0f, 0.4f, 0), property.FindPropertyRelative("icon"), new GUIContent(" "));
                EditorGUI.PropertyField(position.Slice(0.4f, 0.6f, 0), property.FindPropertyRelative("path"), new GUIContent(" "));

                EditorGUI.PropertyField(position.Slice(0.0f, 0.4f, 1), property.FindPropertyRelative("parameter"), new GUIContent(" "));
                EditorGUI.PropertyField(position.Slice(0.4f, 0.2f, 1), property.FindPropertyRelative("value"), new GUIContent(" "));
                EditorGUI.PropertyField(position.Slice(0.6f, 0.4f, 1), property.FindPropertyRelative("controlType"), new GUIContent(" "));
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return BoxHeight(LineHeight(2f));
        }
    }
}