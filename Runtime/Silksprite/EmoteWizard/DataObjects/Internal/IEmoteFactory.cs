using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Impl;
using Silksprite.EmoteWizard.Sources.Templates;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public interface IEmoteFactory
    {
        LayerKind LayerKind { get; }
        string GroupName { get; }
        bool LooksLikeMirrorItem { get; }
        bool LooksLikeToggle { get; }

        IEnumerable<Motion> AllClipRefs();

        IEmoteFactoryTemplate ToTemplate();
        EmoteSequence Build(IClipBuilder builder);
        IEnumerable<TrackingOverride> TrackingOverrides();

        interface IClipBuilder
        {
            EmoteWizardEnvironment Environment { get; }
            Motion Build(string clipName, IEnumerable<SimpleEmoteFactory.AnimatedValue<float>> floatValues);
        }
    }
}