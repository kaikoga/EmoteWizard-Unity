using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(AnimatorLayerConfigBase), true)]
    public class AnimatorLayerConfigBaseEditor : EmoteWizardEditorBase<AnimatorLayerConfigBase>
    {
        LocalizedProperty _defaultAvatarMask;
        LocalizedProperty _outputAsset;
        LocalizedProperty _hasResetClip;
        LocalizedProperty _resetClip;

        void OnEnable()
        {
            _defaultAvatarMask = Lop(nameof(AnimatorLayerConfigBase.defaultAvatarMask), Loc("AnimatorLayerConfigBase::defaultAvatarMask"));
            _outputAsset = Lop(nameof(AnimatorLayerConfigBase.outputAsset), Loc("AnimatorLayerConfigBase::outputAsset"));
            _hasResetClip = Lop(nameof(AnimatorLayerConfigBase.hasResetClip), Loc("AnimatorLayerConfigBase::hasResetClip"));
            _resetClip = Lop(nameof(AnimatorLayerConfigBase.resetClip), Loc("AnimatorLayerConfigBase::resetClip"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CreateEnv();

            using (new ObjectChangeScope(soleTarget))
            {
                if (soleTarget.LayerKind == LayerKind.Gesture)
                {
                    EmoteWizardGUILayout.PropertyFieldWithGenerate(_defaultAvatarMask, () =>
                    {
                        var avatarMask = env.EnsureAsset<AvatarMask>(GeneratedPaths.GestureDefaultMask);
                        return AvatarMaskUtils.SetupAsGestureDefault(avatarMask);
                    });
                }
                else
                {
                    EmoteWizardGUILayout.Prop(_defaultAvatarMask);
                }

                EmoteWizardGUILayout.Prop(_hasResetClip);

                EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
                {
#if EW_VRCSDK3_AVATARS
                    if (EmoteWizardGUILayout.Button(Loc("AnimatorLayerConfigBase::Generate Animation Controller")))
                    {
                        soleTarget.GetContext(soleTarget.CreateEnv()).BuildOutputAsset(env.GetContext<ParametersContext>().Snapshot());
                    }
#endif

                    EmoteWizardGUILayout.Prop(_outputAsset);
                    using (new EditorGUI.DisabledScope(!soleTarget.hasResetClip))
                    {
                        EmoteWizardGUILayout.Prop(_resetClip);
                    }
                });
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}