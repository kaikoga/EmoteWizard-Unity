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
            var parametersWizard = ((EmoteWizardDataSourceBase)serializedProperty.serializedObject.targetObject).EmoteWizardRoot.EnsureWizard<ParametersWizard>();

            const bool IsEditing = true;
            const bool AsGesture = true;

            var y = 0;
            if (IsEditing)
            {
                EditorGUI.PropertyField(position.UISliceV(y++), serializedProperty.FindPropertyRelative(nameof(EmoteControl.transitionDuration)));
                EditorGUI.PropertyField(position.UISliceV(y++), serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeEnabled)));
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(!serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeEnabled)).boolValue))
                {
                    if (AsGesture)
                    {
                        using (new InvalidValueScope(parametersWizard.IsInvalidParameter(serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeLeft)).stringValue)))
                        {
                            EditorGUI.PropertyField(position.UISliceV(y++), serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeLeft)));
                        }

                        using (new InvalidValueScope(parametersWizard.IsInvalidParameter(serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeRight)).stringValue)))
                        {
                            EditorGUI.PropertyField(position.UISliceV(y++), serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeRight)));
                        }
                    }
                    else
                    {
                        using (new InvalidValueScope(parametersWizard.IsInvalidParameter(serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeLeft)).stringValue)))
                        {
                            EditorGUI.PropertyField(position.UISliceV(y++), serializedProperty.FindPropertyRelative(nameof(EmoteControl.normalizedTimeLeft)));
                        }
                    }
                }

                EditorGUI.PropertyField(position.UISliceV(y++, 2), serializedProperty.FindPropertyRelative(nameof(EmoteControl.trackingOverrides)));
            }
            else
            {
                /*
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
                */
            }
        }

        public override float GetPropertyHeight(SerializedProperty serializedProperty, GUIContent label)
        {
            const bool IsEditing = true;
            const bool AsGesture = true;
            if (IsEditing)
            {
                return LineHeight(AsGesture ? 4f : 3f) + EditorGUI.GetPropertyHeight(serializedProperty.FindPropertyRelative(nameof(EmoteControl.trackingOverrides))) + EditorGUIUtility.standardVerticalSpacing;;
            }

            // return LineHeight((property.normalizedTimeEnabled ? 1f : 0f) + (property.trackingOverrides.Count > 0 ? 1f : 0f));
        }
    }
}