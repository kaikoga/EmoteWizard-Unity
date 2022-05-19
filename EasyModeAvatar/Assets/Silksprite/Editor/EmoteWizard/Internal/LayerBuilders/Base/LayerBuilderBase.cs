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
            _position.y += 60f;
            return result;
        }

        protected void NextStateColumn(int indent = 0)
        {
            _position.x += 300f;
            _position.y = 75f * indent;
        }

        protected bool AssertParameterExists(string parameterName)
        {
            return Builder.ParametersWizard == null || Builder.ParametersWizard.AssertParameterExists(parameterName);
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

        protected AnimatorState AddStateWithoutTransition(string stateName, Motion motion)
        {
            if (motion == null) motion = AnimationWizardBase.EmoteWizardRoot.ProvideEmptyClip();
            var state = StateMachine.AddState(stateName, NextStatePosition());
            state.motion = motion;
            state.writeDefaultValues = false;
            Builder.MarkParameter(motion);
            return state;
        }

        [Obsolete("Avoid AnyState")]
        protected AnimatorStateTransition AddAnyStateTransition(AnimatorState state, ConditionBuilder conditions = null)
        {
            var transition = StateMachine.AddAnyStateTransition(state);
            transition.conditions = conditions?.ToArray();
            return transition;
        }

        protected AnimatorStateTransition AddTransition(AnimatorState fromState, AnimatorState toState, ConditionBuilder conditions = null)
        {
            var transition = fromState.AddTransition(toState);
            transition.conditions = conditions?.ToArray();
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

        protected void ApplyEmoteControl(AnimatorStateTransition transition, bool isLeft, EmoteControl control)
        {
            foreach (var enforcer in control.trackingOverrides)
            {
                Builder.RegisterOverrider(enforcer.target, transition);
            }

            transition.hasExitTime = false;
            transition.duration = control.transitionDuration;
            transition.canTransitionToSelf = false;

            var state = transition.destinationState;
            if (state.motion == null || !control.normalizedTimeEnabled) return;

            var timeParameter = isLeft ? control.normalizedTimeLeft : control.normalizedTimeRight;
            if (!AssertParameterExists(timeParameter)) return;

            state.timeParameterActive = true;
            state.timeParameter = timeParameter;
            state.motion.SetLoopTimeRec(false);
            EditorUtility.SetDirty(state.motion);
            Builder.MarkParameter(timeParameter);
        }
    }
}