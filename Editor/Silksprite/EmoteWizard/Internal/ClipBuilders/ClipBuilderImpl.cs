using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ClipBuilders
{
    public class ClipBuilderImpl : IEmoteFactory.IClipBuilder
    {
        readonly EmoteWizardEnvironment _environment;

        public ClipBuilderImpl(EmoteWizardEnvironment environment)
        {
            _environment = environment;
        }

        Motion IEmoteFactory.IClipBuilder.Build(string clipName, Dictionary<string, float> floatValues)
        {
            return new AnimationClip
            {
                name = clipName
            };
        }
    }
}