using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Default Action Emote Item Source", 911)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/default_action_emote_item_source")]
    public class DefaultActionEmoteItemSource : EmoteWizardBase, IEmoteTemplateSource
    {
        [SerializeField] public DefaultActionIndex defaultActionIndex;

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate() => CompositeTemplate();

        ICompositeEmoteTemplate CompositeTemplate()
        {
            return new DefaultActionEmoteItemTemplate(SelfPath, defaultActionIndex);
        }

        protected override IEnumerable<IEmoteTemplate> SourceTemplates(EmoteWizardEnvironment environment)
        {
            return CompositeTemplate().Unpack(environment.GetPlatformFeatures());
        }
    }
}