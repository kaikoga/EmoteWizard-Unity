using JetBrains.Annotations;
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
    [UsedImplicitly]
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
                    TypedGUI.TextField(position.UISliceH(0.0f, 0.4f), new GUIContent(" "), ref property.parameter);
                }
                TypedGUI.EnumPopup(position.UISliceH(0.4f, 0.2f), new GUIContent(" "), ref property.kind);
                TypedGUI.EnumPopup(position.UISliceH(0.6f, 0.2f), new GUIContent(" "), ref property.mode);
                TypedGUI.FloatField(position.UISliceH(0.8f, 0.2f), new GUIContent(" "), ref property.threshold);
            }
        }
    }
}