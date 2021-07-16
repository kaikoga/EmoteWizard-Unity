using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.UI.Base.Legacy;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteGestureCondition))]
    public class EmoteGestureConditionDrawer : HybridDrawerBase<EmoteGestureCondition>
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
        
        public override void OnGUI(Rect position, ref EmoteGestureCondition property, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                TypedGUI.EnumPopup(position.UISliceH(0.00f, 0.33f), new GUIContent(" "), ref property.parameter);
                TypedGUI.EnumPopup(position.UISliceH(0.33f, 0.33f), new GUIContent(" "), ref property.mode);
                TypedGUI.EnumPopup(position.UISliceH(0.66f, 0.33f), new GUIContent(" "), ref property.handSign);
            }
        }

    }
}