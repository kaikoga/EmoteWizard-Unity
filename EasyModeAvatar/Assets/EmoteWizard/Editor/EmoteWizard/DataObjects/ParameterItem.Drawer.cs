using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterItem))]
    public class ParameterItemDrawer : PropertyDrawer
    {
        static EmoteWizardRoot _emoteWizardRoot;

        public static void StartContext(EmoteWizardRoot emoteWizardRoot)
        {
            _emoteWizardRoot = emoteWizardRoot;
        }

        public static void EndContext()
        {
            _emoteWizardRoot = null;
        }

        public static void DrawHeader(bool useReorderUI)
        {
            var position = GUILayoutUtility.GetRect(0, BoxHeight(LineHeight(2f)));
            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            position.xMin += useReorderUI ? 20f : 6f;
            position.xMax -= 6f;
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.Slice(0.00f, 0.40f, 0), "Name");
                GUI.Label(position.Slice(0.40f, 0.25f, 0), "Type");
                GUI.Label(position.Slice(0.65f, 0.20f, 0), "Default");
                GUI.Label(position.Slice(0.85f, 0.15f, 0), "Saved");

                GUI.Label(position.Slice(0.00f, 0.20f, 1), "Value");
                ParameterStateDrawer.DrawGestureClip = GUI.Toggle(position.Slice(0.20f, 0.40f, 1), ParameterStateDrawer.DrawGestureClip, "Gesture Clip");
                ParameterStateDrawer.DrawFxClip = GUI.Toggle(position.Slice(0.60f, 0.40f, 1), ParameterStateDrawer.DrawFxClip, "FX Clip");
            }
        }

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
                {
                    var labelWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 1f;
                    using (new EditorGUI.DisabledScope(defaultParameter))
                    {
                        EditorGUI.PropertyField(position.Slice(0.00f, 0.40f, 0), property.FindPropertyRelative("name"), new GUIContent(" "));
                        EditorGUI.PropertyField(position.Slice(0.40f, 0.25f, 0), property.FindPropertyRelative("valueType"), new GUIContent(" "));
                        EditorGUI.PropertyField(position.Slice(0.65f, 0.20f, 0), property.FindPropertyRelative("defaultValue"), new GUIContent(" "));
                        EditorGUI.PropertyField(position.Slice(0.85f, 0.15f, 0), property.FindPropertyRelative("saved"));
                    }
                    EditorGUIUtility.labelWidth = labelWidth;
                }

                using (new EditorGUI.DisabledScope(property.FindPropertyRelative("valueType").intValue == (int)VRCExpressionParameters.ValueType.Float))
                {
                    ParameterStateDrawer.StartContext(_emoteWizardRoot, property.FindPropertyRelative("name").stringValue);
                    EditorGUI.PropertyField(position.SliceV(1, -1), property.FindPropertyRelative("states"), true);
                    ParameterStateDrawer.EndContext();
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // return BoxHeight(LineHeight(1f) + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("states"), true));
            var states = property.FindPropertyRelative("states");
            var statesLines = states.isExpanded ? states.arraySize + 2f : 1f;
            return BoxHeight(LineHeight(1f + statesLines));
        }
    }
}