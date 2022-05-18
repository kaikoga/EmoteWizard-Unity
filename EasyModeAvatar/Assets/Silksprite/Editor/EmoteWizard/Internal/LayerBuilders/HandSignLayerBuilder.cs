using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class HandSignLayerBuilder : LayerBuilderBase
    {
        public HandSignLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer) : base(builder, layer) { }

        public void Build(bool isLeft, bool isAdvanced)
        {
            var emotes = AnimationWizardBase.CollectEmotes();

            var emoteStates = emotes.Select(emote =>
            {
                var clip = isLeft || !isAdvanced ? emote.clipLeft : emote.clipRight;
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
                    var transition = StateMachine.AddAnyStateTransition(state);
                    transition.AddCondition(AnimatorConditionMode.Equals, emote.overrideIndex, AnimationWizardBase.HandSignOverrideParameter);

                    ApplyEmoteControl(transition, isLeft, emote.control);
                }
            }

            foreach (var (emote, state) in emoteStates)
            {
                var transition = StateMachine.AddAnyStateTransition(state);
                ApplyEmoteGestureConditions(transition, isLeft, emote.gesture1, true);
                ApplyEmoteGestureConditions(transition, isLeft, emote.gesture2);
                if (AnimationWizardBase.handSignOverrideEnabled)
                {
                    transition.AddCondition(AnimatorConditionMode.Equals, 0, AnimationWizardBase.HandSignOverrideParameter);
                }
                ApplyEmoteConditions(transition, emote.conditions);

                ApplyEmoteControl(transition, isLeft, emote.control);
            }
            
            StateMachine.defaultState = StateMachine.states.FirstOrDefault().state;
        }
    }
}