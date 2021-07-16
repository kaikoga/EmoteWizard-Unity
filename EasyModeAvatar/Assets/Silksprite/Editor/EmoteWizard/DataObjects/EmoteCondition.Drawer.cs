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
    [CustomPropertyDrawer(typeof(EmoteCondition))]
    public class EmoteConditionDrawer : HybridDrawerWithContext<EmoteCondition, EmoteConditionDrawerContext>
    {
        public static EmoteConditionDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard) => StartContext(new EmoteConditionDrawerContext(emoteWizardRoot, parametersWizard));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            using (new EditorGUI.PropertyScope(position, label, property))
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                var parameter = property.FindPropertyRelative("parameter");
                using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(parameter.stringValue)))
                {
                    EditorGUI.PropertyField(position.UISliceH(0.00f, 0.50f), parameter, new GUIContent(" "));
                }
                EditorGUI.PropertyField(position.UISliceH(0.50f, 0.25f), property.FindPropertyRelative("mode"), new GUIContent(" "));
                EditorGUI.PropertyField(position.UISliceH(0.75f, 0.25f), property.FindPropertyRelative("threshold"), new GUIContent(" "));
            }
        }
        
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