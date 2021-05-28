using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using EmoteWizard.Tools;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using static EmoteWizard.Extensions.EditorUITools;

namespace EmoteWizard
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : AnimationWizardBaseEditor
    {
        FxWizard fxWizard;

        void OnEnable()
        {
            fxWizard = target as FxWizard;
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = this.serializedObject;

            SetupOnlyUI(fxWizard, () =>
            {
                if (GUILayout.Button("Repopulate Emotes"))
                {
                    RepopulateDefaultFxEmotes(fxWizard);
                }
            });

            PropertyFieldWithGenerate(serializedObj.FindProperty("globalClip"), () => fxWizard.EmoteWizardRoot.ProvideAnimationClip("FX/GeneratedGlobalFX.anim"));
            PropertyFieldWithGenerate(serializedObj.FindProperty("ambienceClip"), () => fxWizard.EmoteWizardRoot.ProvideAnimationClip("FX/GeneratedAmbienceFX.anim"));
            EditorGUILayout.PropertyField(serializedObj.FindProperty("emotes"), true);

            OutputUIArea(fxWizard, () =>
            {
                if (GUILayout.Button("Generate Animation Controller"))
                {
                    BuildAnimatorController("FX/GeneratedFX.controller", animatorController =>
                    {
                        var resetClip = fxWizard.EnsureResetClip();
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

                        foreach (var grouping in fxWizard.ExpressionWizard.GroupExpressionItems)
                        {
                            var expressionLayer = PopulateLayer(animatorController, grouping.Key); 
                            BuildExpressionStateMachine(expressionLayer.stateMachine, grouping.Key, grouping);
                        }
                        
                        BuildParameters(animatorController, fxWizard.ParametersWizard.parameterItems);
                    });
                }

                EditorGUILayout.PropertyField(serializedObj.FindProperty("outputAsset"));
                EditorGUILayout.PropertyField(serializedObj.FindProperty("resetClip"));
            });

            serializedObj.ApplyModifiedProperties();
        }

        static void RepopulateDefaultFxEmotes(AnimationWizardBase wizard)
        {
            var newEmotes = Enumerable.Empty<Emote>()
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther),
                    }))
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther, GestureConditionMode.NotEqual),
                    }))
                .ToList();
            wizard.emotes = newEmotes;
        }
    }
}