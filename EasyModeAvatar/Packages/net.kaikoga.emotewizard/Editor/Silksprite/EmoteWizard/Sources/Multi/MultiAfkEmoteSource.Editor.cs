using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Multi
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

            if (GUILayout.Button("Explode"))
            {
                SourceExploder.Explode(_multiAfkEmoteSource);
            }

            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}