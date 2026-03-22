using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterNameAttribute))]
    public class ParameterNameAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var parameterNameAttribute = (ParameterNameAttribute)attribute;
            PropertyField(position, serializedProperty, label, parameterNameAttribute.AllowEmpty, parameterNameAttribute.AllowNew);
        }

        static void PropertyField(Rect position, SerializedProperty serializedProperty, GUIContent label, bool allowEmpty, bool allowNew)
        {
            var isInvalidValue = ParameterNameAttribute.IsInvalidParameterInput(serializedProperty.stringValue, allowEmpty);
            
            if (!allowNew)
            {
                var env = ((EmoteWizardBehaviour)serializedProperty.serializedObject.targetObject).CreateEnv();
                isInvalidValue |= env.GetContext<ParametersContext>().Snapshot().IsInvalidParameterReference(serializedProperty.stringValue);
            }

            using (new InvalidValueScope(isInvalidValue))
            {
                EditorGUI.PropertyField(position, serializedProperty, label);
            }
        }
    }
}