using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.Collections;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using EmoteWizard.Tools;
using EmoteWizard.UI;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : AnimationWizardBaseEditor
    {
        FxWizard fxWizard;

        ExpandableReorderableList emotesList;
        ExpandableReorderableList mixinsList;

        void OnEnable()
        {
            fxWizard = target as FxWizard;

            emotesList = new ExpandableReorderableList(serializedObject,
                serializedObject.FindProperty("emotes"),
                "Emotes",
                new EmoteListHeaderDrawer());
            mixinsList = new ExpandableReorderableList(serializedObject,
                serializedObject.FindProperty("mixins"),
                "Mixins",
                new AnimationMixinListHeaderDrawer());
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = fxWizard.EmoteWizardRoot;

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
            });

            EmoteWizardGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("globalClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("FX/@@@Generated@@@GlobalFX.anim"));
            EmoteWizardGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("ambienceClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("FX/@@@Generated@@@AmbienceFX.anim"));
            
            emotesList.DrawAsProperty(emoteWizardRoot.useReorderUI);
            using (AnimationMixinDrawer.StartContext(emoteWizardRoot, "FX/Mixin/"))
            {
                mixinsList.DrawAsProperty(emoteWizardRoot.useReorderUI);
            }

            EmoteWizardGUILayout.OutputUIArea(() =>
            {
                if (GUILayout.Button("Generate Animation Controller"))
                {
                    BuildAnimatorController("FX/@@@Generated@@@FX.controller", animatorController =>
                    {
                        var resetClip = fxWizard.ProvideResetClip();
                        BuildResetClip(resetClip);
                        
                        var resetLayer = PopulateLayer(animatorController, "Reset");
                        BuildStaticStateMachine(resetLayer.stateMachine, "Reset", resetClip);

                        var allPartsLayer = PopulateLayer(animatorController, "AllParts");
                        BuildStaticStateMachine(allPartsLayer.stateMachine, "Global", fxWizard.globalClip);

                        var ambienceLayer = PopulateLayer(animatorController, "Ambience");
                        BuildStaticStateMachine(ambienceLayer.stateMachine, "Global", fxWizard.ambienceClip);

                        var leftHandLayer = PopulateLayer(animatorController, "Left Hand", VrcSdkAssetLocator.HandLeft()); 
                        BuildGestureStateMachine(leftHandLayer.stateMachine, true);
                
                        var rightHandLayer = PopulateLayer(animatorController, "Right Hand", VrcSdkAssetLocator.HandRight()); 
                        BuildGestureStateMachine(rightHandLayer.stateMachine, false);

                        foreach (var parameterItem in fxWizard.ParametersWizard.CustomParameterItems)
                        {
                            var expressionLayer = PopulateLayer(animatorController, parameterItem.name); 
                            BuildExpressionStateMachine(expressionLayer.stateMachine, parameterItem, true);
                        }
                        
                        foreach (var mixin in fxWizard.mixins)
                        {
                            var mixinLayer = PopulateLayer(animatorController, mixin.name); 
                            BuildMixinLayerStateMachine(mixinLayer.stateMachine, mixin);
                        }
                        
                        BuildParameters(animatorController, fxWizard.ParametersWizard.CustomParameterItems);
                    });
                }

                EditorGUILayout.PropertyField(serializedObj.FindProperty("outputAsset"));
                EditorGUILayout.PropertyField(serializedObj.FindProperty("resetClip"));
            });

            serializedObj.ApplyModifiedProperties();
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