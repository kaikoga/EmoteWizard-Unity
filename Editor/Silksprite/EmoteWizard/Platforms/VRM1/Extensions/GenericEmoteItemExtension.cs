#if ATIV_DETECTED_VRM1

using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Animations;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;
using UniVRM10;

namespace Silksprite.EmoteWizard.Platforms.VRM1.Extensions
{
    public static class GenericEmoteItemExtension
    {
        public static VRM10Expression ToVRM10Expression(this GenericEmoteItem genericEmoteItem, EmoteWizardEnvironment environment, out ExpressionPreset expressionPreset)
        {
            genericEmoteItem.Trigger.TryGetVrm1ExpressionPreset(out var vrm1Expression);
            expressionPreset = vrm1Expression.ToExpressionPreset();
            var expression = ScriptableObject.CreateInstance<VRM10Expression>();

            expression.name = expressionPreset == ExpressionPreset.custom ? genericEmoteItem.Trigger.name : expressionPreset.ToString();

            IEnumerable<MorphTargetBinding> ToMorphTargetBindings(AnimatedBlendShape animatedBlendShape)
            {
                if (!(animatedBlendShape.relativeRef.target is { } target))
                {
                    yield break;
                }
                yield return new MorphTargetBinding
                {
                    RelativePath = RuntimeUtil.CalculateAnimationTransformPath(environment.AvatarRoot, target.transform),
                    Index = animatedBlendShape.relativeRef.target.sharedMesh.GetBlendShapeIndex(animatedBlendShape.blendShapeName),
                    Weight = animatedBlendShape.value / 100f
                };
            }
            expression.MorphTargetBindings = genericEmoteItem.GenericEmoteSequence.animatedBlendShapes
                .SelectMany(ToMorphTargetBindings)
                .ToArray();

            return expression;
        }
    }
}

#endif