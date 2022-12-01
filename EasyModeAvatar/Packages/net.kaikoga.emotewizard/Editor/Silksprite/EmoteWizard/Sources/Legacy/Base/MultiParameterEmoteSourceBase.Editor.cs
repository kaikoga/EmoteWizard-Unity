using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources.Legacy.Base
{
    [CustomEditor(typeof(MultiParameterEmoteSourceBase), true)]
    public class MultiParameterEmoteSourceBaseEditor : Editor
    {
        MultiParameterEmoteSourceBase _multiParameterEmoteSource;

        void OnEnable()
        {
            _multiParameterEmoteSource = (MultiParameterEmoteSourceBase)target;

        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Legacy Data", $"{_multiParameterEmoteSource.parameterEmotes.Count} Items");
            
            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}