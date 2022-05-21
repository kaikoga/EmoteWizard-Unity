using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class HandSignLayerBuilder : LayerBuilderBase
    {
        readonly bool _isLeft;
        readonly bool _isAdvanced;

        public HandSignLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer, bool isLeft, bool isAdvanced) : base(builder, layer)
        {
            _isLeft = isLeft;
            _isAdvanced = isAdvanced;
        }

        protected override void Process()
        {
            AnimatorState GenerateStateFromEmote(Emote emote)
            {
                var clip = _isLeft || !_isAdvanced ? emote.clipLeft : emote.clipRight;
                var state = AddStateWithoutTransition(emote.ToStateName(), clip);
                return state;
            }

            var defaultState = PopulateDefaultState();

            var emotes = AnimationWizardBase.CollectEmotes().ToList();

            if (AnimationWizardBase.handSignOverrideEnabled)
            {
                Builder.MarkParameter(AnimationWizardBase.HandSignOverrideParameter);
                foreach (var emote in emotes)
                {
                    if (!emote.OverrideAvailable) continue;
                    var state = GenerateStateFromEmote(emote);
                    var conditions = ConditionBuilder.EqualsCondition(AnimationWizardBase.HandSignOverrideParameter, emote.overrideIndex);
                    var transition = AddSelectTransition(defaultState, state, conditions);

                    ApplyEmoteControl(transition, _isLeft, emote.control);
                }
            }

            foreach (var emote in emotes)
            {
                var conditions = new ConditionBuilder();
                var state = GenerateStateFromEmote(emote);
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