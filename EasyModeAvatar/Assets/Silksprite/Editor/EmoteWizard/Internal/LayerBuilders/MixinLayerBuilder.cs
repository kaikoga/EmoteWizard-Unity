using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
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
                if (mixin.conditions.Count > 0)
                {
                    ApplyEmoteConditions(transition, mixin.conditions);
                    var defaultTransition = AddStateAsTransition("Default", null);
                    defaultTransition.AddAlwaysTrueCondition();
                }
            }

            StateMachine.defaultState = StateMachine.states.FirstOrDefault().state;
        }
    }
}