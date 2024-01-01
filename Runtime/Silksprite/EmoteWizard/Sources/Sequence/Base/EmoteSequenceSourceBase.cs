using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Sequence.Base
{
    [DisallowMultipleComponent]
    public abstract class EmoteSequenceSourceBase : EmoteWizardDataSourceBase
    {
        public abstract IEmoteFactory ToEmoteFactory();
    }
}