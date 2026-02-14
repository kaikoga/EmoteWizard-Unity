using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

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
            _outputAsset = Lop(nameof(ExpressionConfig.outputAsset), Loc("ExpressionConfig::outputAsset"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CreateEnv();

            LEditorGUILayout.Prop(_buildAsSubAsset);

            EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
            {
#if EW_VRCSDK3_AVATARS
                if (LGUILayout.Button(Loc("ExpressionConfig::Generate Expression Menu"), new GUILayoutOption[0]))
                {
                    soleTarget.GetContext(soleTarget.CreateEnv()).BuildOutputAsset();
                }
#endif
                using (new EditorGUI.DisabledScope(!EmoteWizardConstants.SupportedPlatforms.VRCSDK3_AVATARS))
                {
                    LEditorGUILayout.Prop(_outputAsset);
                }
            });

            serializedObject.ApplyModifiedProperties();
        }
    }
}