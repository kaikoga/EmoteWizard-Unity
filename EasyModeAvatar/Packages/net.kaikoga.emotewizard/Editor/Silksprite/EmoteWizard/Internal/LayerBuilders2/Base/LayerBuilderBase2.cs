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

namespace Silksprite.EmoteWizard.Internal.LayerBuilders2.Base
{
    public abstract class LayerBuilderBase2
    {
        protected readonly AnimatorLayerBuilder Builder;
        readonly AnimatorControllerLayer _layer;

        protected AnimatorLayerWizardBase Wizard => Builder.Wizard;
        AnimatorStateMachine StateMachine => _layer.stateMachine;

        Vector3 _position = new Vector3(0f, 0f, 0f);
        protected Vector3 NextStatePosition()
        {
            var result = _position;
            _position.x += 300f;
            return result;
        }

        protected void NextStateRow()
        {
            _position.x = 0f;
            _position.y += 75f;
        }

        protected bool AssertParameterExists(string parameterName, ParameterItemKind itemKind)
        {
            return Builder.ParametersWizard == null || Builder.ParametersWizard.AssertParameterExists(parameterName, itemKind);
        }

        protected LayerBuilderBase2(AnimatorLayerBuilder builder, AnimatorControllerLayer layer)
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

        AnimatorState AddStateWithoutTransition(string stateName, Motion motion, Vector3 position, bool isReallyEmptyState)
        {
            if (motion == null && !isReallyEmptyState) motion = Wizard.EmoteWizardRoot.ProvideEmptyClip();
            var state = StateMachine.AddState(stateName, position);
            state.motion = motion;
            state.writeDefaultValues = false;
            Builder.MarkParameter(motion);
            return state;
        }

        protected AnimatorState AddStateWithoutTransition(string stateName, Motion motion) => AddStateWithoutTransition(stateName, motion, NextStatePosition(), false);

        protected AnimatorTransition AddEntryTransition(AnimatorState toState, ConditionBuilder conditions = null)
        {
            var transition = StateMachine.AddEntryTransition(toState);
            transition.conditions = conditions?.ToArray();
            return transition;
        }

        protected AnimatorStateTransition AddTransition(AnimatorState fromState, AnimatorState toState, ConditionBuilder conditions = null)
        {
            var transition = fromState.AddTransition(toState);
            transition.conditions = conditions?.ToArray();
            return transition;
        }

        protected void AddTransitions(AnimatorState fromState, AnimatorState toState, IEnumerable<ConditionBuilder> conditions)
        {
            foreach (var cond in conditions) AddTransition(fromState, toState, cond);
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
            var defaultState = AddStateWithoutTransition(stateName, clip, NextStatePosition(), false);
            StateMachine.defaultState = defaultState;
            NextStateRow();
            return defaultState;
        }

        protected AnimatorStateTransition AddConditionTransition(AnimatorState defaultState, AnimatorState state, ConditionBuilder conditions)
        {
            var transition = AddTransition(defaultState, state, conditions);
            AddExitTransitions(state, conditions.Inverse());
            return transition;
        }

        protected AnimatorStateTransition AddInverseConditionTransition(AnimatorState defaultState, AnimatorState state, ConditionBuilder conditions)
        {
            var transition = AddTransition(defaultState, state, conditions);
            AddExitTransitions(state, conditions.Inverse());
            return transition;
        }

        protected void ApplyEmoteConditions(ConditionBuilder conditions, IEnumerable<EmoteCondition> emoteConditions)
        {
            var validConditions = emoteConditions
                .Where(emoteCondition => AssertParameterExists(emoteCondition.parameter, ParameterItemKind.Auto));
            foreach (var condition in validConditions)
            {
                conditions.EmoteCondition(condition);
                Builder.MarkParameter(condition.parameter);
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
        
        protected static void PopulateTrackingControl(AnimatorState state, VRC_AnimatorTrackingControl.TrackingType value)
        {
            var trackingControl = state.AddStateMachineBehaviour<VRCAnimatorTrackingControl>();
            trackingControl.trackingHead = value; 
            trackingControl.trackingLeftHand = value; 
            trackingControl.trackingRightHand = value; 
            trackingControl.trackingHip = value; 
            trackingControl.trackingLeftFoot = value; 
            trackingControl.trackingRightFoot = value; 
            trackingControl.trackingLeftFingers = value; 
            trackingControl.trackingRightFingers = value; 
        }

        protected static void PopulatePlayableLayerControl(AnimatorState state, float goalWeight, float duration)
        {
            var playableLayerControl = state.AddStateMachineBehaviour<VRCPlayableLayerControl>();
            playableLayerControl.layer = VRC_PlayableLayerControl.BlendableLayer.Action;
            playableLayerControl.goalWeight = goalWeight;
            playableLayerControl.blendDuration = duration;
        }

    }
}