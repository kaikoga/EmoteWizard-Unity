using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ParametersWizard : EmoteWizardBase
    {
        [SerializeField] public VRCExpressionParameters outputAsset;

        public override IBehaviourContext ToContext() => GetContext();
        public IParametersWizardContext GetContext() => new ParametersContext(this);

        IEnumerable<EmoteItem> CollectEmoteItems()
        {
            return Environment.CollectAllEmoteItems();
        }

        IEnumerable<ExpressionItem> CollectExpressionItems()
        {
            var expressionWizard = GetContext<IExpressionWizardContext>();
            return expressionWizard != null ? expressionWizard.CollectExpressionItems() : Enumerable.Empty<ExpressionItem>();
        }

        IEnumerable<ParameterItem> CollectSourceParameterItems()
        {
            return Environment.GetComponentsInChildren<IParameterSource>()
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
        
        class ParametersContext : IParametersWizardContext
        {
            readonly ParametersWizard _wizard;

            public ParametersContext(ParametersWizard wizard) => _wizard = wizard;

            IEmoteWizardEnvironment IBehaviourContext.Environment => _wizard.Environment;

            GameObject IBehaviourContext.GameObject => _wizard.gameObject;

            VRCExpressionParameters IOutputContext<VRCExpressionParameters>.OutputAsset
            {
                get => _wizard.outputAsset;
                set => _wizard.outputAsset = value;
            }

            void IBehaviourContext.DisconnectOutputAssets()
            {
                _wizard.outputAsset = null;
            }

            ParametersSnapshot IParametersWizardContext.Snapshot() => _wizard.Snapshot();
        }
    }
}