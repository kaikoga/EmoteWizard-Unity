using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteCondition))]
    public class EmoteConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var serializedParameter = serializedProperty.FindPropertyRelative(nameof(EmoteCondition.parameter));
            var serializedKind = serializedProperty.FindPropertyRelative(nameof(EmoteCondition.kind));
            var serializedMode = serializedProperty.FindPropertyRelative(nameof(EmoteCondition.mode));
            var serializedThreshold = serializedProperty.FindPropertyRelative(nameof(EmoteCondition.threshold));

            var parametersWizard = ((EmoteWizardDataSourceBase)serializedProperty.serializedObject.targetObject).EmoteWizardRoot.EnsureWizard<ParametersWizard>();

            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                using (new InvalidValueScope(parametersWizard.IsInvalidParameter(serializedParameter.stringValue)))
                {
                    EditorGUI.PropertyField(position.UISliceH(0.0f, 0.4f), serializedParameter);
                }

                EditorGUI.PropertyField(position.UISliceH(0.4f, 0.2f), serializedKind);
                EditorGUI.PropertyField(position.UISliceH(0.6f, 0.2f), serializedMode);
                EditorGUI.PropertyField(position.UISliceH(0.8f, 0.2f), serializedThreshold);
            }
        }
    }
}