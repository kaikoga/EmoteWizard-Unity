using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using EmoteWizard.Tools;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.EditorUITools;

namespace EmoteWizard
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : AnimationWizardBaseEditor
    {
        FxWizard fxWizard;
        ExpandableReorderableList emotesList;

        void OnEnable()
        {
            fxWizard = target as FxWizard;

            emotesList = new ExpandableReorderableList(serializedObject, serializedObject.FindProperty("emotes"), "Emotes");
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = fxWizard.EmoteWizardRoot;

            SetupOnlyUI(fxWizard, () =>
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

            PropertyFieldWithGenerate(serializedObj.FindProperty("globalClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("FX/@@@Generated@@@GlobalFX.anim"));
            PropertyFieldWithGenerate(serializedObj.FindProperty("ambienceClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("FX/@@@Generated@@@AmbienceFX.anim"));
            
            EmoteDrawer.DrawHeader(emoteWizardRoot.useReorderUI);
            emotesList.DrawAsProperty(emoteWizardRoot.useReorderUI);

            OutputUIArea(() =>
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