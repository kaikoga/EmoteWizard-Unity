using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EmoteItemSource))]
    public class EmoteItemSourceEditor : EmoteWizardEditorBase<EmoteItemSource>
    {
        LocalizedProperty _name;
        LocalizedProperty _priority;
        LocalizedProperty _conditions;

        LocalizedProperty _hasExpressionItem;
        LocalizedProperty _expressionItemPath;
        LocalizedProperty _expressionItemIcon;

        LocalizedProperty _sequence;

        void OnEnable()
        {
            var serializedTrigger = Lop(nameof(EmoteItemSource.trigger), Loc("EmoteItemSource::trigger"));

            _name = serializedTrigger.Lop(nameof(EmoteTrigger.name), Loc("EmoteTrigger::name"));
            _priority = serializedTrigger.Lop(nameof(EmoteTrigger.priority), Loc("EmoteTrigger::priority"));
            _conditions = serializedTrigger.Lop(nameof(EmoteTrigger.conditions), Loc("EmoteTrigger::conditions"));

            _hasExpressionItem = Lop(nameof(EmoteItemSource.hasExpressionItem), Loc("EmoteItemSource::hasExpressionItem"));
            _expressionItemPath = Lop(nameof(EmoteItemSource.expressionItemPath), Loc("EmoteItemSource::expressionItemPath"));
            _expressionItemIcon = Lop(nameof(EmoteItemSource.expressionItemIcon), Loc("EmoteItemSource::expressionItemIcon"));

            _sequence = Lop(nameof(EmoteItemSource.sequence), Loc("EmoteItemSource::sequence"));
        }

        protected override void OnInnerInspectorGUI()
        {
            EmoteWizardGUILayout.Prop(_name);
            EmoteWizardGUILayout.Prop(_priority);
            EmoteWizardGUILayout.Prop(_conditions);

            EmoteWizardGUILayout.Prop(_sequence);
            if (!_sequence.Property.objectReferenceValue)
            {
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(true))
                {
                    EmoteSequenceSourceBase obj = soleTarget.FindEmoteSequenceSource();
                    EmoteWizardGUILayout.ObjectField(Loc("EmoteItemSource::Detected Emote Sequence"), obj, true);
                }
            }

            using (new EditorGUI.DisabledScope(!soleTarget.CanAutoExpression))
            {
                EmoteWizardGUILayout.Prop(_hasExpressionItem);
            }
            if (soleTarget.IsAutoExpression)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EmoteWizardGUILayout.Prop(_expressionItemPath);
                    EmoteWizardGUILayout.Prop(_expressionItemIcon);
                }
            }

            serializedObject.ApplyModifiedProperties();

            if (soleTarget.LooksLikeMirrorItem)
            {
                EmoteWizardGUILayout.HelpBox(Loc("EmoteItemSource:MirrorInfo"), MessageType.Info);
            }
        }
    }
}
