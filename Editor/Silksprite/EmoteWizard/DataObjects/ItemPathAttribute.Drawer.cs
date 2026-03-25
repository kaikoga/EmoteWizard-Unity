using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ItemPathAttribute))]
    public class ItemPathAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            PropertyField(position, serializedProperty, label);
        }

        public static void PropertyField(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var isInvalidValue = ItemPathUtil.IsInvalidPathFormat(serializedProperty.stringValue);
            
            using (new InvalidValueScope(isInvalidValue))
            {
                EditorGUI.PropertyField(position, serializedProperty, label);
            }
        }
    }
}