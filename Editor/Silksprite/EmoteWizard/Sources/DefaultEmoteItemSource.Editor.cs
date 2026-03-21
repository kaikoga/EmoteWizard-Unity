using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DefaultEmoteItemSource))]
    public class DefaultEmoteItemSourceEditor : EmoteWizardEditorBase<DefaultEmoteItemSource>
    {
        LocalizedProperty _emoteItemKind;
        LocalizedProperty _emoteSequenceFactoryKind;
        LocalizedProperty _layerKind;
        LocalizedProperty _handSign;

        void OnEnable()
        {
            _emoteItemKind = Lop(nameof(DefaultEmoteItemSource.emoteItemKind), Loc("DefaultEmoteItemSource::emoteItemKind"));
            _emoteSequenceFactoryKind = Lop(nameof(DefaultEmoteItemSource.emoteSequenceFactoryKind), Loc("DefaultEmoteItemSource::emoteSequenceFactoryKind"));
            _layerKind = Lop(nameof(DefaultEmoteItemSource.layerKind), Loc("DefaultEmoteItemSource::layerKind"));
            _handSign = Lop(nameof(DefaultEmoteItemSource.handSign), Loc("DefaultEmoteItemSource::handSign"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CreateEnv();

            LEditorGUILayout.Prop(_emoteItemKind);
            LEditorGUILayout.Prop(_emoteSequenceFactoryKind);
            LEditorGUILayout.Prop(_layerKind);
            LEditorGUILayout.Prop(_handSign);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
