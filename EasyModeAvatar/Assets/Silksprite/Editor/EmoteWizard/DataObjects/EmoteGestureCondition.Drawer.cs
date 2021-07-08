using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteGestureCondition))]
    public class EmoteGestureConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                EditorGUI.PropertyField(position.UISliceH(0.00f, 0.33f), property.FindPropertyRelative("parameter"), new GUIContent(" "));
                EditorGUI.PropertyField(position.UISliceH(0.33f, 0.33f), property.FindPropertyRelative("mode"), new GUIContent(" "));
                EditorGUI.PropertyField(position.UISliceH(0.66f, 0.33f), property.FindPropertyRelative("handSign"), new GUIContent(" "));
            }
        }
    }
}