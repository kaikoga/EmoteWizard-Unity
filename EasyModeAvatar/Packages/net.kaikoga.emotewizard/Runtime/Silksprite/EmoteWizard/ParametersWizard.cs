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

        IEnumerable<ParameterItem> CollectSourceParameterItems()
        {
            return EmoteWizardRoot.GetComponentsInChildren<IParameterSource>()
                .SelectMany(source => source.ParameterItems)
                .Where(item => item.enabled);
        }

        public ParametersSnapshot Snapshot()
        {
            var builder = new ExpressionParameterBuilder();

            var expressionWizard = GetWizard<ExpressionWizard>();
            if (expressionWizard != null)
            {
                foreach (var expressionItem in expressionWizard.CollectExpressionItems())
                {
                    if (!string.IsNullOrEmpty(expressionItem.parameter))
                    {
                        builder.FindOrCreate(expressionItem.parameter, true).AddUsage(expressionItem.value);
                    }

                    if (!expressionItem.IsPuppet) continue;
                    foreach (var subParameter in expressionItem.subParameters.Where(subParameter => !string.IsNullOrEmpty(subParameter)))
                    {
                        builder.FindOrCreate(subParameter, true).AddPuppetUsage(expressionItem.itemKind == ExpressionItemKind.TwoAxisPuppet);
                    }
                }
            }

            builder.Import(CollectSourceParameterItems());

            return new ParametersSnapshot
            {
                ParameterItems = builder.ParameterItems.Where(item => item.enabled).Select(item => item.ToInstance()).ToList()
            };
        }
    }
}