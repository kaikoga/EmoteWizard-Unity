using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(SetupWizard))]
    public class SetupWizardEditor : Editor
    {
        SetupWizard _wizard;

        SerializedProperty _serializedIsSetupMode;

        void OnEnable()
        {
            _wizard = (SetupWizard) target;

            _serializedIsSetupMode = serializedObject.FindProperty(nameof(SetupWizard.isSetupMode));
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _wizard.EmoteWizardRoot;

            EditorGUILayout.PropertyField(_serializedIsSetupMode, new GUIContent("Enable Setup Only UI"));

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Generate Wizards"))
            {
                GenerateWizards(emoteWizardRoot);
            }

            EmoteWizardGUILayout.SetupOnlyUI(_wizard, () =>
            {
                if (GUILayout.Button("Quick Setup EmoteItems"))
                {
                    QuickSetupEmoteItems(emoteWizardRoot);
                }
            });

            if (GUILayout.Button("Complete setup and remove me"))
            {
                DestroySelf(emoteWizardRoot);
                return;
            }
            
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static void GenerateWizards(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.EnsureWizard<AvatarWizard>();
            emoteWizardRoot.EnsureWizard<ExpressionWizard>();
            emoteWizardRoot.EnsureWizard<ParametersWizard>();
            emoteWizardRoot.EnsureWizard<FxLayerWizard>();
            emoteWizardRoot.EnsureWizard<GestureLayerWizard>();
            emoteWizardRoot.EnsureWizard<ActionLayerWizard>();
        }

        void DestroySelf(EmoteWizardRoot emoteWizardRoot)
        {
            if (_wizard.gameObject != emoteWizardRoot.gameObject)
            {
                DestroyImmediate(_wizard.gameObject, true);
            }
            else
            {
                DestroyImmediate(_wizard, true);
            }
        }

        static void QuickSetupEmoteItems(EmoteWizardRoot emoteWizardRoot)
        {
            GenerateWizards(emoteWizardRoot);

            var expressionSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("Expression Sources");
            foreach (var expressionItem in DefaultActionEmote.PopulateDefaultExpressionItems("Default/", Enumerable.Empty<ExpressionItem>()))
            {
                var expressionSource = expressionSources.FindOrCreateChildComponent<ExpressionItemSource>(expressionItem.path);
                expressionSource.expressionItem = expressionItem;

            }

            var parameterSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("Parameter Sources");

            var fxSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("FX Sources");
            foreach (var fxItem in DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.FX))
            {
                var fxSource = fxSources.FindOrCreateChildComponent<EmoteItemSource>(fxItem.Trigger.name);
                fxSource.trigger = fxItem.Trigger;
                fxSource.gameObject.AddComponent<EmoteSequenceSource>().sequence = fxItem.Sequence;
            }
            var gestureSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("Gesture Sources");
            foreach (var gestureItem in DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.Gesture))
            {
                var gestureSource = gestureSources.FindOrCreateChildComponent<EmoteItemSource>(gestureItem.Trigger.name);
                gestureSource.trigger = gestureItem.Trigger;
                gestureSource.gameObject.AddComponent<EmoteSequenceSource>().sequence = gestureItem.Sequence;
            }
            var actionSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("Action Sources");
            foreach (var actionItem in DefaultActionEmote.EnumerateDefaultActionEmoteItems())
            {
                var actionSource = actionSources.FindOrCreateChildComponent<EmoteItemSource>(actionItem.Trigger.name);
                actionSource.trigger = actionItem.Trigger;
                actionSource.gameObject.AddComponent<EmoteSequenceSource>().sequence = actionItem.Sequence;
            }
        }

        static string Tutorial =>
            string.Join("\n",
                "EmoteWizardの初期セットアップと、破壊的な各設定のリセットを行います。",
                "セットアップ中に表示される各ボタンは既存の設定を一括で消去して上書きするため、注意して扱ってください。");
    }
}