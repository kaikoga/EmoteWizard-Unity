using System.Linq;
using Silksprite.EmoteWizard.Base;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AnimationWizardBaseExtension
    {
        public static void BuildResetClip(this AnimationWizardBase animationWizardBase, AnimationClip targetClip)
        {
            var allClips = Enumerable.Empty<AnimationClip>()
                .Concat(animationWizardBase.CollectBaseMixins().SelectMany(e => e.AllClips()))
                .Concat(animationWizardBase.CollectEmotes().SelectMany(e => e.AllClips()))
                .Concat(animationWizardBase.CollectParameterEmotes().SelectMany(p => p.AllClips()))
                .Concat(animationWizardBase.CollectMixins().SelectMany(p => p.AllClips()))
                .Where(c => c != null).ToList();
            var curveBindings = allClips.SelectMany(AnimationUtility.GetCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            var objectReferenceCurveBindings = allClips.SelectMany(AnimationUtility.GetObjectReferenceCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            
            var proxyAnimator = animationWizardBase.EmoteWizardRoot.GetWizard<AvatarWizard>()?.ProvideProxyAnimator();
            var avatar = proxyAnimator != null ? proxyAnimator.gameObject : null;

            targetClip.ClearCurves();
            targetClip.frameRate = 60f;
            if (!avatar)
            {
                Debug.LogWarning("FXWizard: Failed to build reset clip because Avatar is not specified.\nProxyAnimator or AvatarDescriptor is required to build ResetClip.");
                return;
            }

            void WarnBindingNotFound(EditorCurveBinding curveBinding)
            {
                Debug.LogWarning($@"FXWizard: ResetClip may be insufficient because animated property is not found in avatar.
Object Path: {curveBinding.path}
Property: {curveBinding.type} {curveBinding.propertyName}
This property is not included in ResetClip.");
            }

            foreach (var curveBinding in curveBindings)
            {
                var value = 0f;
                if (AnimationUtility.GetFloatValue(avatar, curveBinding, out value))
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
                Object value = null;
                if (AnimationUtility.GetObjectReferenceValue(avatar, curveBinding, out value))
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