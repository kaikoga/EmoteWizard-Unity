using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
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
        LocalizedProperty? _debugSnapshot;

        [SerializeField] ParametersSnapshot? debugSnapshot;

        void OnEnable()
        {
            _outputAsset = Lop(nameof(ParametersConfig.outputAsset), Loc("ParametersConfig::outputAsset"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var environment = CachedEnv();

            if (_debugSnapshot == null)
            {
                if (LGUILayout.Button(Loc("ParametersConfig::debugSnapshot"), new GUILayoutOption[0]))
                {
                    debugSnapshot = soleTarget.GetContext(environment).Snapshot();
                    _debugSnapshot = new SerializedObject(this).Lop(nameof(debugSnapshot), Loc("ParametersConfig::debugSnapshot"));
                }
            }
            else
            {
                LEditorGUILayout.Prop(_debugSnapshot);
            }

            EmoteWizardSupportGUILayout.OutputUIArea(environment.PersistGeneratedAssets, () =>
            {
#if EW_VRCSDK3_AVATARS
                if (LGUILayout.Button(Loc("ParametersConfig::Generate Expression Parameters"), new GUILayoutOption[0]))
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