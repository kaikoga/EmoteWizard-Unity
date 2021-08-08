using System;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class ParameterLayerBuilder : LayerBuilderBase
    {
        public ParameterLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer) : base(builder, layer) { }

        public void Build(ParameterEmote parameterEmote)
        {
            if (!AssertParameterExists(parameterEmote.parameter)) return;
            Builder.MarkParameter(parameterEmote.parameter);

            switch (parameterEmote.emoteKind)
            {
                case ParameterEmoteKind.Transition:
                    BuildTransitionStateMachine(parameterEmote);
                    break;
                case ParameterEmoteKind.NormalizedTime:
                    BuildNormalizedTimeStateMachine(parameterEmote);
                    break;
                case ParameterEmoteKind.BlendTree:
                    BuildBlendTreeStateMachine(parameterEmote);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            StateMachine.defaultState = StateMachine.states.FirstOrDefault().state;
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
                var transition = AddStateAsTransition(stateName, parameterEmoteState.clip);
                switch (parameterEmote.valueKind)
                {
                    case ParameterValueKind.Int:
                        transition.AddCondition(AnimatorConditionMode.Equals, parameterEmoteState.value, parameterEmote.parameter);
                        break;
                    case ParameterValueKind.Float:
                        if (nextValue is float nextVal)
                        {
                            transition.AddCondition(AnimatorConditionMode.Less, nextVal, parameterEmote.parameter);
                        }
                        else
                        {
                            transition.AddAlwaysTrueCondition();
                        }
                        break;
                    case ParameterValueKind.Bool:
                        transition.AddCondition(parameterEmoteState.value != 0 ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot, parameterEmoteState.value, parameterEmote.parameter);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

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

            var state = AddStateAsTransition(parameterEmote.name, clip).destinationState;

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

            AddStateAsTransition(parameterEmote.name, blendTree);
        }
    }
}