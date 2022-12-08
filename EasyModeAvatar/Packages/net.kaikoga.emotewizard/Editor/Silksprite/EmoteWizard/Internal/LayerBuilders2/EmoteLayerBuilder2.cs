using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Internal.LayerBuilders2.Base;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders2
{
    public class EmoteLayerBuilder2 : LayerBuilderBase2
    {
        readonly IEnumerable<EmoteItem> _emoteItems;

        public EmoteLayerBuilder2(AnimatorLayerBuilder builder, AnimatorControllerLayer layer, IEnumerable<EmoteItem> emoteItems) : base(builder, layer)
        {
            _emoteItems = emoteItems.ToArray();
        }

        protected override void Process()
        {
            AnimatorState defaultState = null;
            if (_emoteItems.Any(item => item.sequence.entryTransitionDuration != 0f))
            {
                defaultState = PopulateDefaultState();
            }
            var currentTrackingTargets = _emoteItems.SelectMany(item => item.sequence.trackingOverrides).Select(trackingOverride => trackingOverride.target).Distinct().ToArray();
            var currentForcedConditions = new List<List<EmoteCondition>>();
            foreach (var priority in _emoteItems.OrderBy(item => item.trigger.priority).GroupBy(item => item.trigger.priority))
            {
                foreach (var emoteItem in priority)
                {
                    PopulateSequence(emoteItem, defaultState, currentTrackingTargets, currentForcedConditions);
                }

                foreach (var emoteItem in priority)
                {
                    currentForcedConditions = MergeForcedConditions(currentForcedConditions, emoteItem.trigger.conditions);
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

        void PopulateSequence(EmoteItem emoteItem, AnimatorState defaultState, TrackingTarget[] currentTrackingTargets, List<List<EmoteCondition>> currentForcedConditions)
        {
            void AddTrackingParameterDrivers(AnimatorState state, bool isEntry)
            {
                var avatarParameterDriver = state.AddStateMachineBehaviour<VRCAvatarParameterDriver>();
                avatarParameterDriver.localOnly = true;
                var targets = emoteItem.sequence.trackingOverrides.Select(trackingOverride => trackingOverride.target).ToArray();
                foreach (var target in targets) Builder.MarkTrackingTarget(target);

                avatarParameterDriver.parameters = targets.Select(target => new VRC_AvatarParameterDriver.Parameter
                {
                    name = target.ToAnimatorParameterName(isEntry && currentTrackingTargets.Contains(target)),
                    value = 0f,
                    valueMin = 0f,
                    valueMax = 0f,
                    chance = 1f,
                    type = VRC_AvatarParameterDriver.ChangeType.Set
                }).ToList();
            }

            NextStateRow();
            NextStatePosition();

            var sequence = emoteItem.sequence;
            // emoteItem.sequence.clip.SetLoopTimeRec(!emoteItem.sequence.hasExitTime);

            var conditions = new ConditionBuilder();
            ApplyEmoteConditions(conditions, emoteItem.trigger.conditions);

            AnimatorState entryState = null;
            AnimatorState mainState = null;
            AnimatorState exitState = null;
            AnimatorState releaseState = null;
            
            if (sequence.hasEntryClip)
            {
                entryState = AddStateWithoutTransition($"Entry {emoteItem.trigger.name}", sequence.entryClip);
            }
            else
            {
                NextStatePosition();
            }

            mainState = AddStateWithoutTransition(emoteItem.trigger.name, sequence.clip);

            if (sequence.hasExitClip)
            {
                exitState = AddStateWithoutTransition($"Exit {emoteItem.trigger.name}", sequence.exitClip);
            }
            else
            {
                NextStatePosition();
            }
            
            if (sequence.hasTrackingOverrides)
            {
                releaseState = AddStateWithoutTransition($"Release {emoteItem.trigger.name}", null);
                AddTrackingParameterDrivers(entryState ? entryState : mainState, true);
                AddTrackingParameterDrivers(releaseState, false);
            }
            
            if (!sequence.hasExitTime)
            {
                if (sequence.hasTimeParameter && ResolveParameterType(sequence.timeParameter, ParameterItemKind.Float) == ParameterValueKind.Float)
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
                var firstState = entryState ? entryState : mainState;
                var lastState = releaseState ? releaseState : exitState ? exitState : mainState;
                PopulatePlayableLayerControl(firstState, 1f, sequence.blendIn);
                PopulatePlayableLayerControl(lastState, 0f, sequence.blendOut);
            }

            if (entryState)
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
                if (exitState)
                {
                    exitTransition = AddTransition(mainState, exitState);
                }
                else if (releaseState)
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
                if (exitState)
                {
                    exitTransitions = AddTransitions(mainState, exitState, conditions.Inverse().Concat(forcedConditions));
                }
                else if (releaseState)
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

            if (exitState)
            {
                var postExitTransition = releaseState ? AddTransition(exitState, releaseState) : AddExitTransition(exitState);
                postExitTransition.hasExitTime = true;
                postExitTransition.exitTime = sequence.exitClipExitTime;
                postExitTransition.duration = sequence.postExitTransitionDuration;
                postExitTransition.hasFixedDuration = sequence.isFixedDuration;
            }

            if (releaseState)
            {
                var postReleaseTransition = AddExitTransition(releaseState);
                postReleaseTransition.hasExitTime = true;
                postReleaseTransition.exitTime = 0f;
                postReleaseTransition.duration = 0f;
            }
        }
        
        List<List<EmoteCondition>> MergeForcedConditions(List<List<EmoteCondition>> currentForcedConditions, List<EmoteCondition> conditions)
        {
            currentForcedConditions.Add(conditions);

            // quick and dirty optimization starts here
            // TODO: how about optimizing multiple conditions
            if (conditions.Count == 1 && conditions[0].kind != ParameterItemKind.Float)
            {
                var parameterName = conditions[0].parameter;
                var parameter = Builder.ParametersSnapshot.ResolveParameter(parameterName);
                switch (parameter.ValueKind)
                {
                    case ParameterValueKind.Int:
                    {
                        var readUsages = parameter.ReadUsages;
                        var equalConditions = currentForcedConditions.Where(cond => cond.Count == 1)
                            .Where(cond => cond[0].parameter == parameterName && cond[0].mode == EmoteConditionMode.Equals).ToArray();
                        var values = equalConditions.Select(cond => cond[0].threshold);
                        var elseValues = readUsages.Select(usage => usage.Value).Where(value => !values.Contains(value)).ToArray();
                        if (elseValues.Length == 1)
                        {
                            currentForcedConditions = currentForcedConditions.Where(cond => !equalConditions.Contains(cond)).ToList();
                            currentForcedConditions.Add(new List<EmoteCondition>
                            {
                                new EmoteCondition
                                {
                                    kind = conditions[0].kind,
                                    parameter = conditions[0].parameter,
                                    mode = EmoteConditionMode.NotEqual,
                                    threshold = elseValues[0]
                                }
                            });
                        }
                        break;
                    }
                    case ParameterValueKind.Bool:
                    {
                        var boolConditions = currentForcedConditions.Where(cond => cond.Count == 1)
                            .Where(cond => cond[0].parameter == parameterName).ToArray();
                        var values = boolConditions.Select(cond =>
                        {
                            switch (cond[0].mode)
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
                                    return cond[0].threshold != 0 ? EmoteConditionMode.If : EmoteConditionMode.IfNot;
                                case EmoteConditionMode.NotEqual:
                                    return cond[0].threshold == 0 ? EmoteConditionMode.If : EmoteConditionMode.IfNot;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }).Distinct().ToArray();
                        if (values.Length == 1)
                        {
                            var combinedMode = values[0];
                            currentForcedConditions = currentForcedConditions.Where(cond => !boolConditions.Contains(cond)).ToList();
                            currentForcedConditions.Add(new List<EmoteCondition>
                            {
                                new EmoteCondition
                                {
                                    kind = ParameterItemKind.Bool,
                                    parameter = conditions[0].parameter,
                                    mode = combinedMode,
                                    threshold = 0
                                }
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