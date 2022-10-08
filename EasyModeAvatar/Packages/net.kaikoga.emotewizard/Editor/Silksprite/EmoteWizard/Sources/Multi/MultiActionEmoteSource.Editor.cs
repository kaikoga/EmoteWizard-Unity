using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Multi
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

            if (GUILayout.Button("Explode"))
            {
                SourceExploder.Explode(_multiActionEmoteSource);
            }

            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}