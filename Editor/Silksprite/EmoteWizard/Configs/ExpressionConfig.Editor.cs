using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

#if EW_VRCSDK3_AVATARS
using Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Extensions;
using UnityEngine;
#endif

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(ExpressionConfig))]
    public class ExpressionConfigEditor : EmoteWizardEditorBase<ExpressionConfig>
    {
        LocalizedProperty _buildAsSubAsset = null!;
        LocalizedProperty _outputAsset = null!;

        void OnEnable()
        {
            _buildAsSubAsset = Lop(nameof(ExpressionConfig.buildAsSubAsset), Loc("ExpressionConfig::buildAsSubAsset"));
            _outputAsset = Lop(nameof(ExpressionConfig.outputAsset), Loc("ExpressionConfig::outputAsset"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CachedEnv();

            LEditorGUILayout.Prop(_buildAsSubAsset);

            EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
            {
#if EW_VRCSDK3_AVATARS
                if (LGUILayout.Button(Loc("ExpressionConfig::Generate Expression Menu"), new GUILayoutOption[0]))
                {
                    soleTarget.GetContext(soleTarget.CreateEnv()).BuildOutputAsset();
                }
#endif
                using (new EditorGUI.DisabledScope(!SupportedPlatform.VRCSDK3_AVATARS))
                {
                    LEditorGUILayout.Prop(_outputAsset);
                }
            });

            serializedObject.ApplyModifiedProperties();
        }
    }
}