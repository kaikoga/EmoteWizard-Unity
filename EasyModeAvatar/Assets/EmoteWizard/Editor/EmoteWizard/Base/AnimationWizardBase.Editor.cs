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

        protected void BuildGestureStateMachine(AnimatorStateMachine stateMachine, bool isLeft, bool isAdvanced)
        {
            var emotes = AnimationWizardBase.emotes;

            stateMachine.anyStatePosition = new Vector2(0, 0);
            stateMachine.entryPosition = new Vector2(0, 100);
            stateMachine.exitPosition = new Vector2(0, 200);

            var position = new Vector2(300, 0);
            foreach (var emote in emotes)
            {
                var gesture1 = emote.gesture1;
                var gesture2 = emote.gesture2;
                
                var clip = isLeft || !isAdvanced ? emote.clipLeft : emote.clipRight;
                var state = stateMachine.AddState(emote.ToStateName(), position);
                state.motion = clip ? clip : AnimationWizardBase.EmoteWizardRoot.ProvideEmptyClip();
                state.writeDefaultValues = false;
                if (clip != null && emote.parameter != null && emote.parameter.normalizedTimeEnabled)
                {
                    state.timeParameterActive = true;
                    state.timeParameter = isLeft ? emote.parameter.normalizedTimeLeft : emote.parameter.normalizedTimeRight;
                    clip.SetLoopTimeRec(false);
                    EditorUtility.SetDirty(clip);
                }

                var transition = stateMachine.AddAnyStateTransition(state);
                transition.AddCondition(gesture1.ResolveMode(), gesture1.ResolveThreshold(), gesture1.ResolveParameter(isLeft));
                if (gesture2.mode != GestureConditionMode.Ignore)
                {
                    transition.AddCondition(gesture2.ResolveMode(), gesture2.ResolveThreshold(), gesture2.ResolveParameter(isLeft));
                }
                transition.hasExitTime = false;
                transition.duration = 0.1f;
                transition.canTransitionToSelf = false;
                position.y += 60;
            }
            
            stateMachine.defaultState = stateMachine.states.FirstOrDefault().state;
        }

        protected void BuildExpressionStateMachine(AnimatorStateMachine stateMachine, ParameterEmote parameterEmote)
        {
            void AddTransition(AnimatorState state, string parameterName, float value)
            {
                AnimatorStateTransition transition;
                switch (parameterEmote.valueType)
                {
                    case VRCExpressionParameters.ValueType.Int:
                        transition = stateMachine.AddAnyStateTransition(state);
                        transition.AddCondition(AnimatorConditionMode.Equals, value, parameterName);
                        break;
                    case VRCExpressionParameters.ValueType.Float:
                        return; // TODO
                    case VRCExpressionParameters.ValueType.Bool:
                        transition = stateMachine.AddAnyStateTransition(state);
                        transition.AddCondition(value != 0 ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot, value, parameterName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                transition.hasExitTime = false;
                transition.duration = 0.1f;
                transition.canTransitionToSelf = false;
            }

            stateMachine.anyStatePosition = new Vector2(0, 0);
            stateMachine.entryPosition = new Vector2(0, 100);
            stateMachine.exitPosition = new Vector2(0, 200);

            var position = new Vector2(300, 0);
            foreach (var parameterEmoteState in parameterEmote.states)
            {
                var state = stateMachine.AddState($"{parameterEmote.parameter} = {parameterEmoteState.value}", position);
                state.motion = parameterEmoteState.clip;
                state.writeDefaultValues = false;
                AddTransition(state, parameterEmote.parameter, parameterEmoteState.value);
                position.y += 60;
            }
        }
        
        protected void BuildMixinLayerStateMachine(AnimatorStateMachine stateMachine, AnimationMixin mixin)
        {
            stateMachine.anyStatePosition = new Vector2(0, 0);
            stateMachine.entryPosition = new Vector2(0, 100);
            stateMachine.exitPosition = new Vector2(0, 200);

            var position = new Vector2(300, 0);
            var state = stateMachine.AddState(mixin.name, position);
            state.motion = mixin.Motion;
            state.writeDefaultValues = false;

            if (mixin.kind == AnimationMixin.AnimationMixinKind.AnimationClip && mixin.normalizedTimeEnabled)
            {
                state.timeParameterActive = true;
                state.timeParameter = mixin.normalizedTime;
                mixin.Motion.SetLoopTimeRec(false);
                EditorUtility.SetDirty(mixin.Motion);
            }
        }

        protected void BuildParameters(AnimatorController animatorController, IEnumerable<ParameterItem> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.defaultParameter) continue;

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