using System;
using System.Collections.Generic;
using System.Linq;
using EmoteWizard.DataObjects;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static EmoteWizard.Tools.EmoteWizardEditorTools;

namespace EmoteWizard.Base
{
    public abstract class AnimationWizardBaseEditor : Editor
    {
        AnimationWizardBase AnimationWizardBase => target as AnimationWizardBase;

        protected void BuildResetClip(AnimationClip clip)
        {
            IEnumerable<AnimationClip> ClipsInEmote(Emote emote)
            {
                if (emote.clipLeft != null) yield return emote.clipLeft;
                if (emote.clipRight != null) yield return emote.clipRight;
            }

            var allEmoteClips = AnimationWizardBase.emotes.SelectMany(ClipsInEmote);
            var allParameters = allEmoteClips.SelectMany(AnimationUtility.GetCurveBindings)
                .Select(curve => (curve.path, curve.propertyName, curve.type) ) 
                .Distinct().OrderBy(x => x);
            
            clip.ClearCurves();
            clip.frameRate = 60f;
            foreach (var (path, propertyName, type) in allParameters)
            {
                clip.SetCurve(path, type, propertyName, AnimationCurve.Constant(0f, 1 / 60f, 0f));
            }
        }

        AnimatorController RebuildOrCreateAnimatorController(string defaultRelativePath)
        {
            var outputAsset = AnimationWizardBase.outputAsset;
            var path = outputAsset ? AssetDatabase.GetAssetPath(outputAsset) : AnimationWizardBase.EmoteWizardRoot.GeneratedAssetPath(defaultRelativePath);

            EnsureDirectory(path);
            var animatorController = AnimatorController.CreateAnimatorControllerAtPath(path);
            animatorController.AddParameter("GestureLeft", AnimatorControllerParameterType.Int);
            animatorController.AddParameter("GestureLeftWeight", AnimatorControllerParameterType.Float);
            animatorController.AddParameter("GestureRight", AnimatorControllerParameterType.Int);
            animatorController.AddParameter("GestureRightWeight", AnimatorControllerParameterType.Float);
            return animatorController;
        }

        protected void BuildAnimatorController(string defaultRelativePath, Action<AnimatorController> populateLayers)
        {
            var animatorController = RebuildOrCreateAnimatorController(defaultRelativePath);

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
            stateMachine.anyStatePosition = new Vector2(0, 0);
            stateMachine.entryPosition = new Vector2(0, 100);
            stateMachine.exitPosition = new Vector2(0, 200);
            if (!clip) return;

            var state = stateMachine.AddState(stateName, new Vector2(300, 0));
            state.motion = clip;
            state.writeDefaultValues = false;
            stateMachine.defaultState = state;
        }

        protected void BuildStateMachine(AnimatorStateMachine stateMachine, bool isLeft)
        {
            var emotes = AnimationWizardBase.emotes;

            stateMachine.anyStatePosition = new Vector2(0, 0);
            stateMachine.entryPosition = new Vector2(0, 100);
            stateMachine.exitPosition = new Vector2(0, 200);

            var position = new Vector2(300, 0);
            foreach (var emote in emotes)
            {
                var clip = isLeft ? emote.clipLeft : emote.clipRight;
                var state = stateMachine.AddState(emote.ToStateName(), position);
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
                position.y += 60;
            }
            
            stateMachine.defaultState = stateMachine.states[0].state;
        }
    }
}