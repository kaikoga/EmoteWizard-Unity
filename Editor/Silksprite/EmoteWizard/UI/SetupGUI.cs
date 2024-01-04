using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.UI
{
    public static class SetupGUI
    {
        public static bool OnInspectorGUI(EmoteWizardEnvironment env)
        {
            if (GUILayout.Button("Quick Setup EmoteItems (All Sources)"))
            {
                QuickSetupEmoteItems(new EditorUndoable("Quick Setup EmoteItems (All Sources)"), env);
                return true;
            }

            GUILayout.Space(4f);

            if (!env.Find(Names.ExpressionSources))
            {
                if (GUILayout.Button("Quick Setup Expression Sources"))
                {
                    QuickSetupExpressionSources(new EditorUndoable("Quick Setup Expression Sources"), env);
                    return true;
                }
            }
            if (!env.Find(Names.ParameterSources))
            {
                if (GUILayout.Button("Quick Setup Parameter Sources"))
                {
                    QuickSetupParameterSources(new EditorUndoable("Quick Setup Parameter Sources"), env);
                    return true;
                }
            }
            if (!env.Find(Names.FXSources))
            {
                if (GUILayout.Button("Quick Setup FX Sources"))
                {
                    QuickSetupFXSources(new EditorUndoable("Quick Setup FX Sources"), env);
                    return true;
                }
            }
            if (!env.Find(Names.GestureSources))
            {
                if (GUILayout.Button("Quick Setup Gesture Sources"))
                {
                    QuickSetupGestureSources(new EditorUndoable("Quick Setup Gesture Sources"), env);
                    return true;
                }
            }
            if (!env.Find(Names.ActionSources))
            {
                if (GUILayout.Button("Quick Setup Action Sources"))
                {
                    QuickSetupActionSources(new EditorUndoable("Quick Setup Action Sources"), env);
                    return true;
                }
            }

            if (GUILayout.Button("Generate Intermediate Wizards"))
            {
                GenerateConfigs(env);
                return true;
            }
            return false;
        }

        static void GenerateConfigs(EmoteWizardEnvironment environment)
        {
            environment.AddWizard<ExpressionConfig>();
            environment.AddWizard<ParametersConfig>();
            environment.AddWizard<FxLayerConfig>();
            environment.AddWizard<GestureLayerConfig>();
            environment.AddWizard<ActionLayerConfig>();
        }

        static void QuickSetupEmoteItems(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            QuickSetupExpressionSources(undoable, environment);
            QuickSetupParameterSources(undoable, environment);
            QuickSetupFXSources(undoable, environment);
            QuickSetupGestureSources(undoable, environment);
            QuickSetupActionSources(undoable, environment);
        }

        static void QuickSetupExpressionSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, Names.ExpressionSources);
        }

        static void QuickSetupParameterSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, Names.ParameterSources);
        }

        static void QuickSetupFXSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, Names.FXSources, fxSources =>
            {
                foreach (var fxItem in DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.FX))
                {
                    var fxSource = undoable.AddChildGameObject(fxSources, fxItem.Trigger.name);
                    fxItem.PopulateSources(undoable, fxSource.transform);
                }
            });
        }

        static void QuickSetupGestureSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            environment.OverrideGesture = OverrideGeneratedControllerType2.Generate;
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, Names.GestureSources, gestureSources =>
            {
                foreach (var gestureItem in DefaultEmoteItems.EnumerateDefaultHandSigns(LayerKind.Gesture))
                {
                    var gestureSource = undoable.FindOrCreateChildComponent<EmoteItemSource>(gestureSources, gestureItem.Trigger.name);
                    gestureItem.PopulateSources(undoable, gestureSource.transform);
                }
            });
        }

        static void QuickSetupActionSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            environment.OverrideAction = OverrideGeneratedControllerType1.Generate;
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, Names.ActionSources, actionSources =>
            {
                foreach (var actionItem in DefaultActionEmote.EnumerateDefaultActionEmoteItems())
                {
                    var actionSource = undoable.FindOrCreateChildComponent<EmoteItemSource>(actionSources, actionItem.Trigger.name);
                    actionItem.PopulateSources(undoable, actionSource.transform);
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