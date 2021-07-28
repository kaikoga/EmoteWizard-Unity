using System.Linq;
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
            var emotes = Builder.AnimationWizardBase.emotes;

            foreach (var emote in emotes)
            {
                var clip = isLeft || !isAdvanced ? emote.clipLeft : emote.clipRight;
                if (clip == null) clip = Builder.AnimationWizardBase.EmoteWizardRoot.ProvideEmptyClip(); 
                var transition = AddStateAsTransition(emote.ToStateName(), clip);

                ApplyEmoteGestureConditions(transition, isLeft, emote.gesture1, true);
                ApplyEmoteGestureConditions(transition, isLeft, emote.gesture2);
                ApplyEmoteConditions(transition, emote);

                ApplyEmoteControl(transition, isLeft, emote.control);
            }
            
            StateMachine.defaultState = StateMachine.states.FirstOrDefault().state;
        }
    }
}