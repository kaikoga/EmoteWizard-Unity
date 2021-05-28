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
    [CustomEditor(typeof(GestureWizard))]
    public class GestureWizardEditor : AnimationWizardBaseEditor
    {
        GestureWizard gestureWizard;

        void OnEnable()
        {
            gestureWizard = target as GestureWizard;
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = this.serializedObject;

            SetupOnlyUI(gestureWizard, () =>
            {
                if (GUILayout.Button("Repopulate Emotes"))
                {
                    RepopulateDefaultGestureEmotes(gestureWizard);
                }
            });

            PropertyFieldWithGenerate(serializedObj.FindProperty("globalClip"), () => gestureWizard.EmoteWizardRoot.ProvideAnimationClip("Gesture/GeneratedGlobalGesture.anim"));
            PropertyFieldWithGenerate(serializedObj.FindProperty("ambienceClip"), () => gestureWizard.EmoteWizardRoot.ProvideAnimationClip("Gesture/GeneratedAmbienceGesture.anim"));
            EditorGUILayout.PropertyField(serializedObj.FindProperty("emotes"), true);

            OutputUIArea(gestureWizard, () =>
            {
                if (GUILayout.Button("Generate Animation Controller"))
                {
                    BuildAnimatorController("Gesture/GeneratedGesture.controller", animatorController =>
                    {
                        var resetLayer = PopulateLayer(animatorController, "Reset"); 
                        BuildStaticStateMachine(resetLayer.stateMachine, "Reset", null);

                        var allPartsLayer = PopulateLayer(animatorController, "AllParts");
                        BuildStaticStateMachine(allPartsLayer.stateMachine, "Global", gestureWizard.globalClip);

                        var ambienceLayer = PopulateLayer(animatorController, "Ambience");
                        BuildStaticStateMachine(ambienceLayer.stateMachine, "Ambient", gestureWizard.ambienceClip);

                        var leftHandLayer = PopulateLayer(animatorController, "Left Hand", VrcSdkAssetLocator.HandLeft()); 
                        BuildGestureStateMachine(leftHandLayer.stateMachine, true);
            
                        var rightHandLayer = PopulateLayer(animatorController, "Right Hand", VrcSdkAssetLocator.HandRight()); 
                        BuildGestureStateMachine(rightHandLayer.stateMachine, false);

                        foreach (var grouping in gestureWizard.ExpressionWizard.GroupExpressionItems)
                        {
                            var expressionLayer = PopulateLayer(animatorController, grouping.Key); 
                            BuildExpressionStateMachine(expressionLayer.stateMachine, grouping.Key, grouping);
                        }

                        BuildParameters(animatorController, gestureWizard.ParametersWizard.parameterItems);
                    });
                }

                EditorGUILayout.PropertyField(serializedObj.FindProperty("outputAsset"));
            });

            serializedObj.ApplyModifiedProperties();
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