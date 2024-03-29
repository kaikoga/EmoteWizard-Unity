using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.ClipBuilder;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.ClipBuilder
{
    public class ClipBuilderImpl : IClipBuilder
    {
        readonly string _explodePath; 

        public ClipBuilderImpl(string explodePath = null)
        {
            _explodePath = explodePath;
        }

        Motion IClipBuilder.Build(string clipName, IEnumerable<AnimatedValue<float>> floatValues, Transform avatarRootTransform, bool useSceneOffValues)
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
                    .Select(value => BoundValues.BoundFloatValue.FromAnimatedValue(value, avatarRootTransform, useSceneOffValues))
                    .ToList(),
                new List<BoundValues.BoundObjectValue>());
            boundValues.Build(clip);

            return clip;
        }
    }
}