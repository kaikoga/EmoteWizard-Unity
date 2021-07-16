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
    [CustomPropertyDrawer(typeof(EmoteParameter))]
    public class EmoteParameterDrawer : HybridDrawerWithContext<EmoteParameter, EmoteParameterDrawerContext>
    {
        public static EmoteParameterDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard, bool isEditing) => StartContext(new EmoteParameterDrawerContext(emoteWizardRoot, parametersWizard, isEditing));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                var normalizedTimeEnabled = property.FindPropertyRelative("normalizedTimeEnabled");
                var normalizedTimeLeft = property.FindPropertyRelative("normalizedTimeLeft");
                var normalizedTimeRight = property.FindPropertyRelative("normalizedTimeRight");
                if (context.IsEditing)
                {
                    EditorGUI.PropertyField(position.UISliceV(0), normalizedTimeEnabled, new GUIContent("Normalized Time"));
                    using (new EditorGUI.IndentLevelScope())
                    using (new EditorGUI.DisabledScope(!normalizedTimeEnabled.boolValue))
                    {
                        using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(normalizedTimeLeft.stringValue)))
                        {
                            EditorGUI.PropertyField(position.UISliceV(1), normalizedTimeLeft, new GUIContent("Parameter Left"));
                        }

                        using (new InvalidValueScope(context.ParametersWizard.IsInvalidParameter(normalizedTimeRight.stringValue)))
                        {
                            EditorGUI.PropertyField(position.UISliceV(2), normalizedTimeRight, new GUIContent("Parameter Right"));
                        }
                    }
                }
                else
                {
                    var parameterLabel = "";
                    if (normalizedTimeEnabled.boolValue)
                    {
                        parameterLabel += $"Normalized Time:{normalizedTimeLeft.stringValue}/{normalizedTimeRight.stringValue})";
                    }
                    GUI.Label(position, parameterLabel);
                }
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            if (context.IsEditing)
            {
                return LineHeight(3f);
            }
            if (property.FindPropertyRelative("normalizedTimeEnabled").boolValue)
            {
                return LineHeight(1f);
            }
            return LineHeight(0f);
        }
        
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