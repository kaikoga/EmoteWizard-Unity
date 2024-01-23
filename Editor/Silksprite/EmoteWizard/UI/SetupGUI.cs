using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.UI
{
    public static class SetupGUI
    {
        public static bool OnInspectorGUI(EmoteWizardEnvironment env)
        {
            var result = false;

            Action<IUndoable> onClick = undoable =>
            {
                QuickSetupDefaultSources(undoable, env);
                result = true;
            };
            EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Quick Setup Default Data Sources"), onClick);

            Action<IUndoable> onClick1 = undoable =>
            {
                GenerateConfigs(undoable, env);
                result = true;
            };
            EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Generate Configs"), onClick1);

            return result;
        }

        static void QuickSetupDefaultSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            QuickSetupExpressionSources(undoable, environment);
            QuickSetupParameterSources(undoable, environment);
            QuickSetupFXSources(undoable, environment);
            QuickSetupGestureSources(undoable, environment);
            QuickSetupActionSources(undoable, environment);
        }

        static void GenerateConfigs(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            undoable.AddWizard<EditorLayerConfig>(environment);
            undoable.AddWizard<ExpressionConfig>(environment);
            undoable.AddWizard<ParametersConfig>(environment);
            undoable.AddWizard<FxLayerConfig>(environment);
            undoable.AddWizard<GestureLayerConfig>(environment);
            undoable.AddWizard<ActionLayerConfig>(environment);
        }

        static void QuickSetupExpressionSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Expression Sources");
        }

        static void QuickSetupParameterSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Parameter Sources");
        }

        static void QuickSetupFXSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "FX Sources", fxSources =>
            {
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(fxSources, "Default FX Items Wizard", wizard => wizard.layerKind = LayerKind.FX);
                WizardExploder.ExplodeImmediate(undoable, wizard, false);
            });
        }

        static void QuickSetupGestureSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            environment.OverrideGesture = OverrideGeneratedControllerType2.Generate;
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Gesture Sources", gestureSources =>
            {
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(gestureSources, "Default Gesture Items Wizard", wizard => wizard.layerKind = LayerKind.Gesture);
                WizardExploder.ExplodeImmediate(undoable, wizard, false);
            });
        }

        static void QuickSetupActionSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            environment.OverrideAction = OverrideGeneratedControllerType1.Generate;
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Action Sources", actionSources =>
            {
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(actionSources, "Default Action Items Wizard", wizard => wizard.layerKind = LayerKind.Action);
                WizardExploder.ExplodeImmediate(undoable, wizard, false);
            });
        }
    }
}