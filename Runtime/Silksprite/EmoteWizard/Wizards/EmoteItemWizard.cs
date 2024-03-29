using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizard.Wizards;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [AddComponentMenu("Emote Wizard/Wizards/Emote Item Wizard", 200)]
    public class EmoteItemWizard : EmoteWizardBase
    {
        [SerializeField] public bool hasExpressionItemSource;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;
        [ItemPath]
        [SerializeField] public string itemPath;
        [SerializeField] public bool hasGroupName;
        [SerializeField] public string groupName;
        [SerializeField] public bool hasParameterName;
        [ParameterName(false, true)]
        [SerializeField] public string parameterName;

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            yield return new EmoteItemTemplate(
                itemPath,
                new EmoteTrigger { name = itemPath },
                GenerateEmoteSequenceFactoryTemplate(emoteSequenceFactoryKind,
                    LayerKind.FX,
                    groupName = hasGroupName ? groupName : itemPath),
                !hasExpressionItemSource,
                itemPath,
                VrcSdkAssetLocator.ItemWand()
            );

            if (hasExpressionItemSource)
            {
                yield return new ExpressionItemTemplate(
                    itemPath,
                    new ExpressionItem
                    {
                        enabled = true,
                        icon = VrcSdkAssetLocator.ItemWand(),
                        path = itemPath,
                        parameter = hasParameterName ? parameterName : itemPath,
                        itemKind = ExpressionItemKind.Toggle
                    });
            }
        }
    }
}