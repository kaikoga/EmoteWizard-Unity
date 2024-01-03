using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ClipBuilders
{
    public class ClipBuilderImpl : IEmoteSequenceFactory.IClipBuilder
    {
        readonly EmoteWizardEnvironment _environment;
        EmoteWizardEnvironment IEmoteSequenceFactory.IClipBuilder.Environment => _environment;

        readonly string _explodePath; 

        public ClipBuilderImpl(EmoteWizardEnvironment environment, string explodePath = null)
        {
            _environment = environment;
            _explodePath = explodePath;
        }

        Motion IEmoteSequenceFactory.IClipBuilder.Build(string clipName, IEnumerable<IEmoteSequenceFactory.AnimatedValue<float>> floatValues)
        {
            var clip = new AnimationClip
            {
                name = clipName
            };

            if (!string.IsNullOrWhiteSpace(_explodePath))
            {
                AssetDatabase.CreateAsset(clip, AssetDatabase.GenerateUniqueAssetPath(_explodePath));
            }

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