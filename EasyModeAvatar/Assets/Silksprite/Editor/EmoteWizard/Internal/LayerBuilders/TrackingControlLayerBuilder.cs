using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class TrackingControlLayerBuilder : LayerBuilderBase
    {
        readonly TrackingTarget _target;
        readonly IEnumerable<AnimatorStateTransition> _overriders;

        public TrackingControlLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer, TrackingTarget target, IEnumerable<AnimatorStateTransition> overriders) : base(builder, layer)
        {
            _target = target;
            _overriders = overriders;
        }

        protected override void Process()
        {
            var defaultState = PopulateDefaultState();

            foreach (var sourceTransition in _overriders)
            {
                var state = AddStateWithoutTransition(sourceTransition.destinationState.name, null);
                var transition = AddTransitionAndCopyConditions(defaultState, state, sourceTransition.conditions);
                transition.hasExitTime = false;
                transition.duration = sourceTransition.duration;

                PopulateTrackingControl(transition, _target, VRC_AnimatorTrackingControl.TrackingType.Animation);
                AddExitTransition(state, new ConditionBuilder().If(_target.ToAnimatorParameterName(false), true));
            }

            var trackingState = AddStateWithoutTransition("Tracking", null);
            var trackingTransition = AddTransition(defaultState, trackingState, new ConditionBuilder().AlwaysTrue());

            trackingTransition.hasExitTime = false;
            trackingTransition.duration = 0.1f;

            PopulateTrackingControl(trackingTransition, _target, VRC_AnimatorTrackingControl.TrackingType.Tracking);
            AddExitTransition(trackingState, new ConditionBuilder().If(_target.ToAnimatorParameterName(true), true));
            // FIXME: consume On / Off triggers if if tracking override state is already On / Off
        }
        
        static void PopulateTrackingControl(AnimatorStateTransition transition, TrackingTarget target, VRC_AnimatorTrackingControl.TrackingType value)
        {
            var trackingControl = transition.destinationState.AddStateMachineBehaviour<VRCAnimatorTrackingControl>();
            switch (target)
            {
                case TrackingTarget.Head:
                    trackingControl.trackingHead = value; 
                    break;
                case TrackingTarget.LeftHand:
                    trackingControl.trackingLeftHand = value; 
                    break;
                case TrackingTarget.RightHand:
                    trackingControl.trackingRightHand = value; 
                    break;
                case TrackingTarget.Hip:
                    trackingControl.trackingHip = value; 
                    break;
                case TrackingTarget.LeftFoot:
                    trackingControl.trackingLeftFoot = value; 
                    break;
                case TrackingTarget.RightFoot:
                    trackingControl.trackingRightFoot = value; 
                    break;
                case TrackingTarget.LeftFingers:
                    trackingControl.trackingLeftFingers = value; 
                    break;
                case TrackingTarget.RightFingers:
                    trackingControl.trackingRightFingers = value; 
                    break;
                case TrackingTarget.Eyes:
                    trackingControl.trackingEyes = value; 
                    // TODO: Reset blink blend shape states (if any)
                    break;
                case TrackingTarget.Mouth:
                    trackingControl.trackingMouth = value; 
                    // TODO: Reset lip sync blend shape states (if any) (should we?)
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}