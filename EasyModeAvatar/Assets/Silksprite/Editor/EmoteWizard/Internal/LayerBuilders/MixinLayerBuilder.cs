using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.ConditionBuilders;
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
            var defaultState = AddStateWithoutTransition("Default", null);
            NextStateColumn();
            var state = AddStateWithoutTransition(_mixin.name, _mixin.Motion);

            if (_mixin.conditions.Count > 0)
            {
                var conditions = new ConditionBuilder();
                ApplyEmoteConditions(conditions, _mixin.conditions);
                var transition = AddTransition(defaultState, state, conditions);
                ApplyEmoteControl(transition, true, _mixin.control);
                AddExitTransitions(state, conditions.Inverse());
            }
            else
            {
                var transition = AddTransition(defaultState, state, new ConditionBuilder().AlwaysTrue());
                ApplyEmoteControl(transition, true, _mixin.control);
            }
        }
    }
}