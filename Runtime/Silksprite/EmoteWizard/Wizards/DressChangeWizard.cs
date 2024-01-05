using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [AddComponentMenu("Emote Wizard/Wizards/Dress Change Wizard", 300)]
    public class DressChangeWizard : EmoteWizardBase
    {
        [SerializeField] public bool hasExpressionItemSource;
        [SerializeField] public string itemPath;
        [SerializeField] public bool hasGroupName;
        [SerializeField] public string groupName;
        [SerializeField] public bool hasParameterName;
        [SerializeField] public string parameterName;

        public override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            foreach (var value in Enumerable.Range(0, 2))
            {
                var childName = $"{itemPath}/Item {value}";
                if (hasExpressionItemSource)
                {
                    yield return new ExpressionItemTemplate(new ExpressionItem
                    {
                        enabled = true,
                        icon = VrcSdkAssetLocator.ItemWand(),
                        path = childName,
                        parameter = hasParameterName ? parameterName : itemPath,
                        value = value,
                        itemKind = ExpressionItemKind.Toggle
                    });
                }

                yield return new EmoteItemTemplate(
                    new EmoteTrigger
                    {
                        name = childName,
                        priority = 0,
                        conditions = new List<EmoteCondition>
                        {
                            new EmoteCondition
                            {
                                kind = ParameterItemKind.Int,
                                parameter = hasParameterName ? parameterName : itemPath,
                                mode = EmoteConditionMode.Equals,
                                threshold = value
                            }
                        }
                    },
                    new EmoteSequenceFactory(new EmoteSequence
                    {
                        layerKind = LayerKind.FX,
                        groupName = hasGroupName ? groupName : itemPath

                    }),
                    hasExpressionItemSource,
                    childName,
                    VrcSdkAssetLocator.ItemWand()
                );
            }            
        }
    }
}