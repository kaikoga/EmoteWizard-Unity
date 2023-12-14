using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    [DisallowMultipleComponent]
    public abstract class EmoteSequenceSourceBase : EmoteWizardDataSourceBase
    {
        public abstract bool LooksLikeMirrorItem { get; }

        public abstract EmoteSequence ToEmoteSequence();
    }
}