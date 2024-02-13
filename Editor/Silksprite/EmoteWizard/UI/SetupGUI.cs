using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
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

#if EW_VRCSDK3_AVATARS
            _emoteItemKind = EmoteWizardGUILayout.EnumPopup(Loc("SetupGUI::emoteItemKind"), _emoteItemKind);
            _emoteSequenceFactoryKindFx = EmoteWizardGUILayout.EnumPopup(Loc("SetupGUI::emoteSequenceFactoryKindFx"), _emoteSequenceFactoryKindFx);
#endif

            if (EmoteWizardConstants.SupportedPlatforms.IsMultiple)
            {
#if EW_VRCSDK3_AVATARS
                EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Quick Setup VRChat Sources"), undoable =>
                {
                    QuickSetupDefaultVRChatSources(undoable, env);
                    result = true;
                });
#endif

#if EW_VRM0 || EW_VRM1
                EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Quick Setup VRM Sources"), undoable =>
                {
                    QuickSetupDefaultVrmSources(undoable, env);
                    result = true;
                });
#endif
            }
            else
            {
                EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Quick Setup Default Data Sources"), undoable =>
                {
#if EW_VRCSDK3_AVATARS
                    QuickSetupDefaultVRChatSources(undoable, env);
#endif
#if EW_VRM0 || EW_VRM1
                    QuickSetupDefaultVrmSources(undoable, env);
#endif
                    result = true;
                });
            }

#if EW_VRCSDK3_AVATARS
            EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Generate Configs"), undoable =>
            {
                GenerateConfigs(undoable, env);
                result = true;
            });
#endif

            return result;
        }

        static void QuickSetupDefaultVRChatSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Expression Sources");
            
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Parameter Sources");
            
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "FX Sources", fxSources =>
            {
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(fxSources, "Default FX Items Wizard");
                wizard.defaultSourceKind = DefaultSourceKind.Fx;
                wizard.emoteItemKind = _emoteItemKind;
                wizard.emoteSequenceFactoryKind = _emoteSequenceFactoryKindFx;
                wizard.Explode(undoable, false);
            });

            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Gesture Sources", gestureSources =>
            {
                environment.OverrideGesture = OverrideGeneratedControllerType2.Generate;
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(gestureSources, "Default Gesture Items Wizard");
                wizard.defaultSourceKind = DefaultSourceKind.Gesture;
                wizard.emoteItemKind = _emoteItemKind;
                wizard.emoteSequenceFactoryKind = EmoteSequenceFactoryKind.EmoteSequence;
                wizard.Explode(undoable, false);
            });

            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Action Sources", actionSources =>
            {
                environment.OverrideAction = OverrideGeneratedControllerType1.Generate;
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(actionSources, "Default Action Items Wizard");
                wizard.defaultSourceKind = DefaultSourceKind.Action;
                wizard.emoteItemKind = EmoteItemKind.EmoteItem;
                wizard.emoteSequenceFactoryKind = EmoteSequenceFactoryKind.EmoteSequence;
                wizard.Explode(undoable, false);
            });
        }

        static void QuickSetupDefaultVrmSources(IUndoable undoable, EmoteWizardEnvironment environment)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "BlendShape Sources", bsSources =>
            {
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(bsSources, "Default BlendShape Items Wizard");
                wizard.defaultSourceKind = DefaultSourceKind.Vrm;
                wizard.emoteItemKind = EmoteItemKind.GenericEmoteItem;
                wizard.emoteSequenceFactoryKind = EmoteSequenceFactoryKind.GenericEmoteSequence;
                wizard.Explode(undoable, false);
            });
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
    }
}