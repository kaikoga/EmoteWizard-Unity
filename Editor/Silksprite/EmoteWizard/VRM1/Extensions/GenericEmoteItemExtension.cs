#if EW_VRM1

using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;
using UniVRM10;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class GenericEmoteItemExtension
    {
        public static VRM10Expression ToVRM10Expression(this GenericEmoteItem genericEmoteItem, EmoteWizardEnvironment environment, out ExpressionPreset expressionPreset)
        {
            genericEmoteItem.Trigger.TryGetVrm1ExpressionPreset(out var vrm1Expression);
            expressionPreset = vrm1Expression.ToExpressionPreset();
            var expression = ScriptableObject.CreateInstance<VRM10Expression>();

            expression.name = expressionPreset == ExpressionPreset.custom ? genericEmoteItem.Trigger.name : expressionPreset.ToString();

            expression.MorphTargetBindings = genericEmoteItem.GenericEmoteSequence.animatedBlendShapes
                .Select(animatedBlendShape => new MorphTargetBinding
                {
                    RelativePath = EditorUtil.RelativePath(environment.AvatarRoot.gameObject, animatedBlendShape.target.gameObject),
                    Index = animatedBlendShape.target.sharedMesh.GetBlendShapeIndex(animatedBlendShape.blendShapeName),
                    Weight = animatedBlendShape.value / 100f
                }).ToArray();
            return expression;
        }
    }
}

#endif