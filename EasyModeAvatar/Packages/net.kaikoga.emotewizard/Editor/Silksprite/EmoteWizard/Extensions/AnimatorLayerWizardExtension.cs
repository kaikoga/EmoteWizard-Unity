using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Internal;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AnimatorLayerWizardExtension
    {
        public static RuntimeAnimatorController BuildOutputAsset(this AnimatorLayerWizardBase wizard, ParametersSnapshot parametersSnapshot)
        {
            var layerKind = wizard.LayerKind;
            var builder = new AnimatorLayerBuilder
            {
                Wizard = wizard,
                ParametersSnapshot = parametersSnapshot,
                DefaultRelativePath = $"{layerKind}/@@@Generated@@@{layerKind}.controller"
            };

            var resetClip = wizard.EmoteWizardRoot.EnsureAsset($"{layerKind}/@@@Generated@@@Reset{layerKind}.anim", ref wizard.resetClip);
            wizard.BuildResetClip(resetClip);

            if (wizard.defaultAvatarMask)
            {
                builder.BuildStaticLayer("Default Avatar Mask", null, wizard.defaultAvatarMask);
            }
            builder.BuildStaticLayer("Reset", resetClip, null);
            builder.BuildEmoteLayers(wizard.CollectEmoteItems());
            if (layerKind == LayerKind.FX) // FIXME: configurable?
            {
                builder.BuildTrackingControlLayers(wizard.CollectAllEmoteItems());
            }
            builder.BuildParameters();
            return wizard.outputAsset;
        }

        static void BuildResetClip(this AnimatorLayerWizardBase wizard, AnimationClip targetClip)
        {
            var allClips = Enumerable.Empty<AnimationClip>()
                .Concat(wizard.CollectEmoteItems().SelectMany(e => e.AllClips()))
                .Where(c => c != null).ToList();
            var curveBindings = allClips.SelectMany(AnimationUtility.GetCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            var objectReferenceCurveBindings = allClips.SelectMany(AnimationUtility.GetObjectReferenceCurveBindings)
                .Distinct().OrderBy(curve => (curve.path, curve.propertyName, curve.type));
            
            var proxyAnimator = wizard.EmoteWizardRoot.GetWizard<AvatarWizard>()?.ProvideProxyAnimator();
            var avatar = proxyAnimator != null ? proxyAnimator.gameObject : null;

            targetClip.ClearCurves();
            targetClip.frameRate = 60f;
            if (!avatar)
            {
                var gameObject = wizard.gameObject;
                Debug.LogWarning($"{gameObject}: Failed to build reset clip because Avatar is not specified.\nProxyAnimator or AvatarDescriptor is required to build ResetClip.", gameObject);
                return;
            }

            void WarnBindingNotFound(EditorCurveBinding curveBinding)
            {
                var gameObject = wizard.gameObject;
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