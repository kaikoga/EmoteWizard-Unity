using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(ParametersConfig))]
    public class ParametersConfigEditor : EmoteWizardEditorBase<ParametersConfig>
    {
        LocalizedProperty _outputAsset;
        LocalizedProperty _debugSnapshot;

        [SerializeField] ParametersSnapshot debugSnapshot;

        void OnEnable()
        {
            _outputAsset = Lop(nameof(ParametersConfig.outputAsset), Loc("ParametersConfig::outputAsset"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CreateEnv();

            if (_debugSnapshot == default)
            {
                if (LGUILayout.Button(Loc("ParametersConfig::debugSnapshot"), new GUILayoutOption[0]))
                {
                    debugSnapshot = soleTarget.GetContext(soleTarget.CreateEnv()).Snapshot();
                    _debugSnapshot = new SerializedObject(this).Lop(nameof(debugSnapshot), Loc("ParametersConfig::debugSnapshot"));
                }
            }
            else
            {
                LEditorGUILayout.Prop(_debugSnapshot);
            }

            EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
            {
#if EW_VRCSDK3_AVATARS
                if (LGUILayout.Button(Loc("ParametersConfig::Generate Expression Parameters"), new GUILayoutOption[0]))
                {
                    soleTarget.GetContext(soleTarget.CreateEnv()).BuildOutputAsset();
                }
#endif
                using (new EditorGUI.DisabledScope())
                using (new EditorGUI.DisabledScope(!EmoteWizardConstants.SupportedPlatforms.VRCSDK3_AVATARS))
                {
                    LEditorGUILayout.Prop(_outputAsset);
                }
            });
            serializedObject.ApplyModifiedProperties();
        }
    }
}