using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Wizards
{
    [CustomEditor(typeof(DefaultSourcesWizard))]
    public class DefaultSourcesWizardEditor : EmoteWizardEditorBase<DefaultSourcesWizard>
    {
        LocalizedProperty _defaultSourceKind;
        LocalizedProperty _emoteItemKind;
        LocalizedProperty _emoteSequenceFactoryKind;

        void OnEnable()
        {
            _defaultSourceKind = Lop(nameof(DefaultSourcesWizard.defaultSourceKind), Loc("DefaultSourcesWizard::defaultSourceKind"));
            _emoteItemKind = Lop(nameof(DefaultSourcesWizard.emoteItemKind), Loc("DefaultSourcesWizard::emoteItemKind"));
            _emoteSequenceFactoryKind = Lop(nameof(DefaultSourcesWizard.emoteSequenceFactoryKind), Loc("DefaultSourcesWizard::emoteSequenceFactoryKind"));
        }

        protected override void OnInnerInspectorGUI()
        {
            EmoteWizardGUILayout.Prop(_defaultSourceKind);

            switch ((DefaultSourceKind) _defaultSourceKind.Property.enumValueIndex)
            {
                case DefaultSourceKind.Fx:
                case DefaultSourceKind.Gesture:
                    EmoteWizardGUILayout.Prop(_emoteItemKind);
                    EmoteWizardGUILayout.Prop(_emoteSequenceFactoryKind);
                    break;
            }

            serializedObject.ApplyModifiedProperties();

            string undoLabel = $"Add Default {soleTarget.defaultSourceKind} Items";
            if (EmoteWizardGUILayout.Undoable(Loc("DefaultSourcesWizard::Add"), undoLabel) is IUndoable undoable)
            {
                soleTarget.Explode(undoable, true);
            }
        }
    }
}