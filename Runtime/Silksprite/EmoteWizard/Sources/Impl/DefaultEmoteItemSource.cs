using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizard.Wizards.Defaults;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Default Emote Item Source", 910)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/default_emote_item_source")]
    public class DefaultEmoteItemSource : EmoteWizardBase, IEmoteTemplateSource
    {
        [SerializeField] public EmoteItemKind emoteItemKind;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;
        
        [SerializeField] public LayerKind layerKind;
        [SerializeField] public HandSign handSign;

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate(EmoteWizardEnvironment environment)
        {
            return new DefaultEmoteItemTemplate(EmoteTemplatePath.Context(CreateEnv(), this),
                emoteItemKind,
                emoteSequenceFactoryKind,
                layerKind,
                handSign);
        }

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            yield return ToEmoteTemplate();
        }

        IEmoteTemplate ToEmoteTemplate()
        {
            var environment = CreateEnv();
            return DefaultEmoteItem.UnpackDefaultHandSign(EmoteTemplatePath.Context(environment, this), emoteItemKind, emoteSequenceFactoryKind, environment.GetPlatformFeatures(), layerKind, handSign);
        }
    }
}