using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : AnimationWizardBaseEditor
    {
        FxWizard fxWizard;

        ExpandableReorderableList baseMixinsList;
        ExpandableReorderableList emotesList;
        ExpandableReorderableList parametersList;
        ExpandableReorderableList mixinsList;

        void OnEnable()
        {
            fxWizard = target as FxWizard;

            baseMixinsList = new ExpandableReorderableList(new AnimationMixinListDrawerBase(), serializedObject.FindProperty("baseMixins"));
            emotesList = new ExpandableReorderableList(new EmoteListDrawerBase(), serializedObject.FindProperty("emotes"));
            parametersList = new ExpandableReorderableList(new ParameterEmoteListDrawerBase(), serializedObject.FindProperty("parameterEmotes"));
            mixinsList = new ExpandableReorderableList(new AnimationMixinListDrawerBase(), serializedObject.FindProperty("mixins"));
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = fxWizard.EmoteWizardRoot;
            var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

            EmoteWizardGUILayout.SetupOnlyUI(fxWizard, () =>
            {
                if (GUILayout.Button("Repopulate Emotes: 7 items"))
                {
                    RepopulateDefaultFxEmotes();
                }
                if (GUILayout.Button("Repopulate Emotes: 14 items"))
                {
                    RepopulateDefaultFxEmotes14();
                }
                if (parametersWizard != null)
                {
                    if (GUILayout.Button("Repopulate Parameters"))
                    {
                        parametersWizard.TryRefreshParameters();
                        fxWizard.parameterEmotes = new List<ParameterEmote>();
                        fxWizard.RefreshParameters(parametersWizard);
                    }
                }
            });

            var advancedAnimations = serializedObj.FindProperty("advancedAnimations");
            EditorGUILayout.PropertyField(advancedAnimations);

            using (AnimationMixinDrawer.StartContext(emoteWizardRoot, GeneratedAssetLocator.MixinDirectoryPath(fxWizard.LayerName)))
            {
                baseMixinsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }

            using (EmoteDrawer.StartContext(emoteWizardRoot, parametersWizard, advancedAnimations.boolValue))
            {
                emotesList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }

            using (ParameterEmoteDrawer.StartContext(emoteWizardRoot, fxWizard, parametersWizard, fxWizard.LayerName, ParameterEmoteDrawer.EditTargets))
            {
                parametersList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }
            if (parametersList.serializedProperty.isExpanded)
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

            using (AnimationMixinDrawer.StartContext(emoteWizardRoot, GeneratedAssetLocator.MixinDirectoryPath(fxWizard.LayerName)))
            {
                mixinsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
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
                    builder.BuildGestureStateMachine(leftHandLayer.stateMachine, true, advancedAnimations.boolValue);
            
                    var rightHandLayer = builder.PopulateLayer("Right Hand", VrcSdkAssetLocator.HandRight()); 
                    builder.BuildGestureStateMachine(rightHandLayer.stateMachine, false, advancedAnimations.boolValue);

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

                EditorGUILayout.PropertyField(serializedObj.FindProperty("outputAsset"));
                EditorGUILayout.PropertyField(serializedObj.FindProperty("resetClip"));
            });

            serializedObj.ApplyModifiedProperties();
            
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"FX Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
        }

        void RepopulateDefaultFxEmotes()
        {
            var newEmotes = Emote.HandSigns
                .Select(Emote.Populate)
                .ToList();
            fxWizard.emotes = newEmotes;
        }

        void RepopulateDefaultFxEmotes14()
        {
            var newEmotes = Enumerable.Empty<Emote>()
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther),
                        parameter = EmoteParameter.Populate(handSign)
                    }))
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther, GestureConditionMode.NotEqual),
                        parameter = EmoteParameter.Populate(handSign)
                    }))
                .ToList();
            fxWizard.emotes = newEmotes;
        }
    }
}