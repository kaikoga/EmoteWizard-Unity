using System;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class ParameterLayerBuilder : LayerBuilderBase
    {
        readonly ParameterEmote _parameterEmote;

        public ParameterLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer, ParameterEmote parameterEmote) : base(builder, layer)
        {
            _parameterEmote = parameterEmote;
        }

        protected override void Process()
        {
            if (!AssertParameterExists(_parameterEmote.parameter)) return;
            Builder.MarkParameter(_parameterEmote.parameter);

            switch (_parameterEmote.emoteKind)
            {
                case ParameterEmoteKind.Transition:
                    BuildTransitionStateMachine(_parameterEmote);
                    break;
                case ParameterEmoteKind.NormalizedTime:
                    BuildNormalizedTimeStateMachine(_parameterEmote);
                    break;
                case ParameterEmoteKind.BlendTree:
                    BuildBlendTreeStateMachine(_parameterEmote);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void BuildTransitionStateMachine(ParameterEmote parameterEmote)
        {
            var validStates = parameterEmote.states.Where(state => state.enabled).ToList();
            var stateAndNextValue = validStates.Zip(
                validStates.Skip(1).Select(state => (float?) state.value).Concat(Enumerable.Repeat((float?) null, 1)),
                (s, v) => (s, v));
            foreach (var (parameterEmoteState, nextValue) in stateAndNextValue)
            {
                var stateName = $"{parameterEmote.parameter} = {parameterEmoteState.value}";
                var state = AddStateWithoutTransition(stateName, parameterEmoteState.clip);
                var transition = AddAnyStateTransition(state);
                var condition = new ConditionBuilder();
                switch (parameterEmote.valueKind)
                {
                    case ParameterValueKind.Int:
                        condition = condition.Equals(parameterEmote.parameter, (int)parameterEmoteState.value);
                        break;
                    case ParameterValueKind.Float:
                        if (nextValue is float nextVal)
                        {
                            condition = condition.Less(parameterEmote.parameter, nextVal);
                        }
                        else
                        {
                            condition = condition.AlwaysTrue();
                        }
                        break;
                    case ParameterValueKind.Bool:
                        {
                            condition = condition.If(parameterEmote.parameter, parameterEmoteState.value != 0);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                transition.AddCondition(condition);

                ApplyEmoteControl(transition, true, parameterEmoteState.control);
            }
        }

        void BuildNormalizedTimeStateMachine(ParameterEmote parameterEmote)
        {
            var clip = parameterEmote.states
                .Where(s => s.enabled)
                .Select(s => s.clip)
                .FirstOrDefault(c => c != null);
            if (clip == null) return;

            var state = AddStateWithoutTransition(parameterEmote.name, clip);
            AddAnyStateTransition(state);

            state.timeParameterActive = true;
            state.timeParameter = parameterEmote.parameter;
            clip.SetLoopTimeRec(false);
            EditorUtility.SetDirty(clip);
        }

        void BuildBlendTreeStateMachine(ParameterEmote parameterEmote)
        {
            var path = GeneratedAssetLocator.ParameterEmoteBlendTreePath(Builder.AnimationWizardBase.LayerName, parameterEmote.name);
            var blendTree = Builder.AnimationWizardBase.EmoteWizardRoot.EnsureAsset<BlendTree>(path);

            blendTree.blendParameter = parameterEmote.parameter;
            blendTree.blendType = BlendTreeType.Simple1D;
            blendTree.useAutomaticThresholds = false;
            var validStates = parameterEmote.states.Where(state => state.enabled);
            blendTree.children = validStates.Select(state => new ChildMotion
            {
                cycleOffset = 0,
                directBlendParameter = null,
                mirror = false,
                motion = state.clip,
                position = default,
                threshold = state.value,
                timeScale = 1
            }).ToArray();

            var blendTreeState = AddStateWithoutTransition(parameterEmote.name, blendTree);
            AddAnyStateTransition(blendTreeState);
        }
    }
}