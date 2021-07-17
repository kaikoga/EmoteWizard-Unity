using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
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
    [CustomEditor(typeof(GestureWizard))]
    public class GestureWizardEditor : AnimationWizardBaseEditor
    {
        GestureWizard gestureWizard;

        ExpandableReorderableList<AnimationMixin> baseMixinsList;
        ExpandableReorderableList<Emote> emotesList;
        ExpandableReorderableList<ParameterEmote> parametersList;
        ExpandableReorderableList<AnimationMixin> mixinsList;

        void OnEnable()
        {
            gestureWizard = target as GestureWizard;
            
            baseMixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListDrawer(), new AnimationMixinDrawer(), "Base Mixins", gestureWizard.baseMixins);
            emotesList = new ExpandableReorderableList<Emote>(new EmoteListDrawer(), new EmoteDrawer(), "Emotes", gestureWizard.emotes);
            parametersList = new ExpandableReorderableList<ParameterEmote>(new ParameterEmoteListDrawer(), new ParameterEmoteDrawer(), "Parameter Emotes", gestureWizard.parameterEmotes);
            mixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListDrawer(), new AnimationMixinDrawer(), "Mixins", gestureWizard.mixins);
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(gestureWizard))
            {
                var emoteWizardRoot = gestureWizard.EmoteWizardRoot;
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                EmoteWizardGUILayout.SetupOnlyUI(gestureWizard, () =>
                {
                    if (GUILayout.Button("Repopulate Emotes"))
                    {
                        RepopulateDefaultGestureEmotes(gestureWizard);
                    }

                    if (parametersWizard != null)
                    {
                        if (GUILayout.Button("Repopulate Parameters"))
                        {
                            parametersWizard.TryRefreshParameters();
                            gestureWizard.parameterEmotes = new List<ParameterEmote>();
                            gestureWizard.RefreshParameters(parametersWizard);
                        }
                    }
                });

                TypedGUILayout.Toggle("Advanced Animations", ref gestureWizard.advancedAnimations);

                CustomTypedGUILayout.AssetFieldWithGenerate("Default Avatar Mask", ref gestureWizard.defaultAvatarMask, () =>
                {
                    var avatarMask = emoteWizardRoot.EnsureAsset<AvatarMask>("Gesture/@@@Generated@@@GestureDefaultMask.mask");
                    return AvatarMaskUtils.SetupAsGestureDefault(avatarMask);
                });

                using (AnimationMixinDrawer.StartContext(emoteWizardRoot, GeneratedAssetLocator.MixinDirectoryPath(gestureWizard.LayerName)))
                {
                    baseMixinsList.DrawAsProperty(gestureWizard.baseMixins, emoteWizardRoot.listDisplayMode);
                }

                using (EmoteDrawer.StartContext(emoteWizardRoot, parametersWizard, gestureWizard.advancedAnimations))
                {
                    emotesList.DrawAsProperty(gestureWizard.emotes, emoteWizardRoot.listDisplayMode);
                }

                using (ParameterEmoteDrawer.StartContext(emoteWizardRoot, gestureWizard, parametersWizard, gestureWizard.LayerName, ParameterEmoteDrawer.EditTargets))
                {
                    parametersList.DrawAsProperty(gestureWizard.parameterEmotes, emoteWizardRoot.listDisplayMode);
                }

                if (IsExpandedTracker.GetIsExpanded(gestureWizard.parameterEmotes))
                {
                    EmoteWizardGUILayout.RequireAnotherWizard(gestureWizard, parametersWizard, () =>
                    {
                        if (GUILayout.Button("Collect Parameters"))
                        {
                            parametersWizard.TryRefreshParameters();
                            gestureWizard.RefreshParameters(parametersWizard);
                        }
                    });
                }

                using (AnimationMixinDrawer.StartContext(emoteWizardRoot, GeneratedAssetLocator.MixinDirectoryPath(gestureWizard.LayerName)))
                {
                    mixinsList.DrawAsProperty(gestureWizard.mixins, emoteWizardRoot.listDisplayMode);
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Animation Controller"))
                    {
                        var builder = new AnimationControllerBuilder
                        {
                            AnimationWizardBase = gestureWizard,
                            ParametersWizard = parametersWizard,
                            DefaultRelativePath = "Gesture/@@@Generated@@@Gesture.controller"
                        };

                        var resetLayer = builder.PopulateLayer("Reset", gestureWizard.defaultAvatarMask ? gestureWizard.defaultAvatarMask : VrcSdkAssetLocator.HandsOnly());
                        builder.BuildStaticStateMachine(resetLayer.stateMachine, "Reset", null);

                        foreach (var mixin in gestureWizard.baseMixins.Where(mixin => mixin.Motion != null))
                        {
                            var mixinLayer = builder.PopulateLayer(mixin.name);
                            builder.BuildMixinLayerStateMachine(mixinLayer.stateMachine, mixin);
                        }

                        var leftHandLayer = builder.PopulateLayer("Left Hand", VrcSdkAssetLocator.HandLeft());
                        builder.BuildGestureStateMachine(leftHandLayer.stateMachine, true, gestureWizard.advancedAnimations);

                        var rightHandLayer = builder.PopulateLayer("Right Hand", VrcSdkAssetLocator.HandRight());
                        builder.BuildGestureStateMachine(rightHandLayer.stateMachine, false, gestureWizard.advancedAnimations);

                        foreach (var parameterEmote in gestureWizard.ActiveParameters)
                        {
                            var expressionLayer = builder.PopulateLayer(parameterEmote.name);
                            builder.BuildParameterStateMachine(expressionLayer.stateMachine, parameterEmote);
                        }

                        foreach (var mixin in gestureWizard.mixins.Where(mixin => mixin.Motion != null))
                        {
                            var mixinLayer = builder.PopulateLayer(mixin.name);
                            builder.BuildMixinLayerStateMachine(mixinLayer.stateMachine, mixin);
                        }

                        builder.BuildParameters();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref gestureWizard.outputAsset);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"Gesture Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
            }
        }

        static void RepopulateDefaultGestureEmotes(AnimationWizardBase wizard)
        {
            var newEmotes = Emote.HandSigns
                .Select(Emote.Populate)
                .ToList();
            wizard.emotes = newEmotes;
        }
    }
}