using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ParametersContext : OutputContextBase<ParametersWizard, VRCExpressionParameters>
    {
        public ParametersContext(ParametersWizard wizard) : base(wizard) { }

        public override VRCExpressionParameters OutputAsset
        {
            get => Wizard.outputAsset;
            set => Wizard.outputAsset = value;
        }

        public override void DisconnectOutputAssets()
        {
            Wizard.outputAsset = null;
        }

        IEnumerable<EmoteItem> CollectEmoteItems()
        {
            return Wizard.Environment.CollectAllEmoteItems();
        }

        IEnumerable<ExpressionItem> CollectExpressionItems()
        {
            var expressionWizard = Wizard.Environment.GetContext<ExpressionContext>();
            return expressionWizard != null ? expressionWizard.CollectExpressionItems() : Enumerable.Empty<ExpressionItem>();
        }

        IEnumerable<ParameterItem> CollectSourceParameterItems()
        {
            return Wizard.Environment.GetComponentsInChildren<IParameterSource>()
                .SelectMany(source => source.ParameterItems);
        }

        public ParametersSnapshot Snapshot()
        {
            var builder = new ParameterSnapshotBuilder();

            foreach (var expressionItem in CollectExpressionItems())
            {
                if (!string.IsNullOrEmpty(expressionItem.parameter))
                {
                    builder.FindOrCreate(expressionItem.parameter).AddWriteValue(expressionItem.value);
                }

                if (!expressionItem.IsPuppet) continue;
                foreach (var subParameter in expressionItem.subParameters.Where(subParameter => !string.IsNullOrEmpty(subParameter)))
                {
                    builder.FindOrCreate(subParameter).AddWritePuppet(expressionItem.itemKind == ExpressionItemKind.TwoAxisPuppet);
                }
            }

            foreach (var emoteItem in CollectEmoteItems())
            {
                foreach (var condition in emoteItem.Trigger.conditions)
                {
                    builder.FindOrCreate(condition.parameter).AddReadValue(condition.kind, condition.threshold);
                }
            }

            foreach (var parameter in CollectSourceParameterItems())
            {
                builder.FindOrCreate(parameter.name).Import(parameter);
            }

            return builder.ToSnapshot();
        }
    }
}