using JetBrains.Annotations;
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
    [UsedImplicitly]
    public class ParameterEmoteStateDrawer : TypedDrawerWithContext<ParameterEmoteState, ParameterEmoteStateDrawerContext>
    {
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

                position = position.UISliceV(1);
                if (context.EditTargets)
                {
                    var height = TargetListHeight(property);
                    if (property.targets.Count < MinimumLargeTargetList)
                    {
                        CustomTypedGUI.HorizontalListField(position.SliceV(0f, height), new GUIContent("Targets"), ref property.targets);
                    }
                    else
                    {
                        TypedGUI.ListField(position.SliceV(0f, height), new GUIContent("Targets"), ref property.targets);
                    }
                    position = position.InsetTop(height + EditorGUIUtility.standardVerticalSpacing);
                }
                using (var sub = context.EmoteControlDrawerContext().StartContext())
                {
                    TypedGUI.TypedField(position, ref property.control, new GUIContent("Parameter"));
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        public override float GetPropertyHeight(ParameterEmoteState property, GUIContent label)
        {
            var context = EnsureContext();

            using (context.EmoteControlDrawerContext().StartContext())
            {
                var height = LineHeight(1f);
                if (context.EditTargets)
                {
                    height += TargetListHeight(property) + EditorGUIUtility.standardVerticalSpacing;
                }
                height += TypedGUI.GetPropertyHeight(property.control, "Parameter") + EditorGUIUtility.standardVerticalSpacing;
                return height;
            }
        }

        const int MinimumLargeTargetList = 4;
        static float TargetListHeight(ParameterEmoteState property)
        {
            return property.targets.Count < MinimumLargeTargetList ? LineHeight(1f) : TypedGUI.GetPropertyHeight(property.targets, "Targets");
        }
    }
}