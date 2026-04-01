using Silksprite.EmoteWizard.Platforms.Common.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders.Base;
using Silksprite.EmoteWizard.Platforms.Extensions;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders
{
    public class ActionSelectDriverLayerBuilder : LayerBuilderBase
    {
        readonly int[] _actions;

        public ActionSelectDriverLayerBuilder(AnimatorLayerBuilder builder, AnimatorControllerLayer layer, int[] actions) : base(builder, layer)
        {
            _actions = actions;
        }

        protected override void Process()
        {
            var platformFeatures = Environment.GetPlatformFeatures();
            foreach (var actionIndex in _actions)
            {
                NextStateRow();
                var state = AddStateWithoutTransition($"Action {actionIndex}", null);
                AddEntryTransition(state, new ConditionBuilder().Equals(platformFeatures.ParameterForPlatformActionSelect, actionIndex));
                AddExitTransition(state, new ConditionBuilder().Trigger(platformFeatures.ParameterForPlatformCancelAction));
            }
        }
    }
}