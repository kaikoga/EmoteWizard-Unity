using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteControl))]
    public class EmoteControlDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var serializedTransitionDuration = serializedProperty.FindPropertyRelative(nameof(EmoteControl.transitionDuration));
            var serializedNormalizedTimeEnabled = serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeEnabled));
            var serializedNormalizedTimeLeft = serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeLeft));
            var serializedNormalizedTimeRight = serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeRight));
            var serializedTrackingOverrides = serializedProperty.FindPropertyRelative(nameof(EmoteControl.trackingOverrides));

            var parametersWizard = ((EmoteWizardDataSourceBase)serializedProperty.serializedObject.targetObject).EmoteWizardRoot.EnsureWizard<ParametersWizard>();

            const bool IsEditing = true;
            const bool AsGesture = true;

            var y = 0;
            if (IsEditing)
            {

                EditorGUI.PropertyField(position.UISliceV(y++), serializedTransitionDuration);
                EditorGUI.PropertyField(position.UISliceV(y++), serializedNormalizedTimeEnabled);

                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(!serializedNormalizedTimeEnabled.boolValue))
                {
                    if (AsGesture)
                    {
                        using (new InvalidValueScope(parametersWizard.IsInvalidParameter(serializedNormalizedTimeLeft.stringValue)))
                        {
                            EditorGUI.PropertyField(position.UISliceV(y++), serializedNormalizedTimeLeft);
                        }

                        using (new InvalidValueScope(parametersWizard.IsInvalidParameter(serializedNormalizedTimeRight.stringValue)))
                        {
                            EditorGUI.PropertyField(position.UISliceV(y++), serializedNormalizedTimeRight);
                        }
                    }
                    else
                    {
                        using (new InvalidValueScope(parametersWizard.IsInvalidParameter(serializedNormalizedTimeLeft.stringValue)))
                        {
                            EditorGUI.PropertyField(position.UISliceV(y++), serializedNormalizedTimeLeft);
                        }
                    }
                }

                EditorGUI.PropertyField(position.UISliceV(y++, 2), serializedTrackingOverrides);
            }
            else
            {
                var parameterLabel = "";
                if (serializedNormalizedTimeEnabled.boolValue)
                {
                    parameterLabel += $"Normalized Time:{serializedNormalizedTimeLeft.floatValue}/{serializedNormalizedTimeRight.floatValue})";
                    GUI.Label(position.UISliceV(y++), parameterLabel);
                }
                
                if (serializedTrackingOverrides.arraySize > 0)
                {
                    // var overridesString = string.Join(", ", property.trackingOverrides.Where(t => t.target != TrackingTarget.None).Select(o => o.target));
                    var overridesString = $"{serializedTrackingOverrides.arraySize} overrides";
                    GUI.Label(position.UISliceV(y++), $"Tracking Overrides: {overridesString}");
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty serializedProperty, GUIContent label)
        {
            var serializedNormalizedTimeEnabled = serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeEnabled));
            var serializedTrackingOverrides = serializedProperty.FindPropertyRelative(nameof(EmoteControl.trackingOverrides));

            const bool IsEditing = true;
            const bool AsGesture = true;
            if (IsEditing)
            {
                return LineHeight(AsGesture ? 4f : 3f) + EditorGUI.GetPropertyHeight(serializedTrackingOverrides) + EditorGUIUtility.standardVerticalSpacing;;
            }

            return LineHeight((serializedNormalizedTimeEnabled.boolValue ? 1f : 0f) + (serializedTrackingOverrides.arraySize > 0 ? 1f : 0f));
        }
    }
}