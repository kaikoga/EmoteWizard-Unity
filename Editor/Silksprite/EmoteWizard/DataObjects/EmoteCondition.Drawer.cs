using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteCondition))]
    public class EmoteConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var parameter = serializedProperty.Lop(nameof(EmoteCondition.parameter), Loc("EmoteCondition::parameter"));
            var kind = serializedProperty.Lop(nameof(EmoteCondition.kind), Loc("EmoteCondition::kind"));
            var mode = serializedProperty.Lop(nameof(EmoteCondition.mode), Loc("EmoteCondition::mode"));
            var threshold = serializedProperty.Lop(nameof(EmoteCondition.threshold), Loc("EmoteCondition::threshold"));

            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                EditorGUI.PropertyField(position.UISliceH(0.0f, 0.4f), parameter.Property, parameter.GUIContent);
                EditorGUI.PropertyField(position.UISliceH(0.4f, 0.2f), kind.Property, kind.GUIContent);
                EditorGUI.PropertyField(position.UISliceH(0.6f, 0.2f), mode.Property, mode.GUIContent);
                EditorGUI.PropertyField(position.UISliceH(0.8f, 0.2f), threshold.Property, threshold.GUIContent);
            }
        }
    }
}