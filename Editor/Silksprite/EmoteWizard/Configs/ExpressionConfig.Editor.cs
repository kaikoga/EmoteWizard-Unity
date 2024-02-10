using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(ExpressionConfig))]
    public class ExpressionConfigEditor : EmoteWizardEditorBase<ExpressionConfig>
    {
        LocalizedProperty _buildAsSubAsset;
        LocalizedProperty _outputAsset;

        void OnEnable()
        {
            _buildAsSubAsset = Lop(nameof(ExpressionConfig.buildAsSubAsset), Loc("ExpressionConfig::buildAsSubAsset"));
            using (new EditorGUI.DisabledScope(!EmoteWizardConstants.Platforms.VRCSDK3_AVATARS))
            {
                _outputAsset = Lop(nameof(ExpressionConfig.outputAsset), Loc("ExpressionConfig::outputAsset"));
            }
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CreateEnv();

            EmoteWizardGUILayout.Prop(_buildAsSubAsset);

            EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
            {
#if EW_VRCSDK3_AVATARS
                if (EmoteWizardGUILayout.Button(Loc("ExpressionConfig::Generate Expression Menu")))
                {
                    soleTarget.GetContext(soleTarget.CreateEnv()).BuildOutputAsset();
                }
#endif
                EmoteWizardGUILayout.Prop(_outputAsset);
            });

            serializedObject.ApplyModifiedProperties();
        }
    }
}