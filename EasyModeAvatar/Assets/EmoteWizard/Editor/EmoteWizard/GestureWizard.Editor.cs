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
                null);
            mixinsList = new ExpandableReorderableList(serializedObject,
                serializedObject.FindProperty("mixins"),
                "Mixins",
                new AnimationMixinListHeaderDrawer());
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = gestureWizard.EmoteWizardRoot;

            EmoteWizardGUILayout.SetupOnlyUI(gestureWizard, () =>
            {
                if (GUILayout.Button("Repopulate Emotes"))
                {
                    RepopulateDefaultGestureEmotes(gestureWizard);
                }
                if (GUILayout.Button("Repopulate Parameters"))
                {
                    gestureWizard.ParametersWizard.RefreshParameters();
                    gestureWizard.parameters.Clear();
                    gestureWizard.RefreshParameters(gestureWizard.ParametersWizard != null ? gestureWizard.ParametersWizard.parameterItems : null);
                }
            });

            EmoteWizardGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("globalClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("Gesture/@@@Generated@@@GlobalGesture.anim"));
            EmoteWizardGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("ambienceClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("Gesture/@@@Generated@@@AmbienceGesture.anim"));

            EmoteWizardGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("defaultAvatarMask"), () =>
            {
                var avatarMask = emoteWizardRoot.EnsureAsset<AvatarMask>("Gesture/@@@Generated@@@GestureDefaultMask.mask");
                return AvatarMaskTools.SetupAsGestureDefault(avatarMask);
            });

            emotesList.DrawAsProperty(emoteWizardRoot.useReorderUI);
            using (ParameterEmoteDrawer.StartContext(emoteWizardRoot, "Gesture"))
            {
                parametersList.DrawAsProperty(emoteWizardRoot.useReorderUI);
            }
            if (GUILayout.Button("Collect Parameters"))
            {
                gestureWizard.ParametersWizard.RefreshParameters();
                gestureWizard.RefreshParameters(gestureWizard.ParametersWizard != null ? gestureWizard.ParametersWizard.parameterItems : null);
            }

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
                        BuildGestureStateMachine(leftHandLayer.stateMachine, true);
            
                        var rightHandLayer = PopulateLayer(animatorController, "Right Hand", VrcSdkAssetLocator.HandRight()); 
                        BuildGestureStateMachine(rightHandLayer.stateMachine, false);

                        foreach (var parameterItem in gestureWizard.ParametersWizard.CustomParameterItems)
                        {
                            var parameterEmote = gestureWizard.parameters.FirstOrDefault(emote => emote.parameter == parameterItem.name);
                            if (parameterEmote == null) continue;
                            var expressionLayer = PopulateLayer(animatorController, parameterItem.name);
                            
                            BuildExpressionStateMachine(expressionLayer.stateMachine, parameterItem, parameterEmote);
                        }

                        foreach (var mixin in gestureWizard.mixins)
                        {
                            var mixinLayer = PopulateLayer(animatorController, mixin.name); 
                            BuildMixinLayerStateMachine(mixinLayer.stateMachine, mixin);
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