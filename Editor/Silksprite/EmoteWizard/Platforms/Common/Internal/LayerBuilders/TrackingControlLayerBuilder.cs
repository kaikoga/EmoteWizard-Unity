using System;
using System.Collections.Generic;
using Silksprite.AdLib.ChilloutVR;
using Silksprite.AdLib.ChilloutVR.Access;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using Silksprite.EmoteWizard.Platforms.Common.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Platforms.Common.Internal.Extensions;
using Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders.Base;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders
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
                var state = AddStateWithoutTransition(emoteItem.Trigger.Name, null);
                var conditions = new ConditionBuilder();
                ApplyEmoteConditions(conditions, emoteItem.Trigger.Conditions);
                var transition = AddEntryTransition(state, conditions);

                PopulateBodyControl(transition, _target, 0f);

                AddExitTransition(state, offTriggerConditions); // wait until offTrigger
                // Consume triggers by self transition if current state is already On
                AddTransition(state, state, onTriggerConditions);
                NextStateRow();
            }

            NextStatePosition();
            var trackingState = PopulateDefaultState("Tracking");
            var trackingTransition = AddEntryTransition(trackingState, new ConditionBuilder().AlwaysTrue(Builder.Environment));

            PopulateBodyControl(trackingTransition, _target, 1f);

            AddExitTransition(trackingState, onTriggerConditions);
            // Consume triggers by self transition if current state is already Off
            AddTransition(trackingState, trackingState, offTriggerConditions);
        }
        
        void PopulateBodyControl(AnimatorTransition transition, TrackingTarget target, float targetWeight)
        {
            var bodyControl = transition.destinationState.AddStateMachineBehaviour2Access(CVRTypes.BodyControl.Type, smb => new BodyControlAccess(smb), Builder.IsPersistedAsset);
            void ConfigureEnterTask(BodyControlTask_BodyMaskAccess bodyMask)
            {
                bodyControl.EnterTasks = new List<BodyControlTaskAccess>
                {
                    new BodyControlTaskAccess
                    {
                        target = bodyMask,
                        targetWeight = targetWeight
                    }
                };
            }
            switch (target)
            {
                case TrackingTarget.Head:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.Head.ToAccess()); 
                    break;
                case TrackingTarget.LeftHand:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.LeftArm.ToAccess()); 
                    break;
                case TrackingTarget.RightHand:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.RightArm.ToAccess()); 
                    break;
                case TrackingTarget.Hip:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.Pelvis.ToAccess()); 
                    break;
                case TrackingTarget.LeftFoot:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.LeftLeg.ToAccess()); 
                    break;
                case TrackingTarget.RightFoot:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.RightLeg.ToAccess()); 
                    break;
                case TrackingTarget.LeftFingers:
                case TrackingTarget.RightFingers:
                case TrackingTarget.Eyes:
                case TrackingTarget.Mouth:
                    // not available
                    break;
                default:
                    throw new ArgumentOutOfRangeException(target.ToString());
            }
        }
    }
}