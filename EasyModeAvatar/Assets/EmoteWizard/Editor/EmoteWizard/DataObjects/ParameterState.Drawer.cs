using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterState))]
    public class ParameterStateDrawer : PropertyDrawer
    {
        static bool drawGestureClip = false;
        static bool drawFxClip = true;

        public static bool DrawGestureClip
        {
            get => drawGestureClip;
            set
            {
                drawGestureClip = value;
                if (!value) drawFxClip = true;
            }
        }

        public static bool DrawFxClip
        {
            get => drawFxClip;
            set
            {
                drawFxClip = value;
                if (!value) drawGestureClip = true;
            }
        }

        public static string Context = "";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EmoteWizardRoot FindEmoteWizardRoot() => Object.FindObjectOfType<EmoteWizardRoot>();

            using (new EditorGUI.IndentLevelScope())
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1f;
                EditorGUI.PropertyField(position.SliceH(0.0f, 0.2f), property.FindPropertyRelative("value"));
                if (!DrawFxClip)
                {
                    PropertyFieldWithGenerate(position.SliceH(0.2f, 0.75f),
                        property.FindPropertyRelative("gestureClip"),
                        () =>
                        {
                            var value = property.FindPropertyRelative("value").floatValue;
                            return FindEmoteWizardRoot().ProvideAnimationClip($"FX/@@@Generated@@@FX_{Context}_{value}.anim");
                        });
                }
                else if (!DrawGestureClip)
                {
                    PropertyFieldWithGenerate(position.SliceH(0.25f, 0.75f), property.FindPropertyRelative("fxClip"),
                        () =>
                        {
                            var value = property.FindPropertyRelative("value").floatValue;
                            return FindEmoteWizardRoot().ProvideAnimationClip($"FX/@@@Generated@@@FX_{Context}_{value}.anim");
                        });
                }
                else
                {
                    EditorGUI.PropertyField(position.SliceH(0.2f, 0.4f), property.FindPropertyRelative("gestureClip"), new GUIContent(" "));
                    EditorGUI.PropertyField(position.SliceH(0.6f, 0.4f), property.FindPropertyRelative("fxClip"), new GUIContent(" "));
                }
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
    }
}