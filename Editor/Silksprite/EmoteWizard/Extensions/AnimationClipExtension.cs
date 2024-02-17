using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Animations;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AnimationClipExtension
    {
        public static GenericEmoteSequence ToGenericEmoteSequence(this AnimationClip clip, GameObject animationRoot)
        {
            var sampleTime = clip.length;
            
            var animatedEnable = new List<AnimatedEnable>();
            var animatedBlendShapes = new List<AnimatedBlendShape>();

            var curveBindings = AnimationUtility.GetCurveBindings(clip);
            var objectReferenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

            foreach (var curveBinding in curveBindings)
            {
                if (curveBinding.type == typeof(GameObject))
                {
                    switch (curveBinding.propertyName)
                    {
                        case "m_IsActive":
                            animatedEnable.Add(new AnimatedEnable
                            {
                                relativeRef = new RelativeTransformRef
                                {
                                    target = RuntimeUtil.FromAnimationTransformPath(animationRoot.transform, curveBinding.path),
                                    relativePath = curveBinding.path
                                },
                                isEnable = AnimationUtility.GetEditorCurve(clip, curveBinding).Evaluate(sampleTime) != 0f 
                            });
                            break;
                    }
                } else if (curveBinding.type == typeof(SkinnedMeshRenderer))
                {
                    if (curveBinding.propertyName.StartsWith("blendShape."))
                    {
                        animatedBlendShapes.Add(new AnimatedBlendShape
                        {
                            relativeRef = new RelativeSkinnedMeshRendererRef
                            {
                                target = RuntimeUtil.FromAnimationTransformPath(animationRoot.transform, curveBinding.path).GetComponent<SkinnedMeshRenderer>(),
                                relativePath = curveBinding.path
                            },
                            blendShapeName = curveBinding.propertyName.Substring("blendShape.".Length),
                            value = AnimationUtility.GetEditorCurve(clip, curveBinding).Evaluate(sampleTime)
                        });
                    }
                }
            }

            foreach (var objectReferenceCurveBinding in objectReferenceCurveBindings)
            {
                
            }
            
            return new GenericEmoteSequence
            {
                animatedEnable = animatedEnable.ToArray(),
                animatedBlendShapes = animatedBlendShapes.ToArray()
            };
        }
    }
}