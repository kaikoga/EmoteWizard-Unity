using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders.Base;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Internal.LayerBuilders
{
    public class MixinLayerBuilder : LayerBuilderBase
    {
        readonly AnimationMixin _mixin;

        public MixinLayerBuilder(AnimationControllerBuilder builder, AnimatorControllerLayer layer, AnimationMixin mixin) : base(builder, layer)
        {
            _mixin = mixin;
        }

        protected override void Process()
        {
            var transition = AddStateAsTransition(_mixin.name, _mixin.Motion);

            ApplyEmoteControl(transition, true, _mixin.control);
            if (_mixin.conditions.Count > 0)
            {
                ApplyEmoteConditions(transition, _mixin.conditions);
                var defaultTransition = AddStateAsTransition("Default", null);
                defaultTransition.AddAlwaysTrueCondition();
            }
        }
    }
}