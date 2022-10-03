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
            var parametersWizard = ((EmoteWizardDataSourceBase)serializedProperty.serializedObject.targetObject).EmoteWizardRoot.EnsureWizard<ParametersWizard>();

            using (new EditorGUILayout.HorizontalScope())
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                using (new InvalidValueScope(parametersWizard.IsInvalidParameter(serializedProperty.FindPropertyRelative(nameof(EmoteCondition.parameter)).stringValue)))
                {
                    EditorGUI.PropertyField(position.UISliceH(0.0f, 0.4f), serializedProperty.FindPropertyRelative(nameof(EmoteCondition.parameter)));
                }
                EditorGUI.PropertyField(position.UISliceH(0.4f, 0.2f), serializedProperty.FindPropertyRelative(nameof(EmoteCondition.kind)));
                EditorGUI.PropertyField(position.UISliceH(0.6f, 0.2f), serializedProperty.FindPropertyRelative(nameof(EmoteCondition.mode)));
                EditorGUI.PropertyField(position.UISliceH(0.8f, 0.2f), serializedProperty.FindPropertyRelative(nameof(EmoteCondition.threshold)));
            }
        }
    }
}