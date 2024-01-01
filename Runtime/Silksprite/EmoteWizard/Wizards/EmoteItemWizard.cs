using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
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

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            yield return new EmoteItemTemplate(
                new EmoteTrigger { name = itemPath },
                new EmoteSequenceFactory(new EmoteSequence { groupName = hasGroupName ? groupName : itemPath }),
                !hasExpressionItemSource,
                itemPath,
                VrcSdkAssetLocator.ItemWand()
                );
        }

        public override IEnumerable<ExpressionItem> ToExpressionItems()
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

            foreach (var e in base.ToExpressionItems()) yield return e; 
        }
    }
}