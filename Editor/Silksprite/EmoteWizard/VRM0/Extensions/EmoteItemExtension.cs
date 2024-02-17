#if EW_VRM0

using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;
using VRM;

namespace Silksprite.EmoteWizard.Extensions
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

            blendShapeClip.Values = genericEmoteItem.GenericEmoteSequence.animatedBlendShapes
                .Select(animatedBlendShape => new BlendShapeBinding
                {
                    RelativePath = RuntimeUtil.CalculateAnimationTransformPath(environment.AvatarRoot, animatedBlendShape.target.transform),
                    Index = animatedBlendShape.target.sharedMesh.GetBlendShapeIndex(animatedBlendShape.blendShapeName),
                    Weight = animatedBlendShape.value
                }).ToArray();
            return blendShapeClip;
        }
    }
}

#endif