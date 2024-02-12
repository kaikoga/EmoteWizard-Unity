using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.UI
{
    public static class SetupGUI
    {
        static EmoteItemKind _emoteItemKind;
        static EmoteSequenceFactoryKind _emoteSequenceFactoryKindFx;

        public static bool OnInspectorGUI(EmoteWizardEnvironment env)
        {
            var result = false;

            _emoteItemKind = EmoteWizardGUILayout.EnumPopup(Loc("SetupGUI::emoteItemKind"), _emoteItemKind);
            _emoteSequenceFactoryKindFx = EmoteWizardGUILayout.EnumPopup(Loc("SetupGUI::emoteSequenceFactoryKindFx"), _emoteSequenceFactoryKindFx);

            EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Quick Setup Default Data Sources"), undoable =>
            {
                QuickSetupDefaultSources(undoable, env);
                result = true;
            });

            EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Generate Configs"), undoable =>
            {
                GenerateConfigs(undoable, env);
                result = true;
            });

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
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(fxSources, "Default FX Items Wizard");
                wizard.layerKind = LayerKind.FX;
                wizard.emoteItemKind = _emoteItemKind;
                wizard.emoteSequenceFactoryKind = _emoteSequenceFactoryKindFx;
                wizard.Explode(undoable, false);
            });
        }

        static void QuickSetupGestureSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            environment.OverrideGesture = OverrideGeneratedControllerType2.Generate;
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Gesture Sources", gestureSources =>
            {
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(gestureSources, "Default Gesture Items Wizard");
                wizard.layerKind = LayerKind.Gesture;
                wizard.emoteItemKind = _emoteItemKind;
                wizard.emoteSequenceFactoryKind = EmoteSequenceFactoryKind.EmoteSequence;
                wizard.Explode(undoable, false);
            });
        }

        static void QuickSetupActionSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            environment.OverrideAction = OverrideGeneratedControllerType1.Generate;
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Action Sources", actionSources =>
            {
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(actionSources, "Default Action Items Wizard");
                wizard.layerKind = LayerKind.Action;
                wizard.emoteItemKind = EmoteItemKind.EmoteItem;
                wizard.emoteSequenceFactoryKind = EmoteSequenceFactoryKind.EmoteSequence;
                wizard.Explode(undoable, false);
            });
        }
    }
}