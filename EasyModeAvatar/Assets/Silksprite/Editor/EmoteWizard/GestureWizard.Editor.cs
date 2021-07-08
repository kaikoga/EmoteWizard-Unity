using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
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
            
            emotesList = new ExpandableReorderableList(new EmoteListDrawerBase(), serializedObject.FindProperty("emotes"));
            parametersList = new ExpandableReorderableList(new ParameterEmoteListDrawerBase(), serializedObject.FindProperty("parameters"));
            mixinsList = new ExpandableReorderableList(new AnimationMixinListDrawerBase(), serializedObject.FindProperty("mixins"));
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

            CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("globalClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("Gesture/@@@Generated@@@GlobalGesture.anim"));
            CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("ambienceClip"), () => emoteWizardRoot.EnsureAsset<AnimationClip>("Gesture/@@@Generated@@@AmbienceGesture.anim"));

            CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("defaultAvatarMask"), () =>
            {
                var avatarMask = emoteWizardRoot.EnsureAsset<AvatarMask>("Gesture/@@@Generated@@@GestureDefaultMask.mask");
                return AvatarMaskUtils.SetupAsGestureDefault(avatarMask);
            });

            using (EmoteDrawer.StartContext(emoteWizardRoot, advancedAnimations.boolValue))
            {
                emotesList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }

            EmoteWizardGUILayout.RequireAnotherWizard(gestureWizard, parametersWizard, () =>
            {
                if (GUILayout.Button("Collect Parameters"))
                {
                    parametersWizard.TryRefreshParameters();
                    gestureWizard.RefreshParameters(parametersWizard != null ? parametersWizard.parameterItems : null);
                }
            });
            using (ParameterEmoteDrawer.StartContext(emoteWizardRoot, gestureWizard, gestureWizard.LayerName, ParameterEmoteDrawer.EditTargets))
            {
                parametersList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }

            using (AnimationMixinDrawer.StartContext(emoteWizardRoot, GeneratedAssetLocator.MixinDirectoryPath(gestureWizard.LayerName)))
            {
                mixinsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
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