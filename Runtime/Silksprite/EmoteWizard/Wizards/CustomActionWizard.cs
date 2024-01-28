using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizard.Wizards;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [AddComponentMenu("Emote Wizard/Wizards/Custom Action Wizard", 301)]
    public class CustomActionWizard : EmoteWizardBase
    {
        [SerializeField] public bool hasExpressionItemSource;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;
        [SerializeField] public int actionIndex = -1;
        [ItemPath]
        [SerializeField] public string itemPath;
        [ParameterName(false, true)]
        [SerializeField] public string parameterName = EmoteWizardConstants.Defaults.Params.ActionSelect;

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            int GuessActionIndex()
            {
                var snapshot = CreateEnv().GetContext<ParametersContext>().Snapshot();
                var newValue = 21;
                var usages = snapshot.ParameterItems.FirstOrDefault(v => v.Name == EmoteWizardConstants.Defaults.Params.ActionSelect)?.ReadUsages;
                if (usages != null)
                {
                    while (usages.Any(usage => (int)usage.Value == newValue)) newValue++;
                }
                return newValue;
            }

            if (actionIndex < 0) actionIndex = GuessActionIndex();

            if (hasExpressionItemSource)
            {
                yield return new ExpressionItemTemplate(
                    itemPath,
                    new ExpressionItem
                    {
                        enabled = true,
                        icon = VrcSdkAssetLocator.PersonDance(),
                        path = itemPath,
                        parameter = parameterName,
                        value = actionIndex,
                        itemKind = ExpressionItemKind.Toggle
                    });
            }

            yield return new EmoteItemTemplate(
                itemPath, 
                new EmoteTrigger
                {
                    name = itemPath,
                    priority = 0,
                    conditions = new List<EmoteCondition>
                    {
                        new EmoteCondition
                        {
                            kind = ParameterItemKind.Int,
                            parameter = parameterName,
                            mode = EmoteConditionMode.Equals,
                            threshold = actionIndex
                        }
                    }
                },
                GenerateEmoteSequenceFactoryTemplate(emoteSequenceFactoryKind,
                    LayerKind.Action,
                    EmoteWizardConstants.Defaults.Groups.Action),
                !hasExpressionItemSource,
                itemPath,
                VrcSdkAssetLocator.PersonDance()
            );
        }
    }
}