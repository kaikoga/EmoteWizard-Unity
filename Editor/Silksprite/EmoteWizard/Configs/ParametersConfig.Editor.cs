using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

#if EW_VRCSDK3_AVATARS
using Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Extensions;
#endif

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(ParametersConfig))]
    public class ParametersConfigEditor : EmoteWizardEditorBase<ParametersConfig>
    {
        LocalizedProperty _outputAsset = null!;

        ParametersSnapshotDrawer? _drawer;

        void OnEnable()
        {
            _outputAsset = Lop(nameof(ParametersConfig.outputAsset), Loc("ParametersConfig::outputAsset"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var environment = CachedEnv();

            if (_drawer == null)
            {
                if (LGUILayout.Button(Loc("ParametersConfig::debugSnapshot")))
                {
                    _drawer = new ParametersSnapshotDrawer(environment.GetPlatformFeatures(), soleTarget.GetContext(environment).Snapshot());
                }
            }
            else
            {
                _drawer.OnGUI();
            }


            EmoteWizardSupportGUILayout.OutputUIArea(environment.PersistGeneratedAssets, () =>
            {
#if EW_VRCSDK3_AVATARS
                if (LGUILayout.Button(Loc("ParametersConfig::Generate Expression Parameters")))
                {
                    soleTarget.GetContext(soleTarget.CreateEnv()).BuildOutputAsset();
                }
#endif
                using (new EditorGUI.DisabledScope())
                using (new EditorGUI.DisabledScope(!SupportedPlatform.VRCSDK3_AVATARS))
                {
                    LEditorGUILayout.Prop(_outputAsset);
                }
            });
            serializedObject.ApplyModifiedProperties();
        }
    }
}