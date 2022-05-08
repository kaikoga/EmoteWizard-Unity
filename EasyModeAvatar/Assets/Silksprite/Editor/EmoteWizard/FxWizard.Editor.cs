using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
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

            baseMixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Base Mixins", ref fxWizard.legacyBaseMixins);
            emotesList = new ExpandableReorderableList<Emote>(new EmoteListHeaderDrawer(), new EmoteDrawer(), "HandSign Emotes", ref fxWizard.legacyEmotes);
            parametersList = new ExpandableReorderableList<ParameterEmote>(new ParameterEmoteListHeaderDrawer(), new ParameterEmoteDrawer(), "Parameter Emotes", ref fxWizard.legacyParameterEmotes);
            mixinsList = new ExpandableReorderableList<AnimationMixin>(new AnimationMixinListHeaderDrawer(), new AnimationMixinDrawer(), "Mixins", ref fxWizard.legacyMixins);
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(fxWizard))
            {
                var emoteWizardRoot = fxWizard.EmoteWizardRoot;
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

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
                    baseMixinsList.DrawAsProperty(fxWizard.legacyBaseMixins, emoteWizardRoot.listDisplayMode);
                }

                using (new EmoteDrawerContext(emoteWizardRoot, parametersWizard, fxWizard.LayerName, fxWizard.advancedAnimations, emotesState).StartContext())
                {
                    emotesList.DrawAsProperty(fxWizard.legacyEmotes, emoteWizardRoot.listDisplayMode);
                }

                using (new ParameterEmoteDrawerContext(emoteWizardRoot, fxWizard, parametersWizard, fxWizard.LayerName, parametersState).StartContext())
                {
                    parametersList.DrawAsProperty(fxWizard.legacyParameterEmotes, emoteWizardRoot.listDisplayMode);
                }

                if (IsExpandedTracker.GetIsExpanded(fxWizard.legacyParameterEmotes))
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
                    mixinsList.DrawAsProperty(fxWizard.legacyMixins, emoteWizardRoot.listDisplayMode);
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    EmoteWizardGUILayout.RequireAnotherWizard<AvatarWizard>(fxWizard, () =>
                    {
                        if (GUILayout.Button("Generate Animation Controller"))
                        {
                            fxWizard.BuildOutputAsset(parametersWizard);
                        }
                    });

                    TypedGUILayout.AssetField("Output Asset", ref fxWizard.outputAsset);
                    TypedGUILayout.AssetField("Reset Clip", ref fxWizard.resetClip);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"FX Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
            }
        }
    }
}