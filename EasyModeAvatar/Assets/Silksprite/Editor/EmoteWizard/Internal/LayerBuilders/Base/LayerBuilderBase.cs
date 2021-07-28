using System.Linq;
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
        protected AnimatorStateMachine StateMachine => _layer.stateMachine;

        Vector3 _position = new Vector3(300, -60, 0);
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

        protected AnimatorStateTransition AddStateAsTransition(string stateName, Motion motion)
        {
            var state = StateMachine.AddState(stateName, NextStatePosition());
            state.motion = motion;
            state.writeDefaultValues = false;
            return StateMachine.AddAnyStateTransition(state);
        }

        protected void ApplyEmoteConditions(AnimatorStateTransition transition, Emote emote)
        {
            var validConditions = emote.conditions
                .Where(condition => AssertParameterExists(condition.parameter));
            foreach (var condition in validConditions)
            {
                transition.AddCondition(condition.AnimatorConditionMode, condition.threshold, condition.parameter);
            }
        }

        protected static void ApplyEmoteGestureConditions(AnimatorStateTransition transition, bool isLeft, EmoteGestureCondition gesture, bool mustEmitSomething = false)
        {
            if (gesture.mode != GestureConditionMode.Ignore)
            {
                transition.AddCondition(gesture.ResolveMode(), gesture.ResolveThreshold(), gesture.ResolveParameter(isLeft));
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
        }
    }
}