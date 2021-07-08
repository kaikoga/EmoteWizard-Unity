using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteCondition))]
    public class EmoteConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                EditorGUI.PropertyField(position.UISliceH(0.00f, 0.50f), property.FindPropertyRelative("parameter"), new GUIContent(" "));
                EditorGUI.PropertyField(position.UISliceH(0.50f, 0.25f), property.FindPropertyRelative("mode"), new GUIContent(" "));
                EditorGUI.PropertyField(position.UISliceH(0.75f, 0.50f), property.FindPropertyRelative("threshold"), new GUIContent(" "));
            }
        }
    }
}