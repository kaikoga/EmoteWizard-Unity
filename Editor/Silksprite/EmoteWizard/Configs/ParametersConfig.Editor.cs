using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(ParametersConfig))]
    public class ParametersConfigEditor : EmoteWizardEditorBase<ParametersConfig>
    {
        LocalizedProperty _outputAsset;

        void OnEnable()
        {
            _outputAsset = Lop(nameof(ParametersConfig.outputAsset), Loc("ParametersConfig::outputAsset"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CreateEnv();

            EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
            {
#if EW_VRCSDK3_AVATARS
                if (EmoteWizardGUILayout.Button(Loc("ParametersConfig::Generate Expression Parameters")))
                {
                    soleTarget.GetContext(soleTarget.CreateEnv()).BuildOutputAsset();
                }
#endif
                using (new EditorGUI.DisabledScope())
                using (new EditorGUI.DisabledScope(!EmoteWizardConstants.SupportedPlatforms.VRCSDK3_AVATARS))
                {
                    EmoteWizardGUILayout.Prop(_outputAsset);
                }
            });
            serializedObject.ApplyModifiedProperties();
        }
    }
}