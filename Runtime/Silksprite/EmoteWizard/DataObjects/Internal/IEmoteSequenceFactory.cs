using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Templates.Sequence;
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

        EmoteSequence Build(IClipBuilder builder);
        IEnumerable<TrackingOverride> TrackingOverrides();

        interface IClipBuilder
        {
            EmoteWizardEnvironment Environment { get; }
            Motion Build(string clipName, IEnumerable<GenericEmoteSequenceFactory.AnimatedValue<float>> floatValues);
        }
    }
}