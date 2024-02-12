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
            var blendShapeClip = ScriptableObject.CreateInstance<BlendShapeClip>();
            blendShapeClip.name = genericEmoteItem.Trigger.handSign.ToString();
            blendShapeClip.BlendShapeName = genericEmoteItem.Trigger.handSign.ToString();
            blendShapeClip.Preset = BlendShapePreset.Unknown;
            blendShapeClip.Values = genericEmoteItem.GenericEmoteSequence.animatedBlendShapes
                .Select(animatedBlendShape => new BlendShapeBinding
                {
                    RelativePath = EditorUtil.RelativePath(environment.AvatarRoot.gameObject, animatedBlendShape.target.gameObject),
                    Index = animatedBlendShape.target.sharedMesh.GetBlendShapeIndex(animatedBlendShape.blendShapeName),
                    Weight = animatedBlendShape.value
                }).ToArray();
            return blendShapeClip;
        }
    }
}