using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ClipBuilders
{
    public class ClipBuilderImpl : IEmoteSequenceFactory.IClipBuilder
    {
        readonly EmoteWizardEnvironment _environment;
        EmoteWizardEnvironment IEmoteSequenceFactory.IClipBuilder.Environment => _environment;

        public ClipBuilderImpl(EmoteWizardEnvironment environment)
        {
            _environment = environment;
        }

        Motion IEmoteSequenceFactory.IClipBuilder.Build(string clipName, IEnumerable<GenericEmoteSequenceFactory.AnimatedValue<float>> floatValues)
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