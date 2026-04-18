using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(AnimationClipMixinSource))]
    public class AnimationClipMixinSourceEditor : EmoteWizardEditorBase<AnimationClipMixinSource>
    {
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.VRCPlatforms;

        LocalizedProperty _clip = null!;
        LocalizedProperty _layerKind = null!;
        LocalizedProperty _order = null!;

        void OnEnable()
        {
            var serializedItem = Lop(nameof(AnimationClipMixinSource.mixin), Loc("AnimationClipMixinSource::mixin"));

            _clip = serializedItem.Lop(nameof(AnimationClipMixin.clip), Loc("AnimationClipMixin::clip"));
            _layerKind = serializedItem.Lop(nameof(AnimationClipMixin.layerKind), Loc("AnimationClipMixin::layerKind"));
            _order = serializedItem.Lop(nameof(AnimationClipMixin.order), Loc("AnimationClipMixin::order"));
        }

        protected override void OnInnerInspectorGUI()
        {
            LEditorGUILayout.Prop(_clip);
            LEditorGUILayout.PropAsEnumPopup<LayerKind>(_layerKind);
            LEditorGUILayout.Prop(_order);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
