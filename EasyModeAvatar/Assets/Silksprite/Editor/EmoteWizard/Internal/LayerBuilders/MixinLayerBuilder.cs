using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class MixinLayerBuilder : LayerBuilderBase
    {
        public MixinLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer) : base(builder, layer) { }

        public void Build(AnimationMixin mixin)
        {
            var transition = AddStateAsTransition(mixin.name, mixin.Motion);

            if (mixin.kind == AnimationMixinKind.AnimationClip)
            {
                ApplyEmoteControl(transition, true, mixin.control);
            }

            StateMachine.defaultState = StateMachine.states.FirstOrDefault().state;
        }
    }
}