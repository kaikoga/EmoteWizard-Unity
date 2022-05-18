using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Extensions;
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
                    var transition = LegacyStateMachine.AddAnyStateTransition(state);
                    transition.AddCondition(AnimatorConditionMode.Equals, emote.overrideIndex, AnimationWizardBase.HandSignOverrideParameter);

                    ApplyEmoteControl(transition, _isLeft, emote.control);
                }
            }

            foreach (var (emote, state) in emoteStates)
            {
                var transition = LegacyStateMachine.AddAnyStateTransition(state);
                ApplyEmoteGestureConditions(transition, _isLeft, emote.gesture1, true);
                ApplyEmoteGestureConditions(transition, _isLeft, emote.gesture2);
                if (AnimationWizardBase.handSignOverrideEnabled)
                {
                    transition.AddCondition(AnimatorConditionMode.Equals, 0, AnimationWizardBase.HandSignOverrideParameter);
                }
                ApplyEmoteConditions(transition, emote.conditions);

                ApplyEmoteControl(transition, _isLeft, emote.control);
            }
            
            LegacyStateMachine.defaultState = LegacyStateMachine.states.FirstOrDefault().state;
        }
    }
}