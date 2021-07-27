using System;
using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
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

        ExpandableReorderableList<AnimationMixin> baseMixinsList;
        ExpandableReorderableList<Emote> emotesList;
        ExpandableReorderableList<ParameterEmote> parametersList;
        ExpandableReorderableList<AnimationMixin> mixinsList;

        void OnEnable()
        {
            fxWizard = (FxWizard) target;

            baseMixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Base Mixins", ref fxWizard.baseMixins);
            emotesList = new ExpandableReorderableList<Emote>(new EmoteListHeaderDrawer(), new EmoteDrawer(), "Emotes", ref fxWizard.emotes);
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
                    if (GUILayout.Button("Repopulate Emotes: 7 items"))
                    {
                        SetupWizardUtils.RepopulateDefaultEmotes(fxWizard);
                    }

                    if (GUILayout.Button("Repopulate Emotes: 14 items"))
                    {
                        SetupWizardUtils.RepopulateDefaultEmotes14(fxWizard);
                    }

                    if (parametersWizard != null)
                    {
                        if (GUILayout.Button("Repopulate Parameters"))
                        {
                            SetupWizardUtils.RepopulateParameterEmotes(parametersWizard, fxWizard);
                        }
                    }
                });

                TypedGUILayout.Toggle("Advanced Animations", ref fxWizard.advancedAnimations);

                string relativePath = GeneratedAssetLocator.MixinDirectoryPath(fxWizard.LayerName);
                using (new AnimationMixinDrawerContext(emoteWizardRoot, relativePath).StartContext())
                {
                    baseMixinsList.DrawAsProperty(fxWizard.baseMixins, emoteWizardRoot.listDisplayMode);
                }

                using (new EmoteDrawerContext(emoteWizardRoot, parametersWizard, fxWizard.advancedAnimations).StartContext())
                {
                    emotesList.DrawAsProperty(fxWizard.emotes, emoteWizardRoot.listDisplayMode);
                }

                using (new ParameterEmoteDrawerContext(emoteWizardRoot, fxWizard, parametersWizard, fxWizard.LayerName).StartContext())
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

                string relativePath1 = GeneratedAssetLocator.MixinDirectoryPath(fxWizard.LayerName);
                using (new AnimationMixinDrawerContext(emoteWizardRoot, relativePath1).StartContext())
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

                        var resetLayer = builder.PopulateLayer("Reset");
                        builder.BuildStaticStateMachine(resetLayer.stateMachine, "Reset", resetClip);

                        foreach (var mixin in fxWizard.baseMixins.Where(mixin => mixin.Motion != null))
                        {
                            var mixinLayer = builder.PopulateLayer(mixin.name);
                            builder.BuildMixinLayerStateMachine(mixinLayer.stateMachine, mixin);
                        }

                        var leftHandLayer = builder.PopulateLayer("Left Hand", VrcSdkAssetLocator.HandLeft());
                        builder.BuildGestureStateMachine(leftHandLayer.stateMachine, true, fxWizard.advancedAnimations);

                        var rightHandLayer = builder.PopulateLayer("Right Hand", VrcSdkAssetLocator.HandRight());
                        builder.BuildGestureStateMachine(rightHandLayer.stateMachine, false, fxWizard.advancedAnimations);

                        foreach (var parameterEmote in fxWizard.ActiveParameters)
                        {
                            var expressionLayer = builder.PopulateLayer(parameterEmote.name);
                            builder.BuildParameterStateMachine(expressionLayer.stateMachine, parameterEmote);
                        }

                        foreach (var mixin in fxWizard.mixins.Where(mixin => mixin.Motion != null))
                        {
                            var mixinLayer = builder.PopulateLayer(mixin.name);
                            builder.BuildMixinLayerStateMachine(mixinLayer.stateMachine, mixin);
                        }

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