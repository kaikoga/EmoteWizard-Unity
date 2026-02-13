using Silksprite.EmoteWizard.Base;
using Silksprite.Loch;
using Silksprite.Loch.UI;
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
            LEditorGUILayout.Prop(_outputAsset);
            serializedObject.ApplyModifiedProperties();
        }
    }
}