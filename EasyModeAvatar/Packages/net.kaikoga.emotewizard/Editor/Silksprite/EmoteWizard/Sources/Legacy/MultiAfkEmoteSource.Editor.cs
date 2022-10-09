using Silksprite.EmoteWizard.Sources.Legacy.Impl;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources.Legacy
{
    [CustomEditor(typeof(MultiAfkEmoteSource))]
    public class MultiAfkEmoteSourceEditor : Editor
    {
        MultiAfkEmoteSource _multiAfkEmoteSource;

        void OnEnable()
        {
            _multiAfkEmoteSource = (MultiAfkEmoteSource)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Legacy Data", $"{_multiAfkEmoteSource.afkEmotes.Count} Items");

            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}