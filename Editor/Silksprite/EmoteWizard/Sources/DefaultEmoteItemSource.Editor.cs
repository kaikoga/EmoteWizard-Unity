using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DefaultEmoteItemSource))]
    public class DefaultEmoteItemSourceEditor : EmoteWizardEditorBase<DefaultEmoteItemSource>
    {
        LocalizedProperty _emoteItemKind = null!;
        LocalizedProperty _emoteSequenceFactoryKind = null!;
        LocalizedProperty _layerKind = null!;
        LocalizedProperty _handSign = null!;

        void OnEnable()
        {
            _emoteItemKind = Lop(nameof(DefaultEmoteItemSource.emoteItemKind), Loc("DefaultEmoteItemSource::emoteItemKind"));
            _emoteSequenceFactoryKind = Lop(nameof(DefaultEmoteItemSource.emoteSequenceFactoryKind), Loc("DefaultEmoteItemSource::emoteSequenceFactoryKind"));
            _layerKind = Lop(nameof(DefaultEmoteItemSource.layerKind), Loc("DefaultEmoteItemSource::layerKind"));
            _handSign = Lop(nameof(DefaultEmoteItemSource.handSign), Loc("DefaultEmoteItemSource::handSign"));
        }

        protected override void OnInnerInspectorGUI()
        {
            LEditorGUILayout.PropAsEnumPopup<EmoteItemKind>(_emoteItemKind);
            LEditorGUILayout.PropAsEnumPopup<EmoteSequenceFactoryKind>(_emoteSequenceFactoryKind);
            LEditorGUILayout.PropAsEnumPopup<LayerKind>(_layerKind);
            LEditorGUILayout.PropAsEnumPopup<HandSign>(_handSign);

            serializedObject.ApplyModifiedProperties();

            if (EmoteWizardSupportGUILayout.Undoable(Loc("DefaultEmoteItemSource::Unpack"), "Unpack Default Emote Item Source", out var undoable))
            {
                soleTarget.Explode(CachedEnv(), undoable, true);
            }

        }
    }
}
