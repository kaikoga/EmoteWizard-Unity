using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Extensions;
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
    public class EmoteControlDrawer : TypedDrawerWithContext<EmoteControl, EmoteControlDrawerContext>
    {
        public override bool FixedPropertyHeight => false;

        public override void OnGUI(Rect position, ref EmoteControl property, GUIContent label)
        {
            var context = EnsureContext();

            var y = 0;
            if (context.IsEditing)
            {
                TypedGUI.FloatField(position.UISliceV(y++), new GUIContent("Transition Duration"), ref property.transitionDuration);
                TypedGUI.Toggle(position.UISliceV(y++), new GUIContent("Normalized Time"), ref property.normalizedTimeEnabled);
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(!property.normalizedTimeEnabled))
                {
                    if (context.AsGesture)
                    {
                        using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.normalizedTimeLeft)))
                        {
                            TypedGUI.TextField(position.UISliceV(y++), new GUIContent("Parameter Left"), ref property.normalizedTimeLeft);
                        }

                        using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.normalizedTimeRight)))
                        {
                            TypedGUI.TextField(position.UISliceV(y++), new GUIContent("Parameter Right"), ref property.normalizedTimeRight);
                        }
                    }
                    else
                    {
                        using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.normalizedTimeLeft)))
                        {
                            TypedGUI.TextField(position.UISliceV(y++), new GUIContent("Parameter"), ref property.normalizedTimeLeft);
                        }
                    }
                }

                TypedGUI.TypedField(position.UISliceV(y), ref property.trackingOverrides, "Tracking Overrides");
            }
            else
            {
                var parameterLabel = "";
                if (property.normalizedTimeEnabled)
                {
                    parameterLabel += $"Normalized Time:{property.normalizedTimeLeft}/{property.normalizedTimeRight})";
                    GUI.Label(position.UISliceV(y++), parameterLabel);
                }
                
                if (property.trackingOverrides.Count > 0)
                {
                    var overridesString = string.Join(", ", property.trackingOverrides.Where(t => t.target != TrackingTarget.None).Select(o => o.target));
                    GUI.Label(position.UISliceV(y++), $"Tracking Overrides: {overridesString}");
                }
            }
        }
        
        public override float GetPropertyHeight(EmoteControl property, GUIContent label)
        {
            var context = EnsureContext();

            if (context.IsEditing)
            {
                return LineHeight(context.AsGesture ? 4f : 3f) + TypedGUI.GetPropertyHeight(property.trackingOverrides, "Tracking") + EditorGUIUtility.standardVerticalSpacing;;
            }

            return LineHeight((property.normalizedTimeEnabled ? 1f : 0f) + (property.trackingOverrides.Count > 0 ? 1f : 0f));
        }
    }
}