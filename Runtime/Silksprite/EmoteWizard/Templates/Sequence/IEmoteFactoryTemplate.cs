using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Sequence
{
    public interface IEmoteFactoryTemplate
    {
        bool LooksLikeMirrorItem { get; }
        bool LooksLikeToggle { get; }

        IEmoteFactory ToEmoteFactory();

        EmoteSequenceSourceBase AddSequenceSource(Component target);
    }
}