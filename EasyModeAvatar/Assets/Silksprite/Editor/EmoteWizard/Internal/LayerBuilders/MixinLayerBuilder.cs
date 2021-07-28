using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class MixinLayerBuilder : LayerBuilderBase
    {
        public MixinLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer) : base(builder, layer) { }

        public void Build(AnimationMixin mixin)
        {
            var transition = AddStateAsTransition(mixin.name, mixin.Motion);

            if (mixin.kind == AnimationMixinKind.AnimationClip && mixin.normalizedTimeEnabled)
            {
                ApplyEmoteControl(transition, true, new EmoteControl
                {
                    normalizedTimeEnabled = mixin.normalizedTimeEnabled,
                    normalizedTimeLeft = mixin.normalizedTime,
                    normalizedTimeRight = mixin.normalizedTime
                });
            }

            StateMachine.defaultState = StateMachine.states.FirstOrDefault().state;
        }
    }
}