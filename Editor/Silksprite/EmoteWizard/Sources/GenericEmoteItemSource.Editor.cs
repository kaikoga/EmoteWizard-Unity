using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GenericEmoteItemSource))]
    public class GenericEmoteItemSourceEditor : EmoteWizardEditorBase<GenericEmoteItemSource>
    {
        LocalizedProperty _platform = null!;
        LocalizedProperty _vrcHandSign = null!;
        LocalizedProperty _vrm0BlendShape = null!;
        LocalizedProperty _vrm1Expression = null!;
        LocalizedProperty _name = null!;

        LocalizedProperty _sequence = null!;

        void OnEnable()
        {
            var serializedTrigger = Lop(nameof(GenericEmoteItemSource.trigger), Loc("GenericEmoteItemSource::trigger"));

            _platform = serializedTrigger.Lop(nameof(GenericEmoteTrigger.platform), Loc("GenericEmoteTrigger::platform"));
            _vrcHandSign = serializedTrigger.Lop(nameof(GenericEmoteTrigger.value), Loc("GenericEmoteTrigger::vrcHandSign"));
            _vrm0BlendShape = serializedTrigger.Lop(nameof(GenericEmoteTrigger.value), Loc("GenericEmoteTrigger::vrm0BlendShape"));
            _vrm1Expression = serializedTrigger.Lop(nameof(GenericEmoteTrigger.value), Loc("GenericEmoteTrigger::vrm1Expression"));
            _name = serializedTrigger.Lop(nameof(GenericEmoteTrigger.name), Loc("GenericEmoteTrigger::name"));

            _sequence = Lop(nameof(GenericEmoteItemSource.sequence), Loc("GenericEmoteItemSource::sequence"));
        }

        protected override void OnInnerInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            LEditorGUILayout.PropAsEnumPopup<GenericEmotePlatform>(_platform);
            if (EditorGUI.EndChangeCheck())
            {
                _vrcHandSign.Property.intValue = 0;
            }
            if (!_platform.Property.hasMultipleDifferentValues)
            {
                switch ((GenericEmotePlatform)_platform.Property.intValue)
                {
                    case GenericEmotePlatform.HandSigns:
                        LEditorGUILayout.PropAsEnumPopup<HandSign>(_vrcHandSign);
                        break;
                    case GenericEmotePlatform.VRM0:
                        LEditorGUILayout.PropAsEnumPopup<Vrm0BlendShapePreset>(_vrm0BlendShape);
                        if (_vrm0BlendShape.Property.intValue == (int)Vrm0BlendShapePreset.Unknown)
                        {
                            LEditorGUILayout.Prop(_name);
                        }
                        break;
                    case GenericEmotePlatform.VRM1:
                        LEditorGUILayout.PropAsEnumPopup<Vrm1ExpressionPreset>(_vrm1Expression);
                        if (_vrm1Expression.Property.intValue == (int)Vrm1ExpressionPreset.Custom)
                        {
                            LEditorGUILayout.Prop(_name);
                        }
                        break;
                }
            }

            LEditorGUILayout.Prop(_sequence);
            if (!_sequence.Property.objectReferenceValue)
            {
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(true))
                {
                    var obj = soleTarget.FindEmoteSequenceSource();
                    LEditorGUILayout.ObjectField(Loc("GenericEmoteItemSource::Detected Emote Sequence"), obj, true);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
