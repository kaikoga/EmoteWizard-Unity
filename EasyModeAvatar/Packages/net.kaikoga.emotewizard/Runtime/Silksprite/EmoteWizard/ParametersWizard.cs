using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
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

        public override void DisconnectOutputAssets()
        {
            outputAsset = null;
        }

        IEnumerable<ExpressionItem> CollectExpressionItems()
        {
            var expressionWizard = GetWizard<ExpressionWizard>();
            return expressionWizard ? expressionWizard.CollectExpressionItems() : Enumerable.Empty<ExpressionItem>();
        }

        IEnumerable<ParameterItem> CollectSourceParameterItems()
        {
            return EmoteWizardRoot.GetComponentsInChildren<IParameterSource>()
                .SelectMany(source => source.ParameterItems)
                .Where(item => item.enabled);
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

            foreach (var parameter in CollectSourceParameterItems())
            {
                builder.FindOrCreate(parameter.name).Import(parameter);
            }

            return builder.ToSnapshot();
        }
    }
}