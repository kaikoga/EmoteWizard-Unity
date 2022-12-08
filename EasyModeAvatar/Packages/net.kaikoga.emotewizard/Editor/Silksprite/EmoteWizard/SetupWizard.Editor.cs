using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
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
                if (GUILayout.Button("Quick Setup EmoteItems (All Sources)"))
                {
                    QuickSetupEmoteItems(emoteWizardRoot);
                }

                if (emoteWizardRoot.transform.childCount <= 0) return;

                GUILayout.Space(4f);

                if (!emoteWizardRoot.transform.Find(Names.ExpressionSources))
                {
                    if (GUILayout.Button("Quick Setup Expression Sources"))
                    {
                        QuickSetupExpressionSources(emoteWizardRoot);
                    }
                }
                if (!emoteWizardRoot.transform.Find(Names.ParameterSources))
                {
                    if (GUILayout.Button("Quick Setup Parameter Sources"))
                    {
                        QuickSetupParameterSources(emoteWizardRoot);
                    }
                }
                if (!emoteWizardRoot.transform.Find(Names.FXSources))
                {
                    if (GUILayout.Button("Quick Setup FX Sources"))
                    {
                        QuickSetupFXSources(emoteWizardRoot);
                    }
                }
                if (!emoteWizardRoot.transform.Find(Names.GestureSources))
                {
                    if (GUILayout.Button("Quick Setup Gesture Sources"))
                    {
                        QuickSetupGestureSources(emoteWizardRoot);
                    }
                }
                if (!emoteWizardRoot.transform.Find(Names.ActionSources))
                {
                    if (GUILayout.Button("Quick Setup Action Sources"))
                    {
                        QuickSetupActionSources(emoteWizardRoot);
                    }
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
            emoteWizardRoot.EnsureWizard<GestureLayerWizard>(gestureWizard =>
            {
                gestureWizard.defaultAvatarMask = VrcSdkAssetLocator.HandsOnly();
            });
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

            QuickSetupExpressionSources(emoteWizardRoot);
            QuickSetupParameterSources(emoteWizardRoot);
            QuickSetupFXSources(emoteWizardRoot);
            QuickSetupGestureSources(emoteWizardRoot);
            QuickSetupActionSources(emoteWizardRoot);
        }

        static void QuickSetupExpressionSources(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.ExpressionSources, expressionSources =>
            {
                foreach (var expressionItem in DefaultActionEmote.PopulateDefaultExpressionItems("Default/"))
                {
                    var expressionSource = expressionSources.FindOrCreateChildComponent<ExpressionItemSource>(expressionItem.path);
                    expressionSource.expressionItem = expressionItem;
                }
            });
        }

        static void QuickSetupParameterSources(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.ParameterSources, parameterSources => { });
        }

        static void QuickSetupFXSources(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.FXSources, fxSources =>
            {
                foreach (var fxItem in DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.FX))
                {
                    var fxSource = fxSources.FindOrCreateChildComponent<EmoteItemSource>(fxItem.Trigger.name);
                    fxSource.trigger = fxItem.Trigger;
                    fxSource.gameObject.AddComponent<EmoteSequenceSource>().sequence = fxItem.Sequence;
                }
            });
        }

        static void QuickSetupGestureSources(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.GestureSources, gestureSources =>
            {
                foreach (var gestureItem in DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.Gesture))
                {
                    var gestureSource = gestureSources.FindOrCreateChildComponent<EmoteItemSource>(gestureItem.Trigger.name);
                    gestureSource.trigger = gestureItem.Trigger;
                    gestureSource.gameObject.AddComponent<EmoteSequenceSource>().sequence = gestureItem.Sequence;
                }
            });
        }

        static void QuickSetupActionSources(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.ActionSources, actionSources =>
            {
                foreach (var actionItem in DefaultActionEmote.EnumerateDefaultActionEmoteItems())
                {
                    var actionSource = actionSources.FindOrCreateChildComponent<EmoteItemSource>(actionItem.Trigger.name);
                    actionSource.trigger = actionItem.Trigger;
                    actionSource.gameObject.AddComponent<EmoteSequenceSource>().sequence = actionItem.Sequence;
                }
            });
        }

        static string Tutorial =>
            string.Join("\n",
                "EmoteWizardの初期セットアップと、破壊的な各設定のリセットを行います。",
                "セットアップ中に表示される各ボタンは既存の設定を一括で消去して上書きするため、注意して扱ってください。");

        static class Names
        {
            public const string ExpressionSources = "Expression Sources";
            public const string ParameterSources = "Parameter Sources";
            public const string FXSources = "FX Sources";
            public const string GestureSources = "Gesture Sources";
            public const string ActionSources = "Action Sources";
        }
    }
}