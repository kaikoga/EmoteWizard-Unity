using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class EmoteSequenceSource : EmoteSequenceSourceBase
    {
        [SerializeField] public EmoteSequence sequence;

        public override EmoteSequence EmoteSequence => sequence;
    }
}