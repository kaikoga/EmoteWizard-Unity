using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using Silksprite.EmoteWizard.Platforms.Common.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders.Base;
using Silksprite.EmoteWizard.Platforms.Extensions;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders
{
    public class EmoteLayerBuilder : LayerBuilderBase
    {
        readonly IEnumerable<EmoteInstance> _emoteInstances;

        public EmoteLayerBuilder(AnimatorLayerBuilder builder, AnimatorControllerLayer layer, IEnumerable<EmoteInstance> emoteInstances) : base(builder, layer)
        {
            _emoteInstances = emoteInstances.ToArray();
        }

        protected override void Process()
        {
            AnimatorState? defaultState = null;
            if (_emoteInstances.Any(item => item.Sequence.entryTransitionDuration != 0f))
            {
                defaultState = PopulateDefaultState();
            }
            var currentTrackingTargets = _emoteInstances.SelectMany(instance => instance.Sequence.trackingOverrides).Select(trackingOverride => trackingOverride.target).Distinct().ToArray();
            var currentForcedConditions = new List<List<EmoteConditionInstance>>();
            foreach (var priority in _emoteInstances.OrderBy(instance => instance.Trigger.Priority).GroupBy(instance => instance.Trigger.Priority))
            {
                foreach (var emoteInstance in priority)
                {
                    PopulateSequence(emoteInstance, defaultState, currentTrackingTargets, currentForcedConditions);
                }

                foreach (var emoteInstance in priority)
                {
                    currentForcedConditions = MergeForcedConditions(currentForcedConditions, emoteInstance.Trigger.Conditions);
                }
            }

            if (defaultState == null) PopulateDefaultSequence();
        }

        void PopulateDefaultSequence()
        {
            NextStateRow();
            NextStatePosition();
            NextStatePosition();

            var defaultState = PopulateDefaultState();
            var exitDefaultTransition = AddExitTransition(defaultState);
            exitDefaultTransition.hasExitTime = true;
            exitDefaultTransition.exitTime = 0f;
            exitDefaultTransition.duration = 0f;
        }

        void PopulateSequence(EmoteInstance emoteInstance, AnimatorState? defaultState, TrackingTarget[] currentTrackingTargets, List<List<EmoteConditionInstance>> currentForcedConditions)
        {
            var targets = emoteInstance.Sequence.trackingOverrides.Select(trackingOverride => trackingOverride.target).ToArray();
            foreach (var target in targets) Builder.MarkTrackingTarget(target);

            NextStateRow();
            NextStatePosition();

            var sequence = emoteInstance.Sequence;
            // emoteItem.sequence.clip.SetLoopTimeRec(!emoteItem.sequence.hasExitTime);

            var conditions = new ConditionBuilder();
            ApplyEmoteConditions(conditions, emoteInstance.Trigger.Conditions);

            AnimatorState? entryState = null;
            AnimatorState mainState;
            AnimatorState? exitState = null;
            AnimatorState? releaseState = null;
            
            if (sequence.hasEntryClip)
            {
                entryState = AddStateWithoutTransition($"Entry {emoteInstance.Trigger.Name}", sequence.entryClip);
            }
            else
            {
                NextStatePosition();
            }

            mainState = AddStateWithoutTransition(emoteInstance.Trigger.Name, sequence.clip);

            if (sequence.hasExitClip)
            {
                exitState = AddStateWithoutTransition($"Exit {emoteInstance.Trigger.Name}", sequence.exitClip);
            }
            else
            {
                NextStatePosition();
            }
            
            if (sequence.hasTrackingOverrides)
            {
                releaseState = AddStateWithoutTransition($"Release {emoteInstance.Trigger.Name}", null);
                EditorFeatures.PopulateTriggerDriver(entryState != null ? entryState : mainState,
                    currentTrackingTargets.Select(target => (target, targets.Contains(target) ? TrackingMode.Override : TrackingMode.Tracking)));
                EditorFeatures.PopulateTriggerDriver(releaseState,
                    currentTrackingTargets.Select(target => (target, TrackingMode.Tracking)));
            }
            
            if (!sequence.hasExitTime)
            {
                if (sequence.hasTimeParameter
                    && TryResolveParameterWithType(sequence.timeParameter, ParameterItemKind.Float, out var actualValueKind)
                    && actualValueKind == ParameterValueKind.Float)
                {
                    mainState.timeParameterActive = true;
                    mainState.timeParameter = sequence.timeParameter;
                    Builder.MarkParameter(sequence.timeParameter);
                }
                if (mainState.motion != null)
                {
                    mainState.motion.SetLoopTimeRec(!sequence.hasTimeParameter);
                }
            }

            if (sequence.hasLayerBlend)
            {
                var firstState = entryState != null ? entryState : mainState;
                var lastState = releaseState != null ? releaseState : exitState != null ? exitState : mainState;
                PopulateLayerControl(firstState, 1f, sequence.blendIn);
                PopulateLayerControl(lastState, 0f, sequence.blendOut);
            }

            if (entryState != null)
            {
                if (defaultState == null)
                {
                    AddEntryTransition(entryState, conditions);
                }
                else
                {
                    var entryTransition = AddTransition(defaultState, entryState, conditions);
                    entryTransition.hasExitTime = false;
                    entryTransition.exitTime = 0f;
                    entryTransition.duration = sequence.entryTransitionDuration;
                    entryTransition.hasFixedDuration = sequence.isFixedDuration;
                }
                var postEntryTransition = AddTransition(entryState, mainState);
                postEntryTransition.hasExitTime = true;
                postEntryTransition.exitTime = sequence.entryClipExitTime;
                postEntryTransition.duration = sequence.postEntryTransitionDuration;
                postEntryTransition.hasFixedDuration = sequence.isFixedDuration;
            }
            else
            {
                if (defaultState == null)
                {
                    AddEntryTransition(mainState, conditions);
                }
                else
                {
                    var entryTransition = AddTransition(defaultState, mainState, conditions);
                    entryTransition.hasExitTime = false;
                    entryTransition.exitTime = 0f;
                    entryTransition.duration = sequence.entryTransitionDuration;
                    entryTransition.hasFixedDuration = sequence.isFixedDuration;
                }
            }

            if (sequence.hasExitTime)
            {
                AnimatorStateTransition exitTransition;
                if (exitState != null)
                {
                    exitTransition = AddTransition(mainState, exitState);
                }
                else if (releaseState != null)
                {
                    exitTransition = AddTransition(mainState, releaseState);
                }
                else
                {
                    exitTransition = AddExitTransition(mainState);
                }
                exitTransition.hasExitTime = true;
                exitTransition.exitTime = sequence.clipExitTime;
                exitTransition.duration = sequence.exitTransitionDuration;
                exitTransition.hasFixedDuration = sequence.isFixedDuration;
            }
            else
            {
                var forcedConditions = currentForcedConditions.Select(forcedCondition =>
                {
                    var condition = new ConditionBuilder(); 
                    ApplyEmoteConditions(condition, forcedCondition);
                    return condition;
                });

                IEnumerable<AnimatorStateTransition> exitTransitions;
                if (exitState != null)
                {
                    exitTransitions = AddTransitions(mainState, exitState, conditions.Inverse().Concat(forcedConditions));
                }
                else if (releaseState != null)
                {
                    exitTransitions = AddTransitions(mainState, releaseState, conditions.Inverse().Concat(forcedConditions));
                }
                else
                {
                    exitTransitions = AddExitTransitions(mainState, conditions.Inverse().Concat(forcedConditions));
                }

                foreach (var exitTransition in exitTransitions)
                {
                    exitTransition.hasExitTime = false;
                    exitTransition.exitTime = sequence.clipExitTime;
                    exitTransition.duration = sequence.exitTransitionDuration;
                    exitTransition.hasFixedDuration = sequence.isFixedDuration;
                }
            }

            if (exitState != null)
            {
                var postExitTransition = releaseState != null ? AddTransition(exitState, releaseState) : AddExitTransition(exitState);
                postExitTransition.hasExitTime = true;
                postExitTransition.exitTime = sequence.exitClipExitTime;
                postExitTransition.duration = sequence.postExitTransitionDuration;
                postExitTransition.hasFixedDuration = sequence.isFixedDuration;
            }

            if (releaseState != null)
            {
                var postReleaseTransition = AddExitTransition(releaseState);
                postReleaseTransition.hasExitTime = true;
                postReleaseTransition.exitTime = 0f;
                postReleaseTransition.duration = 0f;
            }
        }

        List<List<EmoteConditionInstance>> MergeForcedConditions(List<List<EmoteConditionInstance>> currentForcedConditions, List<EmoteConditionInstance> conditions)
        {
            var platformFeatures = Environment.GetPlatformFeatures();
            currentForcedConditions.Add(conditions);

            // quick and dirty optimization starts here
            // TODO: how about optimizing multiple conditions
            if (conditions.Count == 1 && conditions[0].Kind != ParameterItemKind.Float)
            {
                var parameterName = conditions[0].Parameter;
                Builder.ParametersSnapshot.TryResolveParameterWithTypeAndWarning(parameterName, conditions[0].Kind, out var parameter, out var actualValueKind);
                switch (actualValueKind)
                {
                    case ParameterValueKind.Int:
                    {
                        var readUsages = parameter.ReadUsages;
                        var equalConditions = currentForcedConditions.Where(cond => cond.Count == 1)
                            .Where(cond => cond[0].Parameter == parameterName && cond[0].Mode == EmoteConditionMode.Equals).ToArray();
                        var values = equalConditions.Select(cond => cond[0].Value.AsInt(platformFeatures)).ToArray();
                        var elseValues = readUsages.Select(usage => usage.Value.AsInt(platformFeatures)).Where(value => !values.Contains(value)).ToArray();
                        if (elseValues.Length == 1)
                        {
                            currentForcedConditions = currentForcedConditions.Where(cond => !equalConditions.Contains(cond)).ToList();
                            currentForcedConditions.Add(new List<EmoteConditionInstance>
                            {
                                new EmoteConditionInstance(
                                    kind: conditions[0].Kind,
                                    parameter: conditions[0].Parameter,
                                    mode: EmoteConditionMode.NotEqual,
                                    value: ParameterValue.Create(ParameterItemKind.Int, elseValues[0])
                                )
                            });
                        }
                        break;
                    }
                    case ParameterValueKind.Bool:
                    {
                        var boolConditions = currentForcedConditions.Where(cond => cond.Count == 1)
                            .Where(cond => cond[0].Parameter == parameterName).ToArray();
                        var values = boolConditions.Select(cond =>
                        {
                            switch (cond[0].Mode)
                            {
                                case EmoteConditionMode.If:
                                    return EmoteConditionMode.If;
                                case EmoteConditionMode.IfNot:
                                    return EmoteConditionMode.IfNot;
                                case EmoteConditionMode.Greater:
                                    return EmoteConditionMode.IfNot;
                                case EmoteConditionMode.Less:
                                    return EmoteConditionMode.IfNot;
                                case EmoteConditionMode.Equals:
                                    return cond[0].Value.IsDefault ? EmoteConditionMode.IfNot : EmoteConditionMode.If;
                                case EmoteConditionMode.NotEqual:
                                    return cond[0].Value.IsDefault ? EmoteConditionMode.If : EmoteConditionMode.IfNot;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }).Distinct().ToArray();
                        if (values.Length == 1)
                        {
                            var combinedMode = values[0];
                            currentForcedConditions = currentForcedConditions.Where(cond => !boolConditions.Contains(cond)).ToList();
                            currentForcedConditions.Add(new List<EmoteConditionInstance>
                            {
                                new EmoteConditionInstance(
                                    kind: ParameterItemKind.Bool,
                                    parameter: conditions[0].Parameter,
                                    mode: combinedMode,
                                    value: ParameterValue.Default
                                )
                            });
                        }

                        break;
                    }
                }
            }

            // quick and dirty optimization ends here

            return currentForcedConditions;
        }
    }
}