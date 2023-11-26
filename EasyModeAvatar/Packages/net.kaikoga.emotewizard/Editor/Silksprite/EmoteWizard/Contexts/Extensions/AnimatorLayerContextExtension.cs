using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class AnimatorLayerContextExtension
    {
        public static RuntimeAnimatorController BuildOutputAsset(this AnimatorLayerContextBase context, ParametersSnapshot parametersSnapshot)
        {
            var layerKind = context.LayerKind;
            var defaultRelativePath = $"{layerKind}/@@@Generated@@@{layerKind}.controller";
            var animatorController = context.ReplaceOrCreateOutputAsset(defaultRelativePath);
            var builder = new AnimatorLayerBuilder(context, parametersSnapshot, animatorController);

            if (context.DefaultAvatarMask)
            {
                builder.BuildStaticLayer("Default Avatar Mask", null, context.DefaultAvatarMask);
            }

            AnimationClip resetClip;
            if (context.HasResetClip)
            {
                resetClip = context.Environment.EnsureAsset($"{layerKind}/@@@Generated@@@Reset{layerKind}.anim", context.ResetClip);
                context.BuildResetClip(resetClip);
                builder.BuildStaticLayer("Reset", resetClip, null);
            }
            else
            {
                resetClip = null;
            }
            context.ResetClip = resetClip;

            builder.BuildEmoteLayers(context.CollectEmoteItems());
            if (layerKind == context.Environment.GenerateTrackingControlLayer)
            {
                builder.BuildTrackingControlLayers(context.Environment.CollectAllEmoteItems());
            }
            builder.BuildParameters();
            return context.OutputAsset;
        }

        static void BuildResetClip(this AnimatorLayerContextBase context, AnimationClip targetClip)
        {
            var allClips = Enumerable.Empty<AnimationClip>()
                .Concat(context.CollectEmoteItems().SelectMany(e => e.AllClips()))
                .Where(c => c != null).ToList();
            var curveBindings = allClips.SelectMany(AnimationUtility.GetCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            var objectReferenceCurveBindings = allClips.SelectMany(AnimationUtility.GetObjectReferenceCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            
            var proxyAnimator = context.Environment.ProvideProxyAnimator();
            var avatar = proxyAnimator != null ? proxyAnimator.gameObject : null;

            targetClip.ClearCurves();
            targetClip.frameRate = 60f;
            if (!avatar)
            {
                var gameObject = context.Environment.ContainerTransform.gameObject;
                Debug.LogWarning($"{gameObject}: Failed to build reset clip because Avatar is not specified.\nProxyAnimator or AvatarDescriptor is required to build ResetClip.", gameObject);
                return;
            }

            void WarnBindingNotFound(EditorCurveBinding curveBinding)
            {
                var gameObject = context.Environment.ContainerTransform.gameObject;
                Debug.LogWarning($@"{gameObject.name}: ResetClip may be insufficient because animated property is not found in avatar.
Object Path: {curveBinding.path}
Property: {curveBinding.type} {curveBinding.propertyName}
This property is not included in ResetClip.", gameObject);
            }

            foreach (var curveBinding in curveBindings)
            {
                if (AnimationUtility.GetFloatValue(avatar, curveBinding, out var value))
                {
                    targetClip.SetCurve(curveBinding.path, curveBinding.type, curveBinding.propertyName, AnimationCurve.Constant(0f, 1 / 60f, value));
                }
                else
                {
                    WarnBindingNotFound(curveBinding);
                }
            }
            foreach (var curveBinding in objectReferenceCurveBindings)
            {
                if (AnimationUtility.GetObjectReferenceValue(avatar, curveBinding, out var value))
                {
                    AnimationUtility.SetObjectReferenceCurve(targetClip, curveBinding, new []
                    {
                        new ObjectReferenceKeyframe { time = 0, value = value },
                        new ObjectReferenceKeyframe { time = 1 / 60f, value = value }
                    });
                }
                else
                {
                    WarnBindingNotFound(curveBinding);
                }
            }
        }
    }
}