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
    [CustomEditor(typeof(GestureWizard))]
    public class GestureWizardEditor : AnimationWizardBaseEditor
    {
        GestureWizard gestureWizard;

        ExpandableReorderableList baseMixinsList;
        ExpandableReorderableList emotesList;
        ExpandableReorderableList parametersList;
        ExpandableReorderableList mixinsList;

        void OnEnable()
        {
            gestureWizard = target as GestureWizard;
            
            baseMixinsList = new ExpandableReorderableList(new AnimationMixinListDrawerBase(), serializedObject.FindProperty("baseMixins"));
            emotesList = new ExpandableReorderableList(new EmoteListDrawerBase(), serializedObject.FindProperty("emotes"));
            parametersList = new ExpandableReorderableList(new ParameterEmoteListDrawerBase(), serializedObject.FindProperty("parameterEmotes"));
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
                        gestureWizard.parameterEmotes.Clear();
                        gestureWizard.RefreshParameters(parametersWizard != null ? parametersWizard.parameterItems : null);
                    }
                }
            });

            var advancedAnimations = serializedObj.FindProperty("advancedAnimations");
            EditorGUILayout.PropertyField(advancedAnimations);

            CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("defaultAvatarMask"), () =>
            {
                var avatarMask = emoteWizardRoot.EnsureAsset<AvatarMask>("Gesture/@@@Generated@@@GestureDefaultMask.mask");
                return AvatarMaskUtils.SetupAsGestureDefault(avatarMask);
            });

            using (AnimationMixinDrawer.StartContext(emoteWizardRoot, GeneratedAssetLocator.MixinDirectoryPath(gestureWizard.LayerName)))
            {
                baseMixinsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }

            using (EmoteDrawer.StartContext(emoteWizardRoot, advancedAnimations.boolValue))
            {
                emotesList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }

            using (ParameterEmoteDrawer.StartContext(emoteWizardRoot, gestureWizard, gestureWizard.LayerName, ParameterEmoteDrawer.EditTargets))
            {
                parametersList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }
            if (parametersList.serializedProperty.isExpanded)
            {
                EmoteWizardGUILayout.RequireAnotherWizard(gestureWizard, parametersWizard, () =>
                {
                    if (GUILayout.Button("Collect Parameters"))
                    {
                        parametersWizard.TryRefreshParameters();
                        gestureWizard.RefreshParameters(parametersWizard != null
                            ? parametersWizard.parameterItems
                            : null);
                    }
                });
            }

            using (AnimationMixinDrawer.StartContext(emoteWizardRoot, GeneratedAssetLocator.MixinDirectoryPath(gestureWizard.LayerName)))
            {
                mixinsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }

            EmoteWizardGUILayout.OutputUIArea(() =>
            {
                if (GUILayout.Button("Generate Animation Controller"))
                {
                    var builder = new AnimationControllerBuilder
                    {
                        AnimationWizardBase = gestureWizard,
                        ParametersWizard = parametersWizard,
                        DefaultRelativePath = "Gesture/@@@Generated@@@Gesture.controller"
                    };

                    var resetLayer = builder.PopulateLayer("Reset", gestureWizard.defaultAvatarMask ? gestureWizard.defaultAvatarMask : VrcSdkAssetLocator.HandsOnly()); 
                    builder.BuildStaticStateMachine(resetLayer.stateMachine, "Reset", null);

                    foreach (var mixin in gestureWizard.baseMixins.Where(mixin => mixin.Motion != null))
                    {
                        var mixinLayer = builder.PopulateLayer(mixin.name); 
                        builder.BuildMixinLayerStateMachine(mixinLayer.stateMachine, mixin);
                    }

                    var leftHandLayer = builder.PopulateLayer("Left Hand", VrcSdkAssetLocator.HandLeft()); 
                    builder.BuildGestureStateMachine(leftHandLayer.stateMachine, true, advancedAnimations.boolValue);
        
                    var rightHandLayer = builder.PopulateLayer("Right Hand", VrcSdkAssetLocator.HandRight()); 
                    builder.BuildGestureStateMachine(rightHandLayer.stateMachine, false, advancedAnimations.boolValue);

                    foreach (var parameterEmote in gestureWizard.ActiveParameters)
                    {
                        var expressionLayer = builder.PopulateLayer(parameterEmote.name);
                        builder.BuildParameterStateMachine(expressionLayer.stateMachine, parameterEmote);
                    }

                    foreach (var mixin in gestureWizard.mixins.Where(mixin => mixin.Motion != null))
                    {
                        var mixinLayer = builder.PopulateLayer(mixin.name); 
                        builder.BuildMixinLayerStateMachine(mixinLayer.stateMachine, mixin);
                    }

                    builder.BuildParameters();
                }

                EditorGUILayout.PropertyField(serializedObj.FindProperty("outputAsset"));
            });

            serializedObj.ApplyModifiedProperties();
            
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"Gesture Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
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