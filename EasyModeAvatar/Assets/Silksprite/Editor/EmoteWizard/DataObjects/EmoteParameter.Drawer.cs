using System;
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
    public class EmoteParameterDrawer : TypedDrawerWithContext<EmoteParameter, EmoteParameterDrawerContext>
    {
        public override bool FixedPropertyHeight => false;

        public override void OnGUI(Rect position, ref EmoteParameter property, GUIContent label)
        {
            var context = EnsureContext();

            if (context.IsEditing)
            {
                TypedGUI.Toggle(position.UISliceV(0), new GUIContent("Normalized Time"), ref property.normalizedTimeEnabled);
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(!property.normalizedTimeEnabled))
                {
                    using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.normalizedTimeLeft)))
                    {
                        TypedGUI.TextField(position.UISliceV(1), new GUIContent("Parameter Left"), ref property.normalizedTimeLeft);
                    }

                    using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.normalizedTimeRight)))
                    {
                        TypedGUI.TextField(position.UISliceV(2), new GUIContent("Parameter Right"), ref property.normalizedTimeRight);
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
        
        public override float GetPropertyHeight(EmoteParameter property, GUIContent label)
        {
            var context = EnsureContext();

            if (context.IsEditing)
            {
                return LineHeight(3f);
            }
            if (property.normalizedTimeEnabled)
            {
                return LineHeight(1f);
            }
            return LineHeight(0f);
        }
    }
}