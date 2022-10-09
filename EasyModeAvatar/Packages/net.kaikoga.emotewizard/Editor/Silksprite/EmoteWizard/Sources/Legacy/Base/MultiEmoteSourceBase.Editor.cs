using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources.Legacy.Base
{
    [CustomEditor(typeof(MultiEmoteSourceBase), true)]
    public class MultiEmoteSourceEditor : Editor
    {
        MultiEmoteSourceBase _multiEmoteSource;

        void OnEnable()
        {
            _multiEmoteSource = (MultiEmoteSourceBase)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Legacy Data", $"{_multiEmoteSource.emotes.Count} Items");
            
            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}