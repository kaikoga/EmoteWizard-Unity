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

        static Context _context;

        public static void StartContext(EmoteWizardRoot emoteWizardRoot, string name) => StartContext(new Context(emoteWizardRoot, name));

        static void StartContext(Context context) => _context = context;

        public static void EndContext() => _context = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_context == null)
            {
                Debug.LogWarning("Internal: context is null", property.serializedObject.targetObject);
                _context = new Context();
            }

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
                            var relativePath = $"FX/@@@Generated@@@FX_{_context.Name}_{value}.anim";
                            return _context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
                else if (!DrawGestureClip)
                {
                    PropertyFieldWithGenerate(position.SliceH(0.25f, 0.75f), property.FindPropertyRelative("fxClip"),
                        () =>
                        {
                            var value = property.FindPropertyRelative("value").floatValue;
                            var relativePath = $"FX/@@@Generated@@@FX_{_context.Name}_{value}.anim";
                            return _context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
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
        
        class Context
        {
            public readonly string Name;
            readonly EmoteWizardRoot _emoteWizardRoot;

            protected internal Context()
            {
            }

            protected internal Context(EmoteWizardRoot emoteWizardRoot, string name)
            {
                Name = name;
                _emoteWizardRoot = emoteWizardRoot;
            }

            public EmoteWizardRoot EmoteWizardRoot => _emoteWizardRoot ? _emoteWizardRoot : Object.FindObjectOfType<EmoteWizardRoot>();
        }
    }
}