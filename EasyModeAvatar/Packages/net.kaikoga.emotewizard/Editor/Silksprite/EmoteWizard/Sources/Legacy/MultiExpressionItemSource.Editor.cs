using Silksprite.EmoteWizard.Sources.Legacy.Impl;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy
{
    [CustomEditor(typeof(MultiExpressionItemSource))]
    public class MultiExpressionItemSourceEditor : Editor
    {
        MultiExpressionItemSource _multiExpressionItemSource;

        void OnEnable()
        {
            _multiExpressionItemSource = (MultiExpressionItemSource)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Legacy Data", $"{_multiExpressionItemSource.expressionItems.Count} Items");

            if (GUILayout.Button("Explode"))
            {
                SourceExploder.Explode(_multiExpressionItemSource);
            }

            EditorGUILayout.HelpBox("古いデータです。", MessageType.Warning);
        }
    }
}