using System.Collections.Generic;
using Silksprite.EmoteWizard.Platforms.Common.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders.Base;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders
{
    public class ParameterRemapDriverLayerBuilder : LayerBuilderBase
    {
        readonly IReadOnlyDictionary<int, int> _actions;
        readonly string _remapFrom;
        readonly string _remapTo;
        readonly string? _cancelParameter;

        ParameterRemapDriverLayerBuilder(AnimatorLayerBuilder builder,
            AnimatorControllerLayer layer,
            IReadOnlyDictionary<int, int> actions,
            string remapFrom,
            string remapTo,
            string? cancelParameter) : base(builder, layer)
        {
            _actions = actions;
            _remapFrom = remapFrom;
            _remapTo = remapTo;
            _cancelParameter = cancelParameter;
        }

        public static ParameterRemapDriverLayerBuilder Create(AnimatorLayerBuilder builder,
            AnimatorControllerLayer layer,
            IReadOnlyDictionary<int, int> actions,
            string remapFrom,
            string remapTo,
            string? cancelParameter)
        {
            return new ParameterRemapDriverLayerBuilder(builder, layer, actions, remapFrom, remapTo, cancelParameter);
        }

        protected override void Process()
        {
            foreach (var actionIndex in _actions)
            {
                var (input, output) = actionIndex;
                NextStateRow();
                var state = AddStateWithoutTransition($"Remap {input} -> {output}", null);
                var entryCondition = new ConditionBuilder().Equals(_remapFrom, input);
                AddEntryTransition(state, entryCondition);
                var exitCondition = _cancelParameter != null
                    ? new ConditionBuilder().Trigger(_cancelParameter)
                    : new ConditionBuilder().NotEqual(_remapFrom, input);
                AddExitTransition(state, exitCondition);
                EditorFeatures.PopulateParameterDriver(state, _remapTo, output);
            }
        }
    }
}