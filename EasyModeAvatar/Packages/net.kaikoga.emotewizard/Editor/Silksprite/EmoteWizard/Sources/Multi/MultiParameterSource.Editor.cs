using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Multi
{
    [CustomEditor(typeof(MultiParameterSource))]
    public class MultiParameterSourceEditor : Editor
    {
        MultiParameterSource _multiParameterSource;

        void OnEnable()
        {
            _multiParameterSource = (MultiParameterSource) target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Legacy Data", $"{_multiParameterSource.parameterItems.Count} Items");

            if (GUILayout.Button("Explode"))
            {
                SourceExploder.Explode(_multiParameterSource);
            }

            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}