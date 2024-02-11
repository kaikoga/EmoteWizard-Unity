using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
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
        LocalizedProperty _layerKind;
        LocalizedProperty _emoteItemKind;
        LocalizedProperty _emoteSequenceFactoryKind;

        void OnEnable()
        {
            _layerKind = Lop(nameof(DefaultSourcesWizard.layerKind), Loc("DefaultSourcesWizard::layerKind"));
            _emoteItemKind = Lop(nameof(DefaultSourcesWizard.emoteItemKind), Loc("DefaultSourcesWizard::emoteItemKind"));
            _emoteSequenceFactoryKind = Lop(nameof(DefaultSourcesWizard.emoteSequenceFactoryKind), Loc("DefaultSourcesWizard::emoteSequenceFactoryKind"));
        }

        protected override void OnInnerInspectorGUI()
        {
            EmoteWizardGUILayout.Prop(_layerKind);

            switch ((LayerKind) _layerKind.Property.enumValueIndex)
            {
                case LayerKind.FX:
                case LayerKind.Gesture:
                    EmoteWizardGUILayout.Prop(_emoteItemKind);
                    EmoteWizardGUILayout.Prop(_emoteSequenceFactoryKind);
                    break;
            }

            serializedObject.ApplyModifiedProperties();

            string undoLabel = $"Add Default {soleTarget.layerKind} Items";
            if (EmoteWizardGUILayout.Undoable(Loc("DefaultSourcesWizard::Add"), undoLabel) is IUndoable undoable)
            {
                soleTarget.Explode(undoable, true);
            }
        }
    }
}