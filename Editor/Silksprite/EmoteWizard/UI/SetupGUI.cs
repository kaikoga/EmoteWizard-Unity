using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;

namespace Silksprite.EmoteWizard.UI
{
    public static class SetupGUI
    {
        public static void OnInspectorGUI(EmoteWizardEnvironment env)
        {
            if (GUILayout.Button("Quick Setup EmoteItems (All Sources)"))
            {
                QuickSetupEmoteItems(env);
            }

            GUILayout.Space(4f);

            if (!env.Find(Names.ExpressionSources))
            {
                if (GUILayout.Button("Quick Setup Expression Sources"))
                {
                    QuickSetupExpressionSources(env);
                }
            }
            if (!env.Find(Names.ParameterSources))
            {
                if (GUILayout.Button("Quick Setup Parameter Sources"))
                {
                    QuickSetupParameterSources(env);
                }
            }
            if (!env.Find(Names.FXSources))
            {
                if (GUILayout.Button("Quick Setup FX Sources"))
                {
                    QuickSetupFXSources(env);
                }
            }
            if (!env.Find(Names.GestureSources))
            {
                if (GUILayout.Button("Quick Setup Gesture Sources"))
                {
                    QuickSetupGestureSources(env);
                }
            }
            if (!env.Find(Names.ActionSources))
            {
                if (GUILayout.Button("Quick Setup Action Sources"))
                {
                    QuickSetupActionSources(env);
                }
            }

            if (GUILayout.Button("Generate Intermediate Wizards"))
            {
                GenerateConfigs(env);
            }
        }

        static void GenerateConfigs(EmoteWizardEnvironment environment)
        {
            environment.AddWizard<ExpressionConfig>();
            environment.AddWizard<ParametersConfig>();
            environment.AddWizard<FxLayerConfig>();
            environment.AddWizard<GestureLayerConfig>();
            environment.AddWizard<ActionLayerConfig>();
        }

        static void QuickSetupEmoteItems(EmoteWizardEnvironment environment)
        {
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
            template.SequenceFactory.PopulateSequenceSource(source);
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
            environment.OverrideGesture = OverrideGeneratedControllerType2.Generate;
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
            environment.OverrideAction = OverrideGeneratedControllerType1.Generate;
            environment.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(Names.ActionSources, actionSources =>
            {
                foreach (var actionItem in DefaultActionEmote.EnumerateDefaultActionEmoteItems())
                {
                    var actionSource = actionSources.FindOrCreateChildComponent<EmoteItemSource>(actionItem.Trigger.name);
                    PopulateEmoteItemSource(actionSource, actionItem);
                }
            });
        }

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