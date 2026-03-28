using System.Linq;
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
            var left = position;
            left.width -= EditorGUIUtility.singleLineHeight;
            var right = position;
            right.xMin = right.xMax - EditorGUIUtility.singleLineHeight;

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.BeginProperty(position, label, serializedProperty);

            var isInvalidValue = ParameterNameUtil.IsInvalidParameterFormat(serializedProperty.stringValue, allowEmpty);

            var snapshot = InnerGUIEnvironmentScope.GetCurrentEnv().GetContext<ParametersContext>().Snapshot();
            if (!allowNew)
            {
                isInvalidValue |= snapshot.IsInvalidParameterReference(serializedProperty.stringValue);
            }

            using (new InvalidValueScope(isInvalidValue))
            {
                EditorGUI.BeginChangeCheck();
                var stringValue = EditorGUI.TextField(left, label, serializedProperty.stringValue);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedProperty.stringValue = stringValue;
                    GUI.changed = true; // propagate
                }
                EditorGUI.BeginChangeCheck();
                var filteredParameters = snapshot.AllParameters.Where(parameter => parameter.name.StartsWith(serializedProperty.stringValue)).ToArray();
                var parameterNames = filteredParameters.Select(parameter => parameter.name).ToArray();
                var parameterDisplayNames = filteredParameters.Select(parameter => $"{parameter.name} ({parameter.itemKind})").ToArray();
                var index = EditorGUI.Popup(right, -1, parameterDisplayNames);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedProperty.stringValue = parameterNames[index];
                    GUI.changed = true; // propagate
                }
            }
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = indent;
        }
    }
}