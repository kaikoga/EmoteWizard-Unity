using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

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
                LEditorGUI.Prop(position.UISliceH(0.0f, 0.4f), parameter);
                LEditorGUI.Prop(position.UISliceH(0.4f, 0.2f), kind);
                LEditorGUI.Prop(position.UISliceH(0.6f, 0.2f), mode);
                LEditorGUI.Prop(position.UISliceH(0.8f, 0.2f), threshold);
            }
        }
    }
}