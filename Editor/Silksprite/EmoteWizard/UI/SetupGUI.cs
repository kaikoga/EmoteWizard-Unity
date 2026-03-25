using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch.IMGUI;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.UI
{
    public static class SetupGUI
    {
        static EmoteItemKind _emoteItemKind;
        static EmoteSequenceFactoryKind _emoteSequenceFactoryKindFx;
        static bool _unpack = true;

        public static bool OnInspectorGUI(EmoteWizardEnvironment env)
        {
            var result = false;

            if (env.MaybeUnityPlatforms())
            {
                _emoteItemKind = LEditorGUILayout.EnumPopup(Loc("SetupGUI::emoteItemKind"), _emoteItemKind);
                _emoteSequenceFactoryKindFx = LEditorGUILayout.EnumPopup(Loc("SetupGUI::emoteSequenceFactoryKindFx"), _emoteSequenceFactoryKindFx);
                _unpack = LEditorGUILayout.Toggle(Loc("SetupGUI::unpack"), _unpack);
            }

            var isMultiPlatform = SupportedPlatform.IsMultiple;
            var singleLoc = Loc("SetupGUI::Quick Setup Default Data Sources");

            if (env.MaybeVRCPlatforms())
            {
                var loc = isMultiPlatform ? Loc("SetupGUI::Quick Setup VRChat Sources") : singleLoc;
                EmoteWizardGUILayout.Undoable(loc, undoable =>
                {
                    QuickSetupDefaultVrcPlatformSources(env, undoable);
                    result = true;
                });
            }

            if (env.MaybeVRM())
            {
                var loc = isMultiPlatform ? Loc("SetupGUI::Quick Setup VRM Sources") : singleLoc;
                EmoteWizardGUILayout.Undoable(loc, undoable =>
                {
                    QuickSetupDefaultVrmSources(env, undoable);
                    result = true;
                });
            }

            if (env.MaybeVRChat())
            {
                EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Generate Configs"), undoable =>
                {
                    GenerateConfigsForVRChat(env, undoable);
                    result = true;
                });
            }

            if (env.MaybeChilloutVR())
            {
                EmoteWizardGUILayout.Undoable(Loc("SetupGUI::Generate Configs"), undoable =>
                {
                    GenerateConfigsForChilloutVR(env, undoable);
                    result = true;
                });
            }

            return result;
        }

        static void QuickSetupDefaultVrcPlatformSources(EmoteWizardEnvironment environment, IUndoable undoable)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Expression Sources");
            
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Parameter Sources");
            
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "FX Sources", fxSources =>
            {
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(fxSources, "Default FX Items Wizard");
                wizard.defaultSourceKind = DefaultSourceKind.Fx;
                wizard.emoteItemKind = _emoteItemKind;
                wizard.emoteSequenceFactoryKind = _emoteSequenceFactoryKindFx;
                wizard.unpack = _unpack;
                wizard.Explode(environment, undoable, false);
            });

            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Gesture Sources", gestureSources =>
            {
                environment.OverrideGesture = OverrideGeneratedControllerType2.Generate;
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(gestureSources, "Default Gesture Items Wizard");
                wizard.defaultSourceKind = DefaultSourceKind.Gesture;
                wizard.emoteItemKind = _emoteItemKind;
                wizard.emoteSequenceFactoryKind = EmoteSequenceFactoryKind.EmoteSequence;
                wizard.unpack = _unpack;
                wizard.Explode(environment, undoable, false);
            });

            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "Action Sources", actionSources =>
            {
                environment.OverrideAction = OverrideGeneratedControllerType1.Generate;
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(actionSources, "Default Action Items Wizard");
                wizard.defaultSourceKind = DefaultSourceKind.Action;
                wizard.emoteItemKind = EmoteItemKind.EmoteItem;
                wizard.emoteSequenceFactoryKind = EmoteSequenceFactoryKind.EmoteSequence;
                wizard.unpack = _unpack;
                wizard.Explode(environment, undoable, false);
            });
        }

        static void QuickSetupDefaultVrmSources(EmoteWizardEnvironment environment, IUndoable undoable)
        {
            undoable.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(environment, "BlendShape Sources", bsSources =>
            {
                var wizard = undoable.AddChildComponent<DefaultSourcesWizard>(bsSources, "Default BlendShape Items Wizard");
                wizard.defaultSourceKind = DefaultSourceKind.Vrm;
                wizard.emoteItemKind = EmoteItemKind.GenericEmoteItem;
                wizard.emoteSequenceFactoryKind = EmoteSequenceFactoryKind.GenericEmoteSequence;
                wizard.Explode(environment, undoable, false);
            });
        }

        static void GenerateConfigsForVRChat(EmoteWizardEnvironment environment, IUndoable undoable)
        {
            undoable.AddWizard<EditorLayerConfig>(environment);
            undoable.AddWizard<ExpressionConfig>(environment);
            undoable.AddWizard<ParametersConfig>(environment);
            undoable.AddWizard<FxLayerConfig>(environment);
            undoable.AddWizard<GestureLayerConfig>(environment);
            undoable.AddWizard<ActionLayerConfig>(environment);
        }

        static void GenerateConfigsForChilloutVR(EmoteWizardEnvironment environment, IUndoable undoable)
        {
            undoable.AddWizard<EditorLayerConfig>(environment);
            undoable.AddWizard<MergedLayerConfig>(environment);
            undoable.AddWizard<OverrideControllerConfig>(environment);
        }
    }
}