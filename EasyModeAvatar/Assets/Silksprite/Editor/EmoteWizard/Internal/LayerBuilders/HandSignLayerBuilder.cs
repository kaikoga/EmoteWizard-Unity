using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class HandSignLayerBuilder : LayerBuilderBase
    {
        readonly bool _isLeft;

        public HandSignLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer, bool isLeft) : base(builder, layer)
        {
            _isLeft = isLeft;
        }

        protected override void Process()
        {
            AnimatorState GenerateStateFromEmote(Emote emote, bool isOverride)
            {
                Motion clip;
                if (_isLeft)
                {
                    clip = emote.clipLeft ? emote.clipLeft : emote.clipRight;
                }
                else
                {
                    clip = emote.clipRight ? emote.clipRight : emote.clipLeft;
                }

                var state = AddStateWithoutTransition(isOverride ? $"{AnimationWizardBase.HandSignOverrideParameter}={emote.overrideIndex}" : emote.ToStateName(false, true), clip);
                return state;
            }

            var emotes = AnimationWizardBase.CollectEmotes().ToList();
            InitEmoteControl(emotes.SelectMany(emote => emote.control.trackingOverrides));

            var defaultState = PopulateDefaultState();

            if (AnimationWizardBase.handSignOverrideEnabled)
            {
                Builder.MarkParameter(AnimationWizardBase.HandSignOverrideParameter);
                foreach (var emote in emotes)
                {
                    if (!emote.OverrideAvailable) continue;
                    var state = GenerateStateFromEmote(emote, true);
                    var conditions = ConditionBuilder.EqualsCondition(AnimationWizardBase.HandSignOverrideParameter, emote.overrideIndex);
                    var transition = AddSelectTransition(defaultState, state, conditions);

                    ApplyEmoteControl(transition, _isLeft, emote.control);
                }
            }

            foreach (var emote in emotes)
            {
                var conditions = new ConditionBuilder();
                var state = GenerateStateFromEmote(emote, false);
                ApplyEmoteGestureConditions(conditions, _isLeft, emote.gesture1, true);
                ApplyEmoteGestureConditions(conditions, _isLeft, emote.gesture2);
                if (AnimationWizardBase.handSignOverrideEnabled)
                {
                    conditions.Equals(AnimationWizardBase.HandSignOverrideParameter, 0);
                }
                ApplyEmoteConditions(conditions, emote.conditions);
                var transition = AddSelectTransition(defaultState, state, conditions);

                ApplyEmoteControl(transition, _isLeft, emote.control);
            }
        }
    }
}