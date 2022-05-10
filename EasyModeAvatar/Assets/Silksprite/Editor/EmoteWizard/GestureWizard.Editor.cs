using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

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

            baseMixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Base Mixins", ref gestureWizard.legacyBaseMixins);
            emotesList = new ExpandableReorderableList<Emote>(new EmoteListHeaderDrawer(), new EmoteDrawer(), "HandSign Emotes", ref gestureWizard.legacyEmotes);
            parametersList = new ExpandableReorderableList<ParameterEmote>(new ParameterEmoteListHeaderDrawer(), new ParameterEmoteDrawer(), "Parameter Emotes", ref gestureWizard.legacyParameterEmotes);
            mixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Mixins", ref gestureWizard.legacyMixins);
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(gestureWizard))
            {
                var emoteWizardRoot = gestureWizard.EmoteWizardRoot;
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

                EmoteWizardGUILayout.SetupOnlyUI(gestureWizard, () =>
                {
                    if (GUILayout.Button("Repopulate HandSigns"))
                    {
                        gestureWizard.RepopulateDefaultEmotes();
                    }

                    if (parametersWizard != null)
                    {
                        if (GUILayout.Button("Repopulate Parameters"))
                        {
                            gestureWizard.RepopulateParameterEmotes(parametersWizard);
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

                if (gestureWizard.HasLegacyData)
                {
                    var relativePath = GeneratedAssetLocator.MixinDirectoryPath(gestureWizard.LayerName);
                    using (new AnimationMixinDrawerContext(emoteWizardRoot, parametersWizard, relativePath, baseMixinsState).StartContext())
                    {
                        baseMixinsList.DrawAsProperty(gestureWizard.legacyBaseMixins, emoteWizardRoot.listDisplayMode);
                    }

                    using (new EmoteDrawerContext(emoteWizardRoot, parametersWizard, gestureWizard.LayerName, gestureWizard.advancedAnimations, emotesState).StartContext())
                    {
                        emotesList.DrawAsProperty(gestureWizard.legacyEmotes, emoteWizardRoot.listDisplayMode);
                    }

                    using (new ParameterEmoteDrawerContext(emoteWizardRoot, gestureWizard, parametersWizard, gestureWizard.LayerName, parametersState).StartContext())
                    {
                        parametersList.DrawAsProperty(gestureWizard.legacyParameterEmotes, emoteWizardRoot.listDisplayMode);
                    }

                    if (IsExpandedTracker.GetIsExpanded(gestureWizard.legacyParameterEmotes))
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
                        mixinsList.DrawAsProperty(gestureWizard.legacyMixins, emoteWizardRoot.listDisplayMode);
                    }

                    if (GUILayout.Button("Migrate to Data Source"))
                    {
                        MigrateToDataSource();
                    }
                }
                if (GUILayout.Button("Add Emote Source"))
                {
                    gestureWizard.AddChildComponentAndSelect<GestureEmoteSource>();
                }
                if (GUILayout.Button("Add Parameter Emote Source"))
                {
                    gestureWizard.AddChildComponentAndSelect<GestureParameterEmoteSource>();
                }
                if (GUILayout.Button("Add Animation Mixin Source"))
                {
                    gestureWizard.AddChildComponentAndSelect<GestureAnimationMixinSource>();
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    EmoteWizardGUILayout.RequireAnotherWizard<AvatarWizard>(gestureWizard, () =>
                    {
                        if (GUILayout.Button("Generate Animation Controller"))
                        {
                            gestureWizard.BuildOutputAsset(parametersWizard);
                        }
                    });

                    TypedGUILayout.AssetField("Output Asset", ref gestureWizard.outputAsset);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"Gesture Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
            }
        }

        void MigrateToDataSource()
        {
            var emoteSource = gestureWizard.AddChildComponent<GestureEmoteSource>();
            emoteSource.emotes = gestureWizard.legacyEmotes.ToList();
            gestureWizard.legacyEmotes.Clear();
            
            var parameterEmoteSource = gestureWizard.AddChildComponent<GestureParameterEmoteSource>();
            parameterEmoteSource.parameterEmotes = gestureWizard.legacyParameterEmotes.ToList();
            gestureWizard.legacyParameterEmotes.Clear();
            
            var mixinSource = gestureWizard.AddChildComponent<GestureAnimationMixinSource>();
            mixinSource.baseMixins = gestureWizard.legacyBaseMixins.ToList();
            mixinSource.mixins = gestureWizard.legacyMixins.ToList();
            gestureWizard.legacyBaseMixins.Clear();
            gestureWizard.legacyMixins.Clear();
        }
    }
}