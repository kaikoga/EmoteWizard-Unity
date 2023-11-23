using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ParametersContext : IOutputContext<VRCExpressionParameters>

    {
    readonly ParametersWizard _wizard;

    public ParametersContext(ParametersWizard wizard) => _wizard = wizard;

    public EmoteWizardEnvironment Environment => _wizard.Environment;

    public GameObject GameObject => _wizard.gameObject;

    public VRCExpressionParameters OutputAsset
    {
        get => _wizard.outputAsset;
        set => _wizard.outputAsset = value;
    }

    public void DisconnectOutputAssets()
    {
        _wizard.outputAsset = null;
    }

    IEnumerable<EmoteItem> CollectEmoteItems()
    {
        return _wizard.Environment.CollectAllEmoteItems();
    }

    IEnumerable<ExpressionItem> CollectExpressionItems()
    {
        var expressionWizard = _wizard.Environment.GetContext<ExpressionContext>();
        return expressionWizard != null ? expressionWizard.CollectExpressionItems() : Enumerable.Empty<ExpressionItem>();
    }

    IEnumerable<ParameterItem> CollectSourceParameterItems()
    {
        return _wizard.Environment.GetComponentsInChildren<IParameterSource>()
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