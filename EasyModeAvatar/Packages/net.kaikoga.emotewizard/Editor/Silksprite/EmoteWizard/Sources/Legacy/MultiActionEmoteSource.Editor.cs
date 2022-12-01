using Silksprite.EmoteWizard.Sources.Legacy.Impl;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources.Legacy
{
    [CustomEditor(typeof(MultiActionEmoteSource))]
    public class MultiActionEmoteSourceEditor : Editor
    {
        MultiActionEmoteSource _multiActionEmoteSource;

        void OnEnable()
        {
            _multiActionEmoteSource = (MultiActionEmoteSource)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Legacy Data", $"{_multiActionEmoteSource.actionEmotes.Count} Items");

            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}