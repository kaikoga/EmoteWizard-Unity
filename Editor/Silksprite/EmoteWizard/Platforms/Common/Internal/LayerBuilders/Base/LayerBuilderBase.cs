using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using Silksprite.EmoteWizard.Platforms.Common.Internal.ConditionBuilders;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders.Base
{
    public abstract class LayerBuilderBase
    {
        protected readonly AnimatorLayerBuilder Builder;
        readonly AnimatorControllerLayer _layer;

        protected EmoteWizardEnvironment Environment => Builder.Environment;
        protected IEditorPlatformFeatures EditorFeatures => Builder.EditorFeatures;
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

        protected bool TryResolveParameterWithType(string parameterName, ParameterItemKind itemKind, out ParameterValueKind actualValueKind)
        {
            return Builder.ParametersSnapshot.TryResolveParameterWithTypeAndWarning(parameterName, itemKind, out _, out actualValueKind);
        }

        protected LayerBuilderBase(AnimatorLayerBuilder builder, AnimatorControllerLayer layer)
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

        AnimatorState AddStateWithoutTransition(string stateName, Motion? motion, Vector3 position)
        {
            if (motion == null) motion = Environment.ProvideEmptyClip();
            var state = StateMachine.AddState(stateName, position);
            state.motion = motion;
            state.writeDefaultValues = false;
            Builder.MarkParameter(motion);
            return state;
        }

        protected AnimatorState AddStateWithoutTransition(string stateName, Motion? motion) => AddStateWithoutTransition(stateName, motion, NextStatePosition());

        private protected AnimatorTransition AddEntryTransition(AnimatorState toState, ConditionBuilder? conditions = null)
        {
            var transition = StateMachine.AddEntryTransition(toState);
            transition.conditions = conditions?.ToArray();
            return transition;
        }

        private protected AnimatorStateTransition AddTransition(AnimatorState fromState, AnimatorState toState, ConditionBuilder? conditions = null)
        {
            var transition = fromState.AddTransition(toState);
            transition.conditions = conditions?.ToArray();
            return transition;
        }

        private protected IEnumerable<AnimatorStateTransition> AddTransitions(AnimatorState fromState, AnimatorState toState, IEnumerable<ConditionBuilder> conditions)
        {
            return conditions.Select(cond => AddTransition(fromState, toState, cond)).ToArray();
        }

        private protected AnimatorStateTransition AddExitTransition(AnimatorState fromState, ConditionBuilder? conditions = null)
        {
            var transition = fromState.AddExitTransition(false);
            transition.conditions = conditions?.ToArray();
            return transition;
        }

        private protected IEnumerable<AnimatorStateTransition> AddExitTransitions(AnimatorState fromState, IEnumerable<ConditionBuilder> conditions)
        {
            return conditions.Select(cond => AddExitTransition(fromState, cond)).ToArray();
        }

        protected AnimatorState PopulateDefaultState(string stateName = "Default", Motion? clip = null)
        {
            var defaultState = AddStateWithoutTransition(stateName, clip, NextStatePosition());
            StateMachine.defaultState = defaultState;
            NextStateRow();
            return defaultState;
        }

        private protected void ApplyEmoteConditions(ConditionBuilder conditions, IEnumerable<EmoteConditionInstance> emoteConditions)
        {
            foreach (var condition in emoteConditions)
            {
                conditions.EmoteCondition(Environment, condition);
                Builder.MarkParameter(condition.Parameter);
            }
        }

        protected void PopulateLayerControl(AnimatorState state, float goalWeight, float duration)
        {
            EditorFeatures.PopulateLayerControl(state, goalWeight, duration);
        }
    }
}