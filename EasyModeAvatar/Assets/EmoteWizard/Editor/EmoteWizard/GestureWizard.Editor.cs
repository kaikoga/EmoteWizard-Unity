using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects;
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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("emotes"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("outputAsset"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("globalClip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ambienceClip"));

            SetupOnlyUI(gestureWizard, () =>
            {
                if (GUILayout.Button("Repopulate Emotes"))
                {
                    RepopulateDefaultGestureEmotes(gestureWizard);
                }
            });
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
                    BuildStateMachine(leftHandLayer.stateMachine, true);
            
                    var rightHandLayer = PopulateLayer(animatorController, "Right Hand", VrcSdkAssetLocator.HandRight()); 
                    BuildStateMachine(rightHandLayer.stateMachine, false);
                });
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