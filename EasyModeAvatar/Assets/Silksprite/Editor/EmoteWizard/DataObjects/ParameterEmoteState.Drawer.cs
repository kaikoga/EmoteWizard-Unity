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

                var y = 1;
                if (context.EditTargets)
                {
                    CustomTypedGUI.HorizontalListField(position.UISliceV(y), new GUIContent("Targets"), ref property.targets);
                    y++;
                }
                using (context.EmoteParameterDrawerContext().StartContext())
                {
                    TypedGUI.TypedField(position.UISliceV(y, 4), ref property.parameter, new GUIContent("Parameter"));
                    
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        public override float GetPropertyHeight(ParameterEmoteState property, GUIContent label)
        {
            var context = EnsureContext();
            var lines = 1f;
            if (context.EditTargets) lines += 1f;

            using (context.EmoteParameterDrawerContext().StartContext())
            {
                return LineHeight(lines) + TypedGUI.GetPropertyHeight(property.parameter, "Parameter") + EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}