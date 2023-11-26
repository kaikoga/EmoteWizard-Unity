using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
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
            var env = _wizard.CreateEnv();

            EditorGUILayout.PropertyField(_serializedIsSetupMode, new GUIContent("Enable Setup Only UI"));

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Generate Wizards"))
            {
                GenerateWizards(env);
            }

            EmoteWizardGUILayout.SetupOnlyUI(_wizard, () =>
            {
                if (GUILayout.Button("Quick Setup EmoteItems (All Sources)"))
                {
                    QuickSetupEmoteItems(env);
                }

                if (env.Transform.childCount <= 0) return;

                GUILayout.Space(4f);

                if (!env.Transform.Find(Names.ExpressionSources))
                {
                    if (GUILayout.Button("Quick Setup Expression Sources"))
                    {
                        QuickSetupExpressionSources(env);
                    }
                }
                if (!env.Transform.Find(Names.ParameterSources))
                {
                    if (GUILayout.Button("Quick Setup Parameter Sources"))
                    {
                        QuickSetupParameterSources(env);
                    }
                }
                if (!env.Transform.Find(Names.FXSources))
                {
                    if (GUILayout.Button("Quick Setup FX Sources"))
                    {
                        QuickSetupFXSources(env);
                    }
                }
                if (!env.Transform.Find(Names.GestureSources))
                {
                    if (GUILayout.Button("Quick Setup Gesture Sources"))
                    {
                        QuickSetupGestureSources(env);
                    }
                }
                if (!env.Transform.Find(Names.ActionSources))
                {
                    if (GUILayout.Button("Quick Setup Action Sources"))
                    {
                        QuickSetupActionSources(env);
                    }
                }
            });

            if (GUILayout.Button("Complete setup and remove me"))
            {
                DestroySelf(env);
                return;
            }
            
            EmoteWizardGUILayout.Tutorial(env, Tutorial);
        }

        static void GenerateWizards(EmoteWizardEnvironment environment)
        {
            environment.AddWizard<AvatarWizard>();
            environment.AddWizard<ExpressionWizard>();
            environment.AddWizard<ParametersWizard>();
            environment.AddWizard<FxLayerWizard>();
            environment.AddWizard<GestureLayerWizard>();
            environment.AddWizard<ActionLayerWizard>();
        }

        void DestroySelf(EmoteWizardEnvironment environment)
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

        static void QuickSetupEmoteItems(EmoteWizardEnvironment environment)
        {
            GenerateWizards(environment);

            QuickSetupExpressionSources(environment);
            QuickSetupParameterSources(environment);
            QuickSetupFXSources(environment);
            QuickSetupGestureSources(environment);
            QuickSetupActionSources(environment);
        }

        static void QuickSetupExpressionSources(EmoteWizardEnvironment environment)
        {
            environment.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.ExpressionSources);
        }

        static void QuickSetupParameterSources(EmoteWizardEnvironment environment)
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

        static void QuickSetupFXSources(EmoteWizardEnvironment environment)
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

        static void QuickSetupGestureSources(EmoteWizardEnvironment environment)
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

        static void QuickSetupActionSources(EmoteWizardEnvironment environment)
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