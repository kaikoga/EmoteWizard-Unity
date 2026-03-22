using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Sequence
{
    [AddComponentMenu("Emote Wizard/Sources/Emote Sequence Source", 100)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/emote_sequence_source")]
    public class EmoteSequenceSource : EmoteSequenceSourceBase, IEmoteTemplateSource
    {
        [SerializeField] public EmoteSequence sequence = new EmoteSequence();

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate(EmoteWizardEnvironment environment)
        {
            return new EmoteSequenceTemplate(EmoteTemplatePath.Context(CreateEnv(), this),
                (EmoteSequenceFactory)ToEmoteFactoryTemplate());
        }

        public override IEmoteSequenceFactoryTemplate ToEmoteFactoryTemplate() => new EmoteSequenceFactory(sequence);
    }
}