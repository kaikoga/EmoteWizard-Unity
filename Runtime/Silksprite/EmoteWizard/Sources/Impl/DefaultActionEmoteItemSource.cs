using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Wizards.Defaults;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Default Action Emote Item Source", 911)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/default_action_emote_item_source")]
    public class DefaultActionEmoteItemSource : EmoteWizardBase, IEmoteTemplateSource
    {
        [SerializeField] public DefaultActionIndex defaultActionIndex;

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate(EmoteWizardEnvironment environment)
        {
            return new DefaultActionEmoteItemTemplate(EmoteTemplatePath.Context(CreateEnv(), this),
                defaultActionIndex);
        }

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            yield return ToEmoteTemplate();
        }

        IEmoteTemplate ToEmoteTemplate()
        {
            return DefaultActionEmote.UnpackDefaultAction(EmoteTemplatePath.Context(CreateEnv(), this), defaultActionIndex);
        }
    }
}