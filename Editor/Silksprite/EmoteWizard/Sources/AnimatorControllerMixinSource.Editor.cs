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
    [CustomEditor(typeof(AnimatorControllerMixinSource))]
    public class AnimatorControllerMixinSourceEditor : EmoteWizardEditorBase<AnimatorControllerMixinSource>
    {
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.VRCPlatforms;

        LocalizedProperty _animatorController = null!;
        LocalizedProperty _layerKind = null!;
        LocalizedProperty _order = null!;

        void OnEnable()
        {
            var serializedItem = Lop(nameof(AnimatorControllerMixinSource.mixin), Loc("AnimatorControllerMixinSource::mixin"));

            _animatorController = serializedItem.Lop(nameof(AnimatorControllerMixin.animatorController), Loc("AnimatorControllerMixin::animatorController"));
            _layerKind = serializedItem.Lop(nameof(AnimatorControllerMixin.layerKind), Loc("AnimatorControllerMixin::layerKind"));
            _order = serializedItem.Lop(nameof(AnimatorControllerMixin.order), Loc("AnimatorControllerMixin::order"));
        }

        protected override void OnInnerInspectorGUI()
        {
            LEditorGUILayout.Prop(_animatorController);
            LEditorGUILayout.PropAsEnumPopup<LayerKind>(_layerKind);
            LEditorGUILayout.Prop(_order);

            serializedObject.ApplyModifiedProperties();
        }
    }
}