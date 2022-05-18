using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class TrackingControlLayerBuilder : LayerBuilderBase
    {
        public TrackingControlLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer) : base(builder, layer) { }

        public void Build(TrackingTarget target, IEnumerable<AnimatorStateTransition> overriders)
        {
            foreach (var sourceTransition in overriders)
            {
                var transition = AddStateAsTransition(sourceTransition.destinationState.name, null);
                foreach (var sourceCondition in sourceTransition.conditions)
                {
                    transition.AddCondition(sourceCondition.mode, sourceCondition.threshold, sourceCondition.parameter);
                }
                transition.hasExitTime = sourceTransition.hasExitTime;
                transition.duration = sourceTransition.duration;
                transition.canTransitionToSelf = true;

                PopulateTrackingControl(transition, target, VRC_AnimatorTrackingControl.TrackingType.Animation);
            }

            var defaultTransition = AddStateAsTransition("Default", null);
            defaultTransition.AddAlwaysTrueCondition();

            defaultTransition.hasExitTime = false;
            defaultTransition.duration = 0.1f;
            defaultTransition.canTransitionToSelf = false;

            PopulateTrackingControl(defaultTransition, target, VRC_AnimatorTrackingControl.TrackingType.Tracking);

            LegacyStateMachine.defaultState = LegacyStateMachine.states.FirstOrDefault().state;
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
                    break;
                case TrackingTarget.Mouth:
                    trackingControl.trackingMouth = value; 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}