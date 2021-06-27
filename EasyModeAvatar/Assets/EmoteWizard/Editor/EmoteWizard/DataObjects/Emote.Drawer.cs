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
        public static bool EditConditions = true;
        public static bool EditAnimations = true;
        public static bool EditParameters = true;

        public static void DrawHeader(bool useReorderUI)
        {
            var position = GUILayoutUtility.GetRect(0, BoxHeight(LineHeight(3f)));
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            GUI.Box(position, GUIContent.none);
            GUI.backgroundColor = backgroundColor;
            
            position = position.InsideBox();
            // position.xMin += useReorderUI ? 20f : 6f;
            // position.xMax -= 6f;
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                EditConditions = EditorGUI.ToggleLeft(position.SliceV(0), "Show Conditions", EditConditions);
                EditAnimations = EditorGUI.ToggleLeft(position.SliceV(1), "Show Animations", EditAnimations);
                EditParameters = EditorGUI.ToggleLeft(position.SliceV(2), "Show Parameters", EditParameters);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            var cursor = position.SliceV(0, 1);
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                if (EditConditions)
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
                }

                if (EditAnimations)
                {
                    EditorGUI.PropertyField(cursor, property.FindPropertyRelative("clipLeft"));
                    cursor.y += LineTop(1f);
                    EditorGUI.PropertyField(cursor, property.FindPropertyRelative("clipRight"));
                    cursor.y += LineTop(1f);
                }

                if (EditParameters)
                {
                    var parameter = property.FindPropertyRelative("parameter");
                    EditorGUI.PropertyField(cursor, parameter, true);
                }
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var h = 0f;
            if (EditConditions)
            {
                h += LineHeight(2f) + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("conditions"), true) + EditorGUIUtility.standardVerticalSpacing;
            }
            if (EditAnimations)
            {
                h += LineHeight(2f) + EditorGUIUtility.standardVerticalSpacing;
            }
            if (EditParameters)
            {
                h += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("parameter"), true) + EditorGUIUtility.standardVerticalSpacing;
            }
            return BoxHeight(h);
        }
    }
}