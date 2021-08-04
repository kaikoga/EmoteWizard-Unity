using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : AnimationWizardBaseEditor
    {
        FxWizard fxWizard;

        AnimationMixinDrawerState baseMixinsState;
        EmoteDrawerState emotesState;
        ParameterEmoteDrawerState parametersState;
        AnimationMixinDrawerState mixinsState;

        ExpandableReorderableList<AnimationMixin> baseMixinsList;
        ExpandableReorderableList<Emote> emotesList;
        ExpandableReorderableList<ParameterEmote> parametersList;
        ExpandableReorderableList<AnimationMixin> mixinsList;

        void OnEnable()
        {
            fxWizard = (FxWizard) target;

            baseMixinsState = new AnimationMixinDrawerState();
            emotesState = new EmoteDrawerState();
            parametersState = new ParameterEmoteDrawerState();
            mixinsState = new AnimationMixinDrawerState();

            baseMixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Base Mixins", ref fxWizard.baseMixins);
            emotesList = new ExpandableReorderableList<Emote>(new EmoteListHeaderDrawer(), new EmoteDrawer(), "HandSign Emotes", ref fxWizard.emotes);
            parametersList = new ExpandableReorderableList<ParameterEmote>(new ParameterEmoteListHeaderDrawer(), new ParameterEmoteDrawer(), "Parameter Emotes", ref fxWizard.parameterEmotes);
            mixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Mixins", ref fxWizard.mixins);
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(fxWizard))
            {
                var emoteWizardRoot = fxWizard.EmoteWizardRoot;
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                EmoteWizardGUILayout.SetupOnlyUI(fxWizard, () =>
                {
                    if (GUILayout.Button("Repopulate HandSigns: 7 items"))
                    {
                        fxWizard.RepopulateDefaultEmotes();
                    }

                    if (GUILayout.Button("Repopulate HandSigns: 14 items"))
                    {
                        fxWizard.RepopulateDefaultEmotes14();
                    }

                    if (parametersWizard != null)
                    {
                        if (GUILayout.Button("Repopulate Parameters"))
                        {
                            fxWizard.RepopulateParameterEmotes(parametersWizard);
                        }
                    }
                });

                TypedGUILayout.Toggle("Advanced Animations", ref fxWizard.advancedAnimations);
                TypedGUILayout.Toggle("HandSign Override", ref fxWizard.handSignOverrideEnabled);
                if (fxWizard.handSignOverrideEnabled)
                {
                    using (new InvalidValueScope(parametersWizard.IsInvalidParameter(fxWizard.handSignOverrideParameter)))
                    {
                        TypedGUILayout.TextField("HandSign Override Parameter", ref fxWizard.handSignOverrideParameter);
                    }
                }
                else
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        TypedGUILayout.TextField("HandSign Override Parameter", ref fxWizard.handSignOverrideParameter);
                    }
                }

                string relativePath = GeneratedAssetLocator.MixinDirectoryPath(fxWizard.LayerName);
                using (new AnimationMixinDrawerContext(emoteWizardRoot, parametersWizard, relativePath, baseMixinsState).StartContext())
                {
                    baseMixinsList.DrawAsProperty(fxWizard.baseMixins, emoteWizardRoot.listDisplayMode);
                }

                using (new EmoteDrawerContext(emoteWizardRoot, parametersWizard, fxWizard.advancedAnimations, emotesState).StartContext())
                {
                    emotesList.DrawAsProperty(fxWizard.emotes, emoteWizardRoot.listDisplayMode);
                }

                using (new ParameterEmoteDrawerContext(emoteWizardRoot, fxWizard, parametersWizard, fxWizard.LayerName, parametersState).StartContext())
                {
                    parametersList.DrawAsProperty(fxWizard.parameterEmotes, emoteWizardRoot.listDisplayMode);
                }

                if (IsExpandedTracker.GetIsExpanded(fxWizard.parameterEmotes))
                {
                    EmoteWizardGUILayout.RequireAnotherWizard(fxWizard, parametersWizard, () =>
                    {
                        if (GUILayout.Button("Collect Parameters"))
                        {
                            parametersWizard.TryRefreshParameters();
                            fxWizard.RefreshParameters(parametersWizard);
                        }
                    });
                }

                using (new AnimationMixinDrawerContext(emoteWizardRoot, parametersWizard, relativePath, mixinsState).StartContext())
                {
                    mixinsList.DrawAsProperty(fxWizard.mixins, emoteWizardRoot.listDisplayMode);
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Animation Controller"))
                    {
                        var builder = new AnimationControllerBuilder
                        {
                            AnimationWizardBase = fxWizard,
                            ParametersWizard = parametersWizard,
                            DefaultRelativePath = "FX/@@@Generated@@@FX.controller"
                        };

                        var resetClip = BuildResetClip(fxWizard.ProvideResetClip());

                        builder.BuildStaticLayer("Reset", resetClip, null);
                        builder.BuildMixinLayers(fxWizard.baseMixins);
                        builder.BuildHandSignLayer("Left Hand", true, fxWizard.advancedAnimations);
                        builder.BuildHandSignLayer("Right Hand", false, fxWizard.advancedAnimations);
                        builder.BuildParameterLayers(fxWizard.ActiveParameters);
                        builder.BuildMixinLayers(fxWizard.mixins);

                        builder.BuildTrackingControlLayers();
                        builder.BuildParameters();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref fxWizard.outputAsset);
                    TypedGUILayout.AssetField("Reset Clip", ref fxWizard.resetClip);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"FX Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
            }
        }
    }
}