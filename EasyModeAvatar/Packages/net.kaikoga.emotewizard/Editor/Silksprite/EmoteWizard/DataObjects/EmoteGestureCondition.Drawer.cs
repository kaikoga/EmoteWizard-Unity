using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteGestureCondition))]
    public class EmoteGestureConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            using (new EditorGUILayout.HorizontalScope())
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                EditorGUI.PropertyField(position.UISliceH(0.00f, 0.33f), serializedProperty.FindPropertyRelative(nameof(EmoteGestureCondition.parameter)));
                EditorGUI.PropertyField(position.UISliceH(0.33f, 0.33f), serializedProperty.FindPropertyRelative(nameof(EmoteGestureCondition.mode)));
                EditorGUI.PropertyField(position.UISliceH(0.66f, 0.33f), serializedProperty.FindPropertyRelative(nameof(EmoteGestureCondition.handSign)));
            }
        }
    }
}