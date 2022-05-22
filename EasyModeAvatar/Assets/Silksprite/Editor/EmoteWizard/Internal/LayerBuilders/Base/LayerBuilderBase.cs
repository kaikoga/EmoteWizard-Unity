using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.ConditionBuilders;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders.Base
{
    public abstract class LayerBuilderBase
    {
        protected readonly AnimationControllerBuilder Builder;
        readonly AnimatorControllerLayer _layer;

        protected AnimationWizardBase AnimationWizardBase => Builder.AnimationWizardBase;
        AnimatorStateMachine StateMachine => _layer.stateMachine;

        Vector3 _position = new Vector3(0f, 0f, 0f);
        Vector3 NextStatePosition()
        {
            var result = _position;
            _position.y += 75f;
            return result;
        }

        protected void NextStateColumn(int indent = 0)
        {
            _position.x += 300f;
            _position.y = 75f * indent;
        }

        [Obsolete("Please provide ParameterItemKind")]
        bool AssertParameterExists(string parameterName) => AssertParameterExists(parameterName, ParameterItemKind.Auto);

        protected bool AssertParameterExists(string parameterName, ParameterItemKind itemKind)
        {
            return Builder.ParametersWizard == null || Builder.ParametersWizard.AssertParameterExists(parameterName, itemKind);
        }

        protected LayerBuilderBase(AnimationControllerBuilder builder, AnimatorControllerLayer layer)
        {
            Builder = builder;
            _layer = layer;
        }

        public void Build()
        {
            Process();
            if (StateMachine.defaultState == null)
            {
                StateMachine.defaultState = StateMachine.states.FirstOrDefault().state;
            }
        }

        protected abstract void Process();

        AnimatorState AddStateWithoutTransition(string stateName, Motion motion, bool isReallyEmptyState)
        {
            if (motion == null && !isReallyEmptyState) motion = AnimationWizardBase.EmoteWizardRoot.ProvideEmptyClip();
            var state = StateMachine.AddState(stateName, NextStatePosition());
            state.motion = motion;
            state.writeDefaultValues = false;
            Builder.MarkParameter(motion);
            return state;
        }

        protected AnimatorState AddStateWithoutTransition(string stateName, Motion motion) => AddStateWithoutTransition(stateName, motion, false);

        protected AnimatorStateTransition AddTransition(AnimatorState fromState, AnimatorState toState, ConditionBuilder conditions = null)
        {
            var transition = fromState.AddTransition(toState);
            transition.conditions = conditions?.ToArray();
            return transition;
        }

        protected AnimatorStateTransition AddTransitionAndCopyConditions(AnimatorState fromState, AnimatorState toState, AnimatorCondition[] rawConditions)
        {
            var transition = fromState.AddTransition(toState);
            transition.conditions = new AnimatorCondition[] { };
            foreach (var sourceCondition in rawConditions)
            {
                transition.AddCondition(sourceCondition.mode, sourceCondition.threshold, sourceCondition.parameter);
            }
            return transition;
        }

        protected AnimatorStateTransition AddExitTransition(AnimatorState fromState, ConditionBuilder conditions = null)
        {
            var transition = fromState.AddExitTransition(false);
            transition.conditions = conditions?.ToArray();
            return transition;
        }

        protected void AddExitTransitions(AnimatorState fromState, IEnumerable<ConditionBuilder> conditions)
        {
            foreach (var cond in conditions) AddExitTransition(fromState, cond);
        }

        protected AnimatorState PopulateDefaultState(string stateName = "Default", Motion clip = null)
        {
            var defaultState = AddStateWithoutTransition(stateName, clip, true);
            NextStateColumn();
            return defaultState;
        }

        protected AnimatorStateTransition AddSelectTransition(AnimatorState defaultState, AnimatorState state, ConditionBuilder conditions)
        {
            var transition = AddTransition(defaultState, state, conditions);
            AddExitTransitions(state, conditions.Inverse());
            return transition;
        }

        protected void ApplyEmoteConditions(ConditionBuilder conditions, IEnumerable<EmoteCondition> emoteConditions)
        {
            var validConditions = emoteConditions
                .Where(emoteCondition => AssertParameterExists(emoteCondition.parameter));
            foreach (var condition in validConditions)
            {
                conditions.EmoteCondition(condition);
                Builder.MarkParameter(condition.parameter);
            }
        }

        protected void ApplyEmoteGestureConditions(ConditionBuilder conditions, bool isLeft, EmoteGestureCondition gesture, bool mustEmitSomething = false)
        {
            if (gesture.mode != GestureConditionMode.Ignore)
            {
                var parameter = gesture.ResolveParameter(isLeft);
                conditions.AddCondition(AnimatorControllerParameterType.Int, gesture.ResolveMode(), parameter, gesture.ResolveThreshold());
                Builder.MarkParameter(parameter);
            }
            else if (mustEmitSomething)
            {
                conditions.AlwaysTrue();
            }
        }

        TrackingTarget[] _allTrackingTargets;

        protected void InitEmoteControl(IEnumerable<TrackingOverride> trackingOverrides)
        {
            _allTrackingTargets = trackingOverrides.Select(o => o.target).Distinct().ToArray();
        }

        protected void ApplyEmoteControl(AnimatorStateTransition transition, bool isLeft, EmoteControl control)
        {
            ApplyEmoteTrackingControl(transition.destinationState, control, transition);
            ApplyEmoteTransitionControl(transition, isLeft, control);
        }

        protected void ApplyDefaultEmoteControl(AnimatorState state)
        {
            ApplyEmoteTrackingControl(state);
        }

        void ApplyEmoteTrackingControl(AnimatorState state, EmoteControl control = null, AnimatorStateTransition transition = null)
        {
            if (_allTrackingTargets == null)
            {
                throw new InvalidOperationException("Please call InitEmoteControl()");
            }

            if (_allTrackingTargets.Length == 0) return;

            TrackingTarget[] currentTrackingTargets;
            if (control == null)
            {
                currentTrackingTargets = new TrackingTarget[] { };
            }
            else
            {
                currentTrackingTargets = control.trackingOverrides
                    .Select(o => o.target)
                    .Where(target => target != TrackingTarget.None)
                    .ToArray();
            }

            if (transition != null)
            {
                foreach (var target in currentTrackingTargets)
                {
                    Builder.RegisterOverrider(target, transition);
                }
            }

            var avatarParameterDriver = state.AddStateMachineBehaviour<VRCAvatarParameterDriver>();
            avatarParameterDriver.localOnly = true;
            avatarParameterDriver.parameters = _allTrackingTargets.Select(target => new VRC_AvatarParameterDriver.Parameter
            {
                name = target.ToAnimatorParameterName(currentTrackingTargets.Contains(target)),
                value = 0f,
                valueMin = 0f,
                valueMax = 0f,
                chance = 1f,
                type = VRC_AvatarParameterDriver.ChangeType.Set
            }).ToList();
        }
        
        void ApplyEmoteTransitionControl(AnimatorStateTransition transition, bool isLeft, EmoteControl control)
        {
            var state = transition.destinationState;

            transition.hasExitTime = false;
            transition.duration = control.transitionDuration;
            transition.canTransitionToSelf = false;

            if (state.motion == null || !control.normalizedTimeEnabled) return;

            var timeParameter = isLeft ? control.normalizedTimeLeft : control.normalizedTimeRight;
            if (!AssertParameterExists(timeParameter, ParameterItemKind.Float)) return;

            state.timeParameterActive = true;
            state.timeParameter = timeParameter;
            state.motion.SetLoopTimeRec(false);
            EditorUtility.SetDirty(state.motion);
            Builder.MarkParameter(timeParameter);
        }
    }
}