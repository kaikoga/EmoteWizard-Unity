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
    [CustomEditor(typeof(GestureWizard))]
    public class GestureWizardEditor : AnimationWizardBaseEditor
    {
        GestureWizard gestureWizard;

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
            gestureWizard = (GestureWizard) target;
            
            baseMixinsState = new AnimationMixinDrawerState();
            emotesState = new EmoteDrawerState();
            parametersState = new ParameterEmoteDrawerState();
            mixinsState = new AnimationMixinDrawerState();

            baseMixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Base Mixins", ref gestureWizard.baseMixins);
            emotesList = new ExpandableReorderableList<Emote>(new EmoteListHeaderDrawer(), new EmoteDrawer(), "HandSign Emotes", ref gestureWizard.emotes);
            parametersList = new ExpandableReorderableList<ParameterEmote>(new ParameterEmoteListHeaderDrawer(), new ParameterEmoteDrawer(), "Parameter Emotes", ref gestureWizard.parameterEmotes);
            mixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Mixins", ref gestureWizard.mixins);
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(gestureWizard))
            {
                var emoteWizardRoot = gestureWizard.EmoteWizardRoot;
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                EmoteWizardGUILayout.SetupOnlyUI(gestureWizard, () =>
                {
                    if (GUILayout.Button("Repopulate HandSigns"))
                    {
                        SetupWizardUtils.RepopulateDefaultEmotes(gestureWizard);
                    }

                    if (parametersWizard != null)
                    {
                        if (GUILayout.Button("Repopulate Parameters"))
                        {
                            SetupWizardUtils.RepopulateParameterEmotes(parametersWizard, gestureWizard);
                        }
                    }
                });

                TypedGUILayout.Toggle("Advanced Animations", ref gestureWizard.advancedAnimations);
                TypedGUILayout.Toggle("HandSign Override", ref gestureWizard.handSignOverrideEnabled);
                if (gestureWizard.handSignOverrideEnabled)
                {
                    using (new InvalidValueScope(parametersWizard.IsInvalidParameter(gestureWizard.handSignOverrideParameter)))
                    {
                        TypedGUILayout.TextField("HandSign Override Parameter", ref gestureWizard.handSignOverrideParameter);
                    }
                }
                else
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        TypedGUILayout.TextField("HandSign Override Parameter", ref gestureWizard.handSignOverrideParameter);
                    }
                }

                CustomTypedGUILayout.AssetFieldWithGenerate("Default Avatar Mask", ref gestureWizard.defaultAvatarMask, () =>
                {
                    var avatarMask = emoteWizardRoot.EnsureAsset<AvatarMask>("Gesture/@@@Generated@@@GestureDefaultMask.mask");
                    return AvatarMaskUtils.SetupAsGestureDefault(avatarMask);
                });

                string relativePath = GeneratedAssetLocator.MixinDirectoryPath(gestureWizard.LayerName);
                using (new AnimationMixinDrawerContext(emoteWizardRoot, parametersWizard, relativePath, baseMixinsState).StartContext())
                {
                    baseMixinsList.DrawAsProperty(gestureWizard.baseMixins, emoteWizardRoot.listDisplayMode);
                }

                using (new EmoteDrawerContext(emoteWizardRoot, parametersWizard, gestureWizard.advancedAnimations, emotesState).StartContext())
                {
                    emotesList.DrawAsProperty(gestureWizard.emotes, emoteWizardRoot.listDisplayMode);
                }

                using (new ParameterEmoteDrawerContext(emoteWizardRoot, gestureWizard, parametersWizard, gestureWizard.LayerName, parametersState).StartContext())
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

                using (new AnimationMixinDrawerContext(emoteWizardRoot, parametersWizard, relativePath, mixinsState).StartContext())
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

                        var defaultAvatarMask = gestureWizard.defaultAvatarMask ? gestureWizard.defaultAvatarMask : VrcSdkAssetLocator.HandsOnly();

                        builder.BuildStaticLayer("Reset", null, defaultAvatarMask);
                        builder.BuildMixinLayers(gestureWizard.baseMixins);
                        builder.BuildHandSignLayer("Left Hand", true, gestureWizard.advancedAnimations);
                        builder.BuildHandSignLayer("Right Hand", false, gestureWizard.advancedAnimations);
                        builder.BuildParameterLayers(gestureWizard.ActiveParameters);
                        builder.BuildMixinLayers(gestureWizard.mixins);

                        builder.BuildTrackingControlLayers();
                        builder.BuildParameters();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref gestureWizard.outputAsset);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"Gesture Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
            }
        }
    }
}