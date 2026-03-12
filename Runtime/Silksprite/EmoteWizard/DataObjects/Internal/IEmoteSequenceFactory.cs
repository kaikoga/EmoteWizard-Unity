using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public interface IEmoteSequenceFactory
    {
        LayerKind LayerKind { get; }
        string GroupName { get; }
        bool LooksLikeMirrorItem { get; }
        bool LooksLikeToggle { get; }

        IEnumerable<Motion> AllClipRefs();

        EmoteSequence Build(EmoteWizardEnvironment environment, IClipBuilder builder);

        IEnumerable<TrackingOverride> TrackingOverrides();
    }
}