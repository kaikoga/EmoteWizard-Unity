using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using static Silksprite.EmoteWizard.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterItem))]
    public class ParameterItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defaultParameter = property.FindPropertyRelative("defaultParameter").boolValue; 
            // GUI.backgroundColor = defaultParameter ? Color.gray : Color.white;  
            GUI.Box(position, GUIContent.none);
            // GUI.backgroundColor = Color.white;

            position = position.InsideBox();
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
                using (new HideLabelsScope())
                {
                    using (new EditorGUI.DisabledScope(defaultParameter))
                    {
                        EditorGUI.PropertyField(position.UISlice(0.00f, 0.40f, 0), property.FindPropertyRelative("name"), new GUIContent(" "));
                        EditorGUI.PropertyField(position.UISlice(0.40f, 0.25f, 0), property.FindPropertyRelative("valueKind"), new GUIContent(" "));
                        EditorGUI.PropertyField(position.UISlice(0.65f, 0.20f, 0), property.FindPropertyRelative("defaultValue"), new GUIContent(" "));
                        EditorGUI.PropertyField(position.UISlice(0.85f, 0.15f, 0), property.FindPropertyRelative("saved"));
                    }
                }

                EditorGUI.PropertyField(position.UISliceV(1, -1), property.FindPropertyRelative("usages"), true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // return BoxHeight(LineHeight(1f) + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("usages"), true));
            var usages = property.FindPropertyRelative("usages");
            var usagesLines = usages.isExpanded ? usages.arraySize + 2f : 1f;
            return BoxHeight(LineHeight(1f + usagesLines));
        }
    }
}