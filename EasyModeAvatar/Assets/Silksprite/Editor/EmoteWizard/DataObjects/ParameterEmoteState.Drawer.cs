using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterEmoteState))]
    public class ParameterEmoteStateDrawer : HybridDrawerWithContext<ParameterEmoteState, ParameterEmoteStateDrawerContext>
    {
        public static ParameterEmoteStateDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, string layer, string name, bool editTargets) => StartContext(new ParameterEmoteStateDrawerContext(emoteWizardRoot, layer, name, editTargets));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            using (new EditorGUI.IndentLevelScope())
            {
                using (new HideLabelsScope())
                {
                    var enabled = property.FindPropertyRelative("enabled");
                    EditorGUI.PropertyField(position.UISlice(0.0f, 0.1f, 0), enabled);
                    EditorGUI.BeginDisabledGroup(!enabled.boolValue);
                    var value = property.FindPropertyRelative("value");
                    EditorGUI.PropertyField(position.UISlice(0.1f, 0.2f, 0), value);
                    CustomEditorGUI.PropertyFieldWithGenerate(position.UISlice(0.3f, 0.7f, 0),
                        property.FindPropertyRelative("clip"),
                        () =>
                        {
                            if (string.IsNullOrEmpty(context.Name))
                            {
                                Debug.LogError("Emote Name is required.");
                                return null;
                            }

                            var relativePath = GeneratedAssetLocator.ParameterEmoteStateClipPath(context.Layer, context.Name, value.floatValue);
                            return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
                if (context.EditTargets)
                {
                    CustomEditorGUI.HorizontalListPropertyField(position.UISliceV(1), property.FindPropertyRelative("targets"));
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);
            return LineHeight(context.EditTargets ? 2f : 1f);
        }

        public override bool FixedPropertyHeight => false;
        
        public override void OnGUI(Rect position, ref ParameterEmoteState property, GUIContent label)
        {
            var context = EnsureContext();

            using (new EditorGUI.IndentLevelScope())
            {
                using (new HideLabelsScope())
                {
                    TypedGUI.Toggle(position.UISlice(0.0f, 0.1f, 0), "Enabled", ref property.enabled);
                    EditorGUI.BeginDisabledGroup(!property.enabled);
                    TypedGUI.FloatField(position.UISlice(0.1f, 0.2f, 0), "Value", ref property.value);
                    var value = property.value;
                    CustomTypedGUI.AssetFieldWithGenerate(position.UISlice(0.3f, 0.7f, 0),
                        "Clip",
                        ref property.clip,
                        () =>
                        {
                            if (string.IsNullOrEmpty(context.Name))
                            {
                                Debug.LogError("Emote Name is required.");
                                return null;
                            }

                            var relativePath = GeneratedAssetLocator.ParameterEmoteStateClipPath(context.Layer, context.Name, value);
                            return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
                if (context.EditTargets)
                {
                    CustomTypedGUI.HorizontalListUntypedField(position.UISliceV(1), new GUIContent("Targets"), property.targets);
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        public override float GetPropertyHeight(ParameterEmoteState property, GUIContent label)
        {
            var context = EnsureContext();
            return LineHeight(context.EditTargets ? 2f : 1f);
        }
    }
}