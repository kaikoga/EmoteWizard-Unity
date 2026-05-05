using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Platforms.Common.Internal.ConditionBuilders;
using Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders.Base;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders
{
    public class ParameterRemapDriverLayerBuilder : LayerBuilderBase
    {
        readonly IReadOnlyDictionary<int, int> _actions;
        readonly string _selectParameter;
        readonly string? _cancelParameter;
        readonly string _outputParameter;

        ParameterRemapDriverLayerBuilder(
            AnimatorLayerBuilder builder,
            AnimatorControllerLayer layer,
            IReadOnlyDictionary<int, int> actions,
            string selectParameter,
            string? cancelParameter,
            string outputParameter) : base(builder, layer)
        {
            _actions = actions;
            _selectParameter = selectParameter;
            _cancelParameter = cancelParameter;
            _outputParameter = outputParameter;
        }

        public static ParameterRemapDriverLayerBuilder Create(
            AnimatorLayerBuilder builder,
            AnimatorControllerLayer layer,
            IEnumerable<int> actions,
            string selectParameter,
            string? cancelParameter,
            string outputParameter)
        {
            return new ParameterRemapDriverLayerBuilder(
                builder,
                layer,
                actions.Select((action, index) => (Key: action, Value: index)).ToDictionary(kv => kv.Key, kv => kv.Value),
                selectParameter,
                cancelParameter,
                outputParameter);
        }

        protected override void Process()
        {
            foreach (var actionIndex in _actions)
            {
                var (input, output) = actionIndex;
                NextStateRow();
                var state = AddStateWithoutTransition($"Action {actionIndex}", null);
                var entryCondition = new ConditionBuilder().Equals(_selectParameter, input);
                AddEntryTransition(state, entryCondition);
                var exitCondition = _cancelParameter != null
                    ? new ConditionBuilder().Trigger(_cancelParameter)
                    : new ConditionBuilder().NotEqual(_selectParameter, input);
                AddExitTransition(state, exitCondition);
                EditorFeatures.PopulateParameterDriver(state, _outputParameter, output);
            }
        }
    }
}