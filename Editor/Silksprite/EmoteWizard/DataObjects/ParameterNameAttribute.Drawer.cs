using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Scopes;
using Silksprite.EmoteWizard.Utils;
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
            var isInvalidValue = ParameterNameUtil.IsInvalidParameterFormat(serializedProperty.stringValue, allowEmpty);
            
            if (!allowNew)
            {
                var env = InnerGUIEnvironmentScope.GetCurrentEnv();
                isInvalidValue |= env.GetContext<ParametersContext>().Snapshot().IsInvalidParameterReference(serializedProperty.stringValue);
            }

            using (new InvalidValueScope(isInvalidValue))
            {
                EditorGUI.PropertyField(position, serializedProperty, label);
            }
        }
    }
}