using EmoteWizard.Base;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using EmoteWizard.UI;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterEmoteState))]
    public class ParameterEmoteStateDrawer : PropertyDrawerWithContext<ParameterEmoteStateDrawer.Context>
    {
        public static Context StartContext(EmoteWizardRoot emoteWizardRoot, string layer, string name, bool editTargets) => PropertyDrawerWithContext<Context>.StartContext(new Context(emoteWizardRoot, layer, name, editTargets));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            using (new EditorGUI.IndentLevelScope())
            {
                using (new HideLabelsScope())
                {
                    var value = property.FindPropertyRelative("value");
                    EditorGUI.PropertyField(position.Slice(0.0f, 0.2f, 0), value);
                    EmoteWizardGUI.PropertyFieldWithGenerate(position.Slice(0.2f, 0.75f, 0),
                        property.FindPropertyRelative("clip"),
                        () =>
                        {
                            if (string.IsNullOrEmpty(context.Name))
                            {
                                Debug.LogError("Emote Name is required.");
                                return null;
                            }

                            var relativePath =
                                $"{context.Layer}/@@@Generated@@@{context.Layer}_{context.Name}_{value.floatValue}.anim";
                            return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
                if (context.EditTargets)
                {
                    EditorGUI.PropertyField(position.SliceV(1, -1), property.FindPropertyRelative("targets"), true);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);
            var result = LineHeight(1f);
            if (!context.EditTargets) return result;
            result += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targets"), true);
            result += EditorGUIUtility.standardVerticalSpacing;
            return result;
        }

        public class Context : ContextBase
        {
            public readonly string Layer;
            public readonly string Name;
            public readonly bool EditTargets;

            public Context() : base(null) { }
            public Context(EmoteWizardRoot emoteWizardRoot, string layer, string name, bool editTargets) : base(emoteWizardRoot)
            {
                Layer = layer;
                Name = name;
                EditTargets = editTargets;
            }
        }
    }
}