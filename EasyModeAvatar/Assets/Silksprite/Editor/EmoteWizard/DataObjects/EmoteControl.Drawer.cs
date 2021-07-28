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

            if (context.IsEditing)
            {
                TypedGUI.FloatField(position.UISliceV(0), new GUIContent("Duration"), ref property.transitionDuration);
                TypedGUI.Toggle(position.UISliceV(1), new GUIContent("Normalized Time"), ref property.normalizedTimeEnabled);
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(!property.normalizedTimeEnabled))
                {
                    if (context.AsGesture)
                    {
                        using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.normalizedTimeLeft)))
                        {
                            TypedGUI.TextField(position.UISliceV(2), new GUIContent("Parameter Left"), ref property.normalizedTimeLeft);
                        }

                        using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.normalizedTimeRight)))
                        {
                            TypedGUI.TextField(position.UISliceV(3), new GUIContent("Parameter Right"), ref property.normalizedTimeRight);
                        }
                    }
                    else
                    {
                        using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.normalizedTimeLeft)))
                        {
                            TypedGUI.TextField(position.UISliceV(2), new GUIContent("Parameter"), ref property.normalizedTimeLeft);
                        }
                    }
                }
            }
            else
            {
                var parameterLabel = "";
                if (property.normalizedTimeEnabled)
                {
                    parameterLabel += $"Normalized Time:{property.normalizedTimeLeft}/{property.normalizedTimeRight})";
                }

                GUI.Label(position, parameterLabel);
            }
        }
        
        public override float GetPropertyHeight(EmoteControl property, GUIContent label)
        {
            var context = EnsureContext();

            if (context.IsEditing)
            {
                return LineHeight(context.AsGesture ? 4f : 3f);
            }
            if (property.normalizedTimeEnabled)
            {
                return LineHeight(1f);
            }
            return LineHeight(0f);
        }
    }
}