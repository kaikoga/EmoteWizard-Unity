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

        readonly bool _debug; 

        public ClipBuilderImpl(EmoteWizardEnvironment environment, bool debug = false)
        {
            _environment = environment;
            _debug = debug;
        }

        Motion IEmoteSequenceFactory.IClipBuilder.Build(string clipName, IEnumerable<IEmoteSequenceFactory.AnimatedValue<float>> floatValues)
        {
            var clip = new AnimationClip
            {
                name = clipName
            };

            if (_debug)
            {
                AssetDatabase.CreateAsset(clip, AssetDatabase.GenerateUniqueAssetPath("Assets/EW_Debug.asset"));
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