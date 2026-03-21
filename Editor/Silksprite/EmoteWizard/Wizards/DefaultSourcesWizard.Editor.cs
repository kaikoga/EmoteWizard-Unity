using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

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
            LEditorGUILayout.Prop(_defaultSourceKind);

            switch ((DefaultSourceKind) _defaultSourceKind.Property.enumValueIndex)
            {
                case DefaultSourceKind.Fx:
                case DefaultSourceKind.Gesture:
                    LEditorGUILayout.Prop(_emoteItemKind);
                    LEditorGUILayout.Prop(_emoteSequenceFactoryKind);
                    break;
            }

            serializedObject.ApplyModifiedProperties();

            string undoLabel = $"Add Default {soleTarget.defaultSourceKind} Items";
            if (EmoteWizardGUILayout.Undoable(Loc("DefaultSourcesWizard::Add"), undoLabel, out var undoable))
            {
                soleTarget.Explode(undoable, true);
            }
        }
    }
}