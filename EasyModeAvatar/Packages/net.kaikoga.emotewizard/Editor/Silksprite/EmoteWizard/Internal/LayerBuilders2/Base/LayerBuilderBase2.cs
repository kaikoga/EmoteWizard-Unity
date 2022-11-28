using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.ConditionBuilders;
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
            return Builder.ParametersSnapshot == null || Builder.ParametersSnapshot.AssertParameterExists(parameterName, itemKind);
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

        protected static void PopulatePlayableLayerControl(AnimatorState state, float goalWeight, float duration)
        {
            var playableLayerControl = state.AddStateMachineBehaviour<VRCPlayableLayerControl>();
            playableLayerControl.layer = VRC_PlayableLayerControl.BlendableLayer.Action;
            playableLayerControl.goalWeight = goalWeight;
            playableLayerControl.blendDuration = duration;
        }

    }
}