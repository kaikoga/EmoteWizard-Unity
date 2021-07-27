using System;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizardSupport.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public class EmoteConditionDrawer : TypedDrawerWithContext<EmoteCondition, EmoteConditionDrawerContext>
    {
        public override void OnGUI(Rect position, ref EmoteCondition property, GUIContent label)
        {
            var context = EnsureContext();

            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(property.parameter)))
                {
                    TypedGUI.TextField(position.UISliceH(0.00f, 0.50f), new GUIContent(" "), ref property.parameter);
                }
                TypedGUI.EnumPopup(position.UISliceH(0.50f, 0.25f), new GUIContent(" "), ref property.mode);
                TypedGUI.FloatField(position.UISliceH(0.75f, 0.25f), new GUIContent(" "), ref property.threshold);
            }
        }
    }
}