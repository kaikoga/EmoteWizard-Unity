using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.Collections;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using EmoteWizard.UI;
using EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard
{
    [CustomEditor(typeof(GestureWizard))]
    public class GestureWizardEditor : AnimationWizardBaseEditor
    {
        GestureWizard gestureWizard;

        ExpandableReorderableList emotesList;
        ExpandableReorderableList parametersList;
        ExpandableReorderableList mixinsList;

        void OnEnable()
        {
            gestureWizard = target as GestureWizard;
            
            emotesList = new ExpandableReorderableList(serializedObject,
                serializedObject.FindProperty("emotes"),
                "Emotes",
                new EmoteListHeaderDrawer());
            parametersList = new ExpandableReorderableList(serializedObject,
                serializedObject.FindProperty("parameters"),
                "Parameters",
                new ParameterEmoteListHeaderDrawer());
            mixinsList = new ExpandableReorderableList(serializedObject,
                serializedObject.FindProperty("mixins"),
                "Mixins",
                new AnimationMixinListHeaderDrawer());
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = gestureWizard.EmoteWizardRoot;
            var parametersWizard = gestureWizard.GetComponent<ParametersWizard>();

            EmoteWizardGUILayout.SetupOnlyUI(gestureWizard, () =>
            {
                if (GUILayout.Button("Repopulate Emotes"))
                {
                    RepopulateDefaultGestureEmotes(gestureWizard);
                }
                if (parametersWizard != null)
                {
                    if (GUILayout.Button("Repopulate Parameters"))
                    {
                        parametersWizard.TryRefreshParameters();
                        gestureWizard.parameters.Clear();
                        gestureWizard.RefreshParameters(parametersWizard != null ? parametersWizard.parameterItems : null);
                    }
                }
            });

            var advancedAnimations = serializedObj.FindProperty("advancedAnimations");
            EditorGUILayout.PropertyField(advancedAnimations);

            EmoteWizardGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("globalClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("Gesture/@@@Generated@@@GlobalGesture.anim"));
            EmoteWizardGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("ambienceClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("Gesture/@@@Generated@@@AmbienceGesture.anim"));

            EmoteWizardGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("defaultAvatarMask"), () =>
            {
                var avatarMask = emoteWizardRoot.EnsureAsset<AvatarMask>("Gesture/@@@Generated@@@GestureDefaultMask.mask");
                return AvatarMaskUtils.SetupAsGestureDefault(avatarMask);
            });

            using (EmoteDrawer.StartContext(emoteWizardRoot, advancedAnimations.boolValue))
            {
                emotesList.DrawAsProperty(emoteWizardRoot.useReorderUI);
            }
            using (ParameterEmoteDrawer.StartContext(emoteWizardRoot, gestureWizard, "Gesture", ParameterEmoteDrawer.EditTargets))
            {
                parametersList.DrawAsProperty(emoteWizardRoot.useReorderUI);
            }

            EmoteWizardGUILayout.RequireAnotherWizard(gestureWizard, parametersWizard, () =>
            {
                if (GUILayout.Button("Collect Parameters"))
                {
                    parametersWizard.TryRefreshParameters();
                    gestureWizard.RefreshParameters(parametersWizard != null ? parametersWizard.parameterItems : null);
                }
            });

            using (AnimationMixinDrawer.StartContext(emoteWizardRoot, "Gesture/Mixin/"))
            {
                mixinsList.DrawAsProperty(emoteWizardRoot.useReorderUI);
            }

            EmoteWizardGUILayout.OutputUIArea(() =>
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
                        BuildGestureStateMachine(leftHandLayer.stateMachine, true, advancedAnimations.boolValue);
            
                        var rightHandLayer = PopulateLayer(animatorController, "Right Hand", VrcSdkAssetLocator.HandRight()); 
                        BuildGestureStateMachine(rightHandLayer.stateMachine, false, advancedAnimations.boolValue);

                        foreach (var parameterEmote in gestureWizard.ActiveParameters)
                        {
                            var expressionLayer = PopulateLayer(animatorController, parameterEmote.name);
                            BuildParameterStateMachine(expressionLayer.stateMachine, parameterEmote);
                        }

                        foreach (var mixin in gestureWizard.mixins)
                        {
                            var mixinLayer = PopulateLayer(animatorController, mixin.name); 
                            BuildMixinLayerStateMachine(mixinLayer.stateMachine, mixin);
                        }

                        BuildParameters(animatorController, parametersWizard.CustomParameterItems);
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