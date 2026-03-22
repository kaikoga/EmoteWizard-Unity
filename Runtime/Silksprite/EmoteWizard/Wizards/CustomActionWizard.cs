using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizard.Wizards.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Wizards
{
    [AddComponentMenu("Emote Wizard/Wizards/Custom Action Wizard", 301)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/wizards/custom_action_wizard")]
    public class CustomActionWizard : EmoteWizardBase
    {
        [SerializeField] public bool hasExpressionItemSource;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;
        [SerializeField] public int actionIndex = -1;
        [ItemPath]
        [SerializeField] public string itemPath = "";
        [ParameterName(false, true)]
        [SerializeField] public string parameterName = EmoteWizardConstants.Params.ActionSelect;

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            var environment = CreateEnv();
            var path = EmoteTemplatePath.Context(environment, this);

            int GuessActionIndex()
            {
                var snapshot = environment.GetContext<ParametersContext>().Snapshot();
                var newValue = 21;
                var usages = snapshot.ParameterItems.FirstOrDefault(v => v.name == EmoteWizardConstants.Params.ActionSelect)?.readUsages;
                if (usages != null)
                {
                    while (usages.Any(usage => (int)usage.value == newValue)) newValue++;
                }
                return newValue;
            }

            if (actionIndex < 0) actionIndex = GuessActionIndex();

            if (hasExpressionItemSource)
            {
                yield return new ExpressionItemTemplate(
                    path.Join(itemPath),
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
                path.Join(itemPath), 
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
                }, EmoteWizardUtil.GenerateEmoteSequenceFactoryTemplate(emoteSequenceFactoryKind,
                    LayerKind.Action,
                    EmoteWizardConstants.Groups.Action,
                    gameObject.name),
                !hasExpressionItemSource,
                itemPath,
                VrcSdkAssetLocator.PersonDance()
            );
        }
    }
}