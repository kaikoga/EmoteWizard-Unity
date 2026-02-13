using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Configs
{
    [CustomEditor(typeof(EditorLayerConfig), true)]
    public class EditorLayerConfigEditor : EmoteWizardEditorBase<EditorLayerConfig>
    {
        LocalizedProperty _outputAsset;

        void OnEnable()
        {
            _outputAsset = Lop(nameof(EditorLayerConfig.outputAsset), Loc("EditorLayerConfig::outputAsset"));
        }

        protected override void OnInnerInspectorGUI()
        {
            EmoteWizardGUILayout.Prop(_outputAsset);
            serializedObject.ApplyModifiedProperties();
        }
    }
}