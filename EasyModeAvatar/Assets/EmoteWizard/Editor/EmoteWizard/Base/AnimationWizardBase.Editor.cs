using System;
using System.Collections.Generic;
using System.Linq;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.Base
{
    public abstract class AnimationWizardBaseEditor : Editor
    {
        AnimationWizardBase AnimationWizardBase => target as AnimationWizardBase;

        protected void BuildResetClip(AnimationClip clip)
        {
            var allEmoteClips = AnimationWizardBase.emotes.SelectMany(e => e.AllClips());
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

        protected void BuildAnimatorController(string defaultRelativePath, Action<AnimatorController> populateLayers)
        {
            var animatorController = AnimationWizardBase.ReplaceOrCreateOutputAsset(ref AnimationWizardBase.outputAsset, defaultRelativePath);
            populateLayers(animatorController);
        }

        protected AnimatorControllerLayer PopulateLayer(AnimatorController animatorController, string layerName, AvatarMask avatarMask = null)
        {
            layerName = animatorController.MakeUniqueLayerName(layerName);
            var layer = new AnimatorControllerLayer
            {
                name = layerName,
                defaultWeight = 1.0f,
                avatarMask = avatarMask,
                stateMachine = new AnimatorStateMachine
                {
                    name = layerName,
                    hideFlags = HideFlags.HideInHierarchy
                }
            };
            AssetDatabase.AddObjectToAsset(layer.stateMachine, AssetDatabase.GetAssetPath(animatorController));
            animatorController.AddLayer(layer);
            return layer;
        }

        protected void BuildStaticStateMachine(AnimatorStateMachine stateMachine, string stateName, Motion clip)
        {
            stateMachine.anyStatePosition = new Vector2(0, 0);
            stateMachine.entryPosition = new Vector2(0, 100);
            stateMachine.exitPosition = new Vector2(0, 200);

            var state = stateMachine.AddState(stateName, new Vector2(300, 0));
            state.motion = clip;
            state.writeDefaultValues = false;
            stateMachine.defaultState = state;
        }

        protected void BuildGestureStateMachine(AnimatorStateMachine stateMachine, bool isLeft)
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

        protected void BuildExpressionStateMachine(AnimatorStateMachine stateMachine, string parameterName,
            IEnumerable<ExpressionItem> expressionItems)
        {
            stateMachine.anyStatePosition = new Vector2(0, 0);
            stateMachine.entryPosition = new Vector2(0, 100);
            stateMachine.exitPosition = new Vector2(0, 200);

            var position = new Vector2(300, 0);
            {
                var state = stateMachine.AddState($"{parameterName} off", position);
                state.motion = AnimationWizardBase.EmoteWizardRoot.emptyClip;
                state.writeDefaultValues = false;
                var transition = stateMachine.AddAnyStateTransition(state);
                transition.AddCondition(AnimatorConditionMode.Equals, 0, parameterName);
                transition.hasExitTime = false;
                transition.duration = 0.1f;
                transition.canTransitionToSelf = false;
                position.y += 60;
            }

            foreach (var expressionItem in expressionItems)
            {
                var state = stateMachine.AddState($"{expressionItem.parameter} = {expressionItem.value}", position);
                state.motion = AnimationWizardBase.EmoteWizardRoot.emptyClip;
                state.writeDefaultValues = false;
                var transition = stateMachine.AddAnyStateTransition(state);
                // FIXME: bool params use If and IfNot
                transition.AddCondition(AnimatorConditionMode.Equals, expressionItem.value, expressionItem.parameter);
                transition.hasExitTime = false;
                transition.duration = 0.1f;
                transition.canTransitionToSelf = false;
                position.y += 60;
            }
        }

        protected void BuildParameters(AnimatorController animatorController, IEnumerable<VRCExpressionParameters.Parameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                var parameterName = parameter.name;
                AnimatorControllerParameterType parameterType;
                switch (parameter.valueType)
                {
                    case VRCExpressionParameters.ValueType.Int:
                        parameterType = AnimatorControllerParameterType.Int;
                        break;
                    case VRCExpressionParameters.ValueType.Float:
                        parameterType = AnimatorControllerParameterType.Float;
                        break;
                    case VRCExpressionParameters.ValueType.Bool:
                        parameterType = AnimatorControllerParameterType.Bool;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                animatorController.AddParameter(parameterName, parameterType);
            }
        }
    }
}