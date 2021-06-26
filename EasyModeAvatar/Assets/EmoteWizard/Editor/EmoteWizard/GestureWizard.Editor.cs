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

        ExpandableReorderableList emotesList;

        void OnEnable()
        {
            gestureWizard = target as GestureWizard;
            
            emotesList = new ExpandableReorderableList(serializedObject, serializedObject.FindProperty("emotes"), "Emotes");
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = gestureWizard.EmoteWizardRoot;

            SetupOnlyUI(gestureWizard, () =>
            {
                if (GUILayout.Button("Repopulate Emotes"))
                {
                    RepopulateDefaultGestureEmotes(gestureWizard);
                }
            });

            PropertyFieldWithGenerate(serializedObj.FindProperty("globalClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("Gesture/@@@Generated@@@GlobalGesture.anim"));
            PropertyFieldWithGenerate(serializedObj.FindProperty("ambienceClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("Gesture/@@@Generated@@@AmbienceGesture.anim"));

            PropertyFieldWithGenerate(serializedObj.FindProperty("defaultAvatarMask"), () =>
            {
                var avatarMask = emoteWizardRoot.EnsureAsset<AvatarMask>("Gesture/@@@Generated@@@GestureDefaultMask.mask");
                return AvatarMaskTools.SetupAsGestureDefault(avatarMask);
            });

            emotesList.DrawAsProperty(emoteWizardRoot.useReorderUI);

            OutputUIArea(() =>
            {
                if (GUILayout.Button("Generate Animation Controller"))
                {
                    BuildAnimatorController("Gesture/@@@Generated@@@Gesture.controller", animatorController =>
                    {
                        var resetLayer = PopulateLayer(animatorController, "Reset", gestureWizard.defaultAvatarMask ? gestureWizard.defaultAvatarMask : VrcSdkAssetLocator.HandsOnly()); 
                        BuildStaticStateMachine(resetLayer.stateMachine, "Reset", null);

                        var allPartsLayer = PopulateLayer(animatorController, "AllParts");
                        BuildStaticStateMachine(allPartsLayer.stateMachine, "Global", gestureWizard.globalClip);

                        var ambienceLayer = PopulateLayer(animatorController, "Ambience");
                        BuildStaticStateMachine(ambienceLayer.stateMachine, "Ambient", gestureWizard.ambienceClip);

                        var leftHandLayer = PopulateLayer(animatorController, "Left Hand", VrcSdkAssetLocator.HandLeft()); 
                        BuildGestureStateMachine(leftHandLayer.stateMachine, true);
            
                        var rightHandLayer = PopulateLayer(animatorController, "Right Hand", VrcSdkAssetLocator.HandRight()); 
                        BuildGestureStateMachine(rightHandLayer.stateMachine, false);

                        foreach (var parameterItem in gestureWizard.ParametersWizard.CustomParameterItems)
                        {
                            var expressionLayer = PopulateLayer(animatorController, parameterItem.name); 
                            BuildExpressionStateMachine(expressionLayer.stateMachine, parameterItem, false);
                        }

                        BuildParameters(animatorController, gestureWizard.ParametersWizard.CustomParameterItems);
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