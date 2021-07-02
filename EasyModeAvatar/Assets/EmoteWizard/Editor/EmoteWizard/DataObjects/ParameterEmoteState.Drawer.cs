using EmoteWizard.Base;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using EmoteWizard.UI;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterEmoteState))]
    public class ParameterEmoteStateDrawer : PropertyDrawerWithContext<ParameterEmoteStateDrawer.Context>
    {
        public static Context StartContext(EmoteWizardRoot emoteWizardRoot, string layer, string name) => PropertyDrawerWithContext<Context>.StartContext(new Context(emoteWizardRoot, layer, name));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                var value = property.FindPropertyRelative("value");
                EditorGUI.PropertyField(position.SliceH(0.0f, 0.2f), value);
                    EmoteWizardGUI.PropertyFieldWithGenerate(position.SliceH(0.2f, 0.75f),
                    property.FindPropertyRelative("clip"),
                    () =>
                    {
                        if (string.IsNullOrEmpty(context.Name))
                        {
                            Debug.LogError("Emote Name is required.");
                            return null;
                        }
                        var relativePath = $"{context.Layer}/@@@Generated@@@{context.Layer}_{context.Name}_{value.floatValue}.anim";
                        return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                    });
            }
        }
        
        public class Context : ContextBase
        {
            public readonly string Layer;
            public readonly string Name;

            public Context() : base(null) { }
            public Context(EmoteWizardRoot emoteWizardRoot, string layer, string name) : base(emoteWizardRoot)
            {
                Layer = layer;
                Name = name;
            }
        }
    }
}