using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
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
            var context = _wizard.Environment;

            EditorGUILayout.PropertyField(_serializedIsSetupMode, new GUIContent("Enable Setup Only UI"));

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Generate Wizards"))
            {
                GenerateWizards(context);
            }

            EmoteWizardGUILayout.SetupOnlyUI(_wizard, () =>
            {
                if (GUILayout.Button("Quick Setup EmoteItems (All Sources)"))
                {
                    QuickSetupEmoteItems(context);
                }

                if (context.Transform.childCount <= 0) return;

                GUILayout.Space(4f);

                if (!context.Transform.Find(Names.ExpressionSources))
                {
                    if (GUILayout.Button("Quick Setup Expression Sources"))
                    {
                        QuickSetupExpressionSources(context);
                    }
                }
                if (!context.Transform.Find(Names.ParameterSources))
                {
                    if (GUILayout.Button("Quick Setup Parameter Sources"))
                    {
                        QuickSetupParameterSources(context);
                    }
                }
                if (!context.Transform.Find(Names.FXSources))
                {
                    if (GUILayout.Button("Quick Setup FX Sources"))
                    {
                        QuickSetupFXSources(context);
                    }
                }
                if (!context.Transform.Find(Names.GestureSources))
                {
                    if (GUILayout.Button("Quick Setup Gesture Sources"))
                    {
                        QuickSetupGestureSources(context);
                    }
                }
                if (!context.Transform.Find(Names.ActionSources))
                {
                    if (GUILayout.Button("Quick Setup Action Sources"))
                    {
                        QuickSetupActionSources(context);
                    }
                }
            });

            if (GUILayout.Button("Complete setup and remove me"))
            {
                DestroySelf(context);
                return;
            }
            
            EmoteWizardGUILayout.Tutorial(context, Tutorial);
        }

        static void GenerateWizards(IEmoteWizardEnvironment environment)
        {
            environment.EnsureWizard<AvatarWizard>();
            environment.EnsureWizard<ExpressionWizard>();
            environment.EnsureWizard<ParametersWizard>();
            environment.EnsureWizard<FxLayerWizard>();
            environment.EnsureWizard<GestureLayerWizard>(gestureWizard =>
            {
                gestureWizard.defaultAvatarMask = VrcSdkAssetLocator.HandsOnly();
            });
            environment.EnsureWizard<ActionLayerWizard>();
        }

        void DestroySelf(IEmoteWizardEnvironment environment)
        {
            if (_wizard.gameObject != environment.GameObject)
            {
                DestroyImmediate(_wizard.gameObject, true);
            }
            else
            {
                DestroyImmediate(_wizard, true);
            }
        }

        static void QuickSetupEmoteItems(IEmoteWizardEnvironment environment)
        {
            GenerateWizards(environment);

            QuickSetupExpressionSources(environment);
            QuickSetupParameterSources(environment);
            QuickSetupFXSources(environment);
            QuickSetupGestureSources(environment);
            QuickSetupActionSources(environment);
        }

        static void QuickSetupExpressionSources(IEmoteWizardEnvironment environment)
        {
            environment.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.ExpressionSources);
        }

        static void QuickSetupParameterSources(IEmoteWizardEnvironment environment)
        {
            environment.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.ParameterSources);
        }

        static void PopulateEmoteItemSource(EmoteItemSource source, EmoteItemTemplate template)
        {
            source.trigger = template.Trigger;
            source.gameObject.AddComponent<EmoteSequenceSource>().sequence = template.Sequence;
            source.hasExpressionItem = template.HasExpressionItem;
            source.expressionItemPath = template.ExpressionItemPath;
            source.expressionItemIcon = template.ExpressionItemIcon;
        }

        static void QuickSetupFXSources(IEmoteWizardEnvironment environment)
        {
            environment.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.FXSources, fxSources =>
            {
                foreach (var fxItem in DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.FX))
                {
                    var fxSource = fxSources.FindOrCreateChildComponent<EmoteItemSource>(fxItem.Trigger.name);
                    PopulateEmoteItemSource(fxSource, fxItem);
                }
            });
        }

        static void QuickSetupGestureSources(IEmoteWizardEnvironment environment)
        {
            environment.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.GestureSources, gestureSources =>
            {
                foreach (var gestureItem in DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.Gesture))
                {
                    var gestureSource = gestureSources.FindOrCreateChildComponent<EmoteItemSource>(gestureItem.Trigger.name);
                    PopulateEmoteItemSource(gestureSource, gestureItem);
                }
            });
        }

        static void QuickSetupActionSources(IEmoteWizardEnvironment environment)
        {
            environment.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.ActionSources, actionSources =>
            {
                foreach (var actionItem in DefaultActionEmote.EnumerateDefaultActionEmoteItems())
                {
                    var actionSource = actionSources.FindOrCreateChildComponent<EmoteItemSource>(actionItem.Trigger.name);
                    PopulateEmoteItemSource(actionSource, actionItem);
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