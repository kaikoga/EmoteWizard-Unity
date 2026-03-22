#if ATIV_DETECTED_VRM0

using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Animations;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;
using VRM;

namespace Silksprite.EmoteWizard.Platforms.VRM0.Extensions
{
    public static class GenericEmoteItemExtension
    {
        public static BlendShapeClip ToBlendShapeClip(this GenericEmoteItem genericEmoteItem, EmoteWizardEnvironment environment)
        {
            genericEmoteItem.Trigger.TryGetVrm0BlendShape(out var vrm0BlendShape);
            var blendShapePreset = vrm0BlendShape.ToBlendShapePreset();
            var blendShapeClip = ScriptableObject.CreateInstance<BlendShapeClip>();

            if (blendShapePreset == BlendShapePreset.Unknown)
            {
                blendShapeClip.name = genericEmoteItem.Trigger.name;
                blendShapeClip.BlendShapeName = genericEmoteItem.Trigger.name;
            }
            else
            {
                blendShapeClip.name = vrm0BlendShape.ToString();
            }
            blendShapeClip.Preset = blendShapePreset;

            IEnumerable<BlendShapeBinding> ToBlendShapeBindings(AnimatedBlendShape animatedBlendShape)
            {
                if (!(animatedBlendShape.relativeRef.target is { } target))
                {
                    yield break;
                }
                yield return new BlendShapeBinding
                {
                    RelativePath = RuntimeUtil.CalculateAnimationTransformPath(environment.AvatarRoot, target.transform),
                    Index = animatedBlendShape.relativeRef.target.sharedMesh.GetBlendShapeIndex(animatedBlendShape.blendShapeName),
                    Weight = animatedBlendShape.value
                };
            }
            blendShapeClip.Values = genericEmoteItem.GenericEmoteSequence.animatedBlendShapes
                .SelectMany(ToBlendShapeBindings)
                .ToArray();

            return blendShapeClip;
        }
    }
}

#endif