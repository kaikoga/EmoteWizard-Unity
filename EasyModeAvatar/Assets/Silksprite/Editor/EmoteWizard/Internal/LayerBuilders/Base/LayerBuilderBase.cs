using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
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

        [Obsolete("Define helper methods instead of direct access LayerBuildBase.StateMachine")]
        protected AnimatorStateMachine LegacyStateMachine => _layer.stateMachine;

        Vector3 _position = new Vector3(300, 0, 0);
        Vector3 NextStatePosition()
        {
            var result = _position;
            _position.y += 60f;
            return result;
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

        [Obsolete("Avoid AnyState")]
        protected AnimatorStateTransition AddStateAsTransition(string stateName, Motion motion)
        {
            var state = StateMachine.AddState(stateName, NextStatePosition());
            state.motion = motion;
            state.writeDefaultValues = false;
            Builder.MarkParameter(motion);
            return StateMachine.AddAnyStateTransition(state);
        }

        protected AnimatorState AddStateWithoutTransition(string stateName, Motion motion)
        {
            var state = StateMachine.AddState(stateName, NextStatePosition());
            state.motion = motion;
            state.writeDefaultValues = false;
            Builder.MarkParameter(motion);
            return state;
        }

        protected void ApplyEmoteConditions(AnimatorStateTransition transition, IEnumerable<EmoteCondition> conditions)
        {
            var validConditions = conditions
                .Where(condition => AssertParameterExists(condition.parameter));
            foreach (var condition in validConditions)
            {
                transition.AddCondition(condition.ToAnimatorConditionMode(), condition.threshold, condition.parameter);
                Builder.MarkParameter(condition.parameter);
            }
        }

        protected void ApplyEmoteGestureConditions(AnimatorStateTransition transition, bool isLeft, EmoteGestureCondition gesture, bool mustEmitSomething = false)
        {
            if (gesture.mode != GestureConditionMode.Ignore)
            {
                var parameter = gesture.ResolveParameter(isLeft);
                transition.AddCondition(gesture.ResolveMode(), gesture.ResolveThreshold(), parameter);
                Builder.MarkParameter(parameter);
            }
            else if (mustEmitSomething)
            {
                transition.AddAlwaysTrueCondition();
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