using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    public class EmoteItemWizard : EmoteWizardBase
    {
        [SerializeField] public bool hasExpressionItemSource;
        [SerializeField] public string itemPath;
        [SerializeField] public bool hasGroupName;
        [SerializeField] public string groupName;
        [SerializeField] public bool hasParameterName;
        [SerializeField] public string parameterName;

        public IEnumerable<ExpressionItem> ToExpressionItems(ExpressionContext context)
        {
            if (hasExpressionItemSource)
            {
                yield return new ExpressionItem
                {
                    enabled = true,
                    icon = VrcSdkAssetLocator.ItemWand(),
                    path = itemPath,
                    parameter = hasParameterName ? parameterName : itemPath,
                    itemKind = ExpressionItemKind.Toggle
                };
            }
        }

        public IEnumerable<EmoteItem> ToEmoteItems()
        {
            yield return new EmoteItem(
                new EmoteTrigger { name = itemPath },
                new StaticEmoteFactory(new EmoteSequence { groupName = hasGroupName ? groupName : itemPath }));
            /*
            emoteItemSource.hasExpressionItem = !hasExpressionItemSource;
            emoteItemSource.expressionItemPath = itemPath;
            emoteItemSource.expressionItemIcon = VrcSdkAssetLocator.ItemWand();
            */
        }
    }
}