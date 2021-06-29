using EmoteWizard.Base;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterState))]
    public class ParameterStateDrawer : PropertyDrawerWithContext<ParameterStateDrawer.Context>
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

        public static Context StartContext(EmoteWizardRoot emoteWizardRoot, string name) => PropertyDrawerWithContext<Context>.StartContext(new Context(emoteWizardRoot, name));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                EditorGUI.PropertyField(position.SliceH(0.0f, 0.2f), property.FindPropertyRelative("value"));
                if (!DrawFxClip)
                {
                    PropertyFieldWithGenerate(position.SliceH(0.2f, 0.75f),
                        property.FindPropertyRelative("gestureClip"),
                        () =>
                        {
                            var value = property.FindPropertyRelative("value").floatValue;
                            var relativePath = $"FX/@@@Generated@@@FX_{context.Name}_{value}.anim";
                            return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
                else if (!DrawGestureClip)
                {
                    PropertyFieldWithGenerate(position.SliceH(0.25f, 0.75f), property.FindPropertyRelative("fxClip"),
                        () =>
                        {
                            var value = property.FindPropertyRelative("value").floatValue;
                            var relativePath = $"FX/@@@Generated@@@FX_{context.Name}_{value}.anim";
                            return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
                else
                {
                    EditorGUI.PropertyField(position.SliceH(0.2f, 0.4f), property.FindPropertyRelative("gestureClip"), new GUIContent(" "));
                    EditorGUI.PropertyField(position.SliceH(0.6f, 0.4f), property.FindPropertyRelative("fxClip"), new GUIContent(" "));
                }
            }
        }
        
        public class Context : ContextBase
        {
            public readonly string Name;

            public Context() : base(null) { }
            public Context(EmoteWizardRoot emoteWizardRoot, string name) : base(emoteWizardRoot) => Name = name;
        }
    }
}