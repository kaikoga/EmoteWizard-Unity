using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Legacy
{
    [CustomPropertyDrawer(typeof(EmoteGestureCondition))]
    public class EmoteGestureConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var serializedParameter = serializedProperty.FindPropertyRelative(nameof(EmoteGestureCondition.parameter));
            var serializedMode = serializedProperty.FindPropertyRelative(nameof(EmoteGestureCondition.mode));
            var serializedHandSign = serializedProperty.FindPropertyRelative(nameof(EmoteGestureCondition.handSign));

            using (new EditorGUILayout.HorizontalScope())
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                EditorGUI.PropertyField(position.UISliceH(0.00f, 0.33f), serializedParameter);
                EditorGUI.PropertyField(position.UISliceH(0.33f, 0.33f), serializedMode);
                EditorGUI.PropertyField(position.UISliceH(0.66f, 0.33f), serializedHandSign);
            }
        }
    }
}