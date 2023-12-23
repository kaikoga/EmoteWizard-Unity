using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Impl;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ClipBuilders
{
    public class ClipBuilderImpl : IEmoteFactory.IClipBuilder
    {
        readonly EmoteWizardEnvironment _environment;
        EmoteWizardEnvironment IEmoteFactory.IClipBuilder.Environment => _environment;

        public ClipBuilderImpl(EmoteWizardEnvironment environment)
        {
            _environment = environment;
        }

        Motion IEmoteFactory.IClipBuilder.Build(string clipName, IEnumerable<SimpleEmoteFactory.AnimatedValue<float>> floatValues)
        {
            var clip = new AnimationClip
            {
                name = clipName
            };

            var boundValues = new BoundValues(
                floatValues
                    .Select(BoundValues.BoundFloatValue.FromAnimatedValue)
                    .ToList(),
                new List<BoundValues.BoundObjectValue>());
            boundValues.Build(clip);

            return clip;
        }
    }
}