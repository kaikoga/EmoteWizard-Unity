using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using Silksprite.EmoteWizard.Platforms.Common.Internal.ConditionBuilders;
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

                EditorFeatures.PopulateTrackingControl(transition.destinationState, _target, 0f);

                AddExitTransition(state, offTriggerConditions); // wait until offTrigger
                // Consume triggers by self transition if current state is already On
                AddTransition(state, state, onTriggerConditions);
                NextStateRow();
            }

            NextStatePosition();
            var trackingState = PopulateDefaultState("Tracking");
            var trackingTransition = AddEntryTransition(trackingState, new ConditionBuilder().AlwaysTrue(Builder.Environment));

            EditorFeatures.PopulateTrackingControl(trackingTransition.destinationState, _target, 1f);

            AddExitTransition(trackingState, onTriggerConditions);
            // Consume triggers by self transition if current state is already Off
            AddTransition(trackingState, trackingState, offTriggerConditions);
        }
    }
}