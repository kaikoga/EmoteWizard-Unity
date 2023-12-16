using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EmptyEmoteSequenceSource))]
    public class EmptyEmoteSequenceSourceEditor : Editor
    {
        SerializedProperty _serializedLayerKind;
        SerializedProperty _serializedGroupName;
        EmptyEmoteSequenceSource _emptyEmoteSequenceSource;

        void OnEnable()
        {
            _serializedLayerKind = serializedObject.FindProperty(nameof(EmptyEmoteSequenceSource.layerKind));
            _serializedGroupName = serializedObject.FindProperty(nameof(EmptyEmoteSequenceSource.groupName));

            _emptyEmoteSequenceSource = (EmptyEmoteSequenceSource)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedLayerKind);
            EditorGUILayout.PropertyField(_serializedGroupName);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Explode"))
            {
                SourceExploder.ExplodeEmoteSequences(_emptyEmoteSequenceSource);
                return;
            }

            _emptyEmoteSequenceSource = (EmptyEmoteSequenceSource)target;
            EmoteWizardGUILayout.Tutorial(_emptyEmoteSequenceSource.CreateEnv(), Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "アニメーションの内容をここに登録します。",
                "Group Nameでアニメーションをグループ分けします。同時に再生したいアニメーションはGroupを分けてください。",
                "",
                "Exit Time: Exit Timeの設定をします。再生終了するタイプのアニメーションを設定します。",
                "Time Parameter: Motion Timeの設定をします。グリップやRadial Puppetに反応するタイプのアニメーションを設定します。",
                "Entry Clip: Clipに遷移する際にアニメーションを挿入します。",
                "Exit Clip: Clipから遷移する際にアニメーションを挿入します。",
                "Layer Blend: VRC Playable Layer Controlの設定をします。現状Actionレイヤー専用です。",
                "Tracking Override: VRC Animator Tracking Controlの設定をします。アニメーションの再生中に一時的にAnimationにする項目を登録します。");
    }
}
