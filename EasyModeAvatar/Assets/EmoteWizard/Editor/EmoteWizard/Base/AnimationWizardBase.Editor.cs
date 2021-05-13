using System;
using System.Linq;
using EmoteWizard.DataObjects;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EmoteWizard.Base
{
    public abstract class AnimationWizardBaseEditor : Editor
    {
        AnimationWizardBase AnimationWizardBase => target as AnimationWizardBase;

        protected static void RepopulateDefaultGestureEmotes(AnimationWizardBase wizard)
        {
            var newEmotes = Emote.HandSigns
                .Select(Emote.Populate)
                .ToList();
            wizard.emotes = newEmotes;
        }

        protected static void RepopulateDefaultFxEmotes(AnimationWizardBase wizard)
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

        AnimatorController RebuildOrCreateAnimatorController(string relativePath)
        {
            var outputAsset = AnimationWizardBase.outputAsset;
            var path = outputAsset ? AssetDatabase.GetAssetPath(outputAsset) : AnimationWizardBase.EmoteWizardRoot.GeneratedAssetPath(relativePath);

            var animatorController = AnimatorController.CreateAnimatorControllerAtPath(path);
            animatorController.AddParameter("GestureLeft", AnimatorControllerParameterType.Int);
            animatorController.AddParameter("GestureLeftWeight", AnimatorControllerParameterType.Float);
            animatorController.AddParameter("GestureRight", AnimatorControllerParameterType.Int);
            animatorController.AddParameter("GestureRightWeight", AnimatorControllerParameterType.Float);
            return animatorController;
        }

        protected void BuildAnimatorController(string relativePath, Action<AnimatorController> populateLayers)
        {
            var animatorController = RebuildOrCreateAnimatorController(relativePath);

            populateLayers(animatorController);

            animatorController.RemoveLayer(0); // Remove Base Layer
            AssetDatabase.SaveAssets();
            AnimationWizardBase.outputAsset = animatorController;
            EditorUtility.SetDirty(animatorController);
        }

        protected AnimatorControllerLayer PopulateLayer(AnimatorController animatorController, string layerName)
        {
            animatorController.AddLayer(layerName);
            var layer = animatorController.layers[animatorController.layers.Length - 1]; 
            layer.defaultWeight = 1.0f;
            return layer;
        }

        protected void BuildStaticStateMachine(AnimatorStateMachine stateMachine, string stateName, Motion clip)
        {
            var state = stateMachine.AddState(stateName, new Vector2(320, 0));
            state.motion = clip;
            state.writeDefaultValues = false;
            stateMachine.defaultState = state;
        }

        protected void BuildStateMachine(AnimatorStateMachine stateMachine, bool isLeft)
        {
            var emotes = AnimationWizardBase.emotes;

            stateMachine.anyStatePosition = new Vector2(0, 0);

            for (var i = 0; i < emotes.Count; i++)
            {
                var emote = emotes[i];
                var clip = isLeft ? emote.clipLeft : emote.clipRight;
                var state = stateMachine.AddState($"{emote.gesture1.handSign}", new Vector2(320, 0 + i * 70));
                state.motion = clip ? clip : AnimationWizardBase.EmoteWizardRoot.emptyClip;
                state.writeDefaultValues = false;
                var transition = stateMachine.AddAnyStateTransition(state);
                transition.AddCondition(emote.gesture1.ResolvedMode, emote.gesture1.ResolvedThreshold, isLeft ? "GestureLeft" : "GestureRight");
                if (emote.gesture2.mode != GestureConditionMode.Ignore)
                {
                    transition.AddCondition(emote.gesture2.ResolvedMode, emote.gesture2.ResolvedThreshold, isLeft ? "GestureRight" : "GestureLeft");
                }
                transition.hasExitTime = false;
                transition.duration = 0.1f;
                transition.canTransitionToSelf = false;
            }
            
            stateMachine.defaultState = stateMachine.states[0].state;
        }
    }
}