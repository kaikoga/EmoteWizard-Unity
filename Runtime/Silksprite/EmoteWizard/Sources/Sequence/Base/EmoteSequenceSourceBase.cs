using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Sequence.Base
{
    [DisallowMultipleComponent]
    public abstract class EmoteSequenceSourceBase : EmoteWizardDataSourceBase
    {
        public abstract IEmoteSequenceFactoryTemplate ToEmoteFactoryTemplate();
    }
}