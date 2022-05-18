using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Extensions;
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
            var emotes = AnimationWizardBase.CollectEmotes();

            var emoteStates = emotes.Select(emote =>
            {
                var clip = _isLeft || !_isAdvanced ? emote.clipLeft : emote.clipRight;
                if (clip == null) clip = AnimationWizardBase.EmoteWizardRoot.ProvideEmptyClip();
                var state = AddStateWithoutTransition(emote.ToStateName(), clip);
                return (emote, state);
            }).ToList();

            if (AnimationWizardBase.handSignOverrideEnabled)
            {
                Builder.MarkParameter(AnimationWizardBase.HandSignOverrideParameter);
                foreach (var (emote, state) in emoteStates)
                {
                    if (!emote.OverrideAvailable) continue;
                    var transition = AddAnyStateTransition(state);
                    var condition = ConditionBuilder.EqualsCondition(AnimationWizardBase.HandSignOverrideParameter, emote.overrideIndex);
                    transition.AddCondition(condition);

                    ApplyEmoteControl(transition, _isLeft, emote.control);
                }
            }

            foreach (var (emote, state) in emoteStates)
            {
                var transition = AddAnyStateTransition(state);
                ApplyEmoteGestureConditions(transition, _isLeft, emote.gesture1, true);
                ApplyEmoteGestureConditions(transition, _isLeft, emote.gesture2);
                if (AnimationWizardBase.handSignOverrideEnabled)
                {
                    var condition = ConditionBuilder.EqualsCondition(AnimationWizardBase.HandSignOverrideParameter, 0);
                    transition.AddCondition(condition);
                }
                ApplyEmoteConditions(transition, emote.conditions);

                ApplyEmoteControl(transition, _isLeft, emote.control);
            }
        }
    }
}