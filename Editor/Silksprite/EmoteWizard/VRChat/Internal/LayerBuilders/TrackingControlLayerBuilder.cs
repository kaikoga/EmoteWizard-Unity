using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Internal.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class TrackingControlLayerBuilder : LayerBuilderBase
    {
        readonly TrackingTarget _target;
        readonly IEnumerable<EmoteItem> _overriders;

        public TrackingControlLayerBuilder(AnimatorLayerBuilder builder, AnimatorControllerLayer layer, TrackingTarget target, IEnumerable<EmoteItem> overriders) : base(builder, layer)
        {
            _target = target;
            _overriders = overriders;
        }

        protected override void Process()
        {
            Builder.MarkTrackingTarget(_target);

            var offTriggerConditions = new ConditionBuilder().If(_target.ToAnimatorParameterName(false), true);
            var onTriggerConditions = new ConditionBuilder().If(_target.ToAnimatorParameterName(true), true);

            foreach (var emoteItem in _overriders)
            {
                NextStatePosition();
                var state = AddStateWithoutTransition(emoteItem.Trigger.name, null);
                var conditions = new ConditionBuilder();
                ApplyEmoteConditions(conditions, emoteItem.Trigger.conditions);
                var transition = AddEntryTransition(state, conditions);

                PopulateTrackingControl(transition, _target, VRC_AnimatorTrackingControl.TrackingType.Animation);

                AddExitTransition(state, offTriggerConditions); // wait until offTrigger
                // Consume triggers by self transition if current state is already On
                AddTransition(state, state, onTriggerConditions);
                NextStateRow();
            }

            NextStatePosition();
            var trackingState = PopulateDefaultState("Tracking");
            var trackingTransition = AddEntryTransition(trackingState, new ConditionBuilder().AlwaysTrue());

            PopulateTrackingControl(trackingTransition, _target, VRC_AnimatorTrackingControl.TrackingType.Tracking);

            AddExitTransition(trackingState, onTriggerConditions);
            // Consume triggers by self transition if current state is already Off
            AddTransition(trackingState, trackingState, offTriggerConditions);
        }
        
        void PopulateTrackingControl(AnimatorTransition transition, TrackingTarget target, VRC_AnimatorTrackingControl.TrackingType value)
        {
            var trackingControl = transition.destinationState.AddStateMachineBehaviour2<VRCAnimatorTrackingControl>(Builder);
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
                    throw new ArgumentOutOfRangeException(target.ToString());
            }
        }
    }
}