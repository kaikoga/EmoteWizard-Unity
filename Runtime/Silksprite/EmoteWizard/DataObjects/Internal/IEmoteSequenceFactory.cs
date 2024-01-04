using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
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

        interface IClipBuilder
        {
            Motion Build(string clipName, IEnumerable<AnimatedValue<float>> floatValues);
        }

        public struct AnimatedValue<T>
        {
            public string Path;
            public string PropertyName;
            public Type Type;

            public T Value;
        }
    }
}