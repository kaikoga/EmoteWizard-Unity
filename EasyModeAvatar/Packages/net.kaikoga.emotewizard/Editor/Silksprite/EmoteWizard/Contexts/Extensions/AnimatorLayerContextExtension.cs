using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Internal.ClipBuilders;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class AnimatorLayerContextExtension
    {
        public static RuntimeAnimatorController BuildOutputAsset(this AnimatorLayerContextBase context, ParametersSnapshot parametersSnapshot)
        {
            var layerKind = context.LayerKind;
            var defaultPath = GeneratedPaths.GeneratedLayer(layerKind);
            var animatorController = context.ReplaceOrCreateOutputAsset(defaultPath);
            var builder = new AnimatorLayerBuilder(context.Environment, context.LayerKind, parametersSnapshot, animatorController);

            if (context.DefaultAvatarMask)
            {
                builder.BuildStaticLayer("Default Avatar Mask", null, context.DefaultAvatarMask);
            }

            AnimationClip resetClip;
            if (context.HasResetClip)
            {
                resetClip = context.Environment.EnsureAsset(GeneratedPaths.GeneratedResetLayer(layerKind), context.ResetClip);
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
                builder.BuildTrackingControlLayers(context.Environment.AllEmoteItems());
            }
            builder.BuildParameters();
            return context.OutputAsset;
        }

        static void BuildResetClip(this AnimatorLayerContextBase context, AnimationClip targetClip)
        {
            var proxyAnimator = context.Environment.ProvideProxyAnimator();
            var avatar = proxyAnimator != null ? proxyAnimator.gameObject : context.GameObject;
            if (!avatar)
            {
                var gameObject = context.Environment.ContainerTransform.gameObject;
                Debug.LogWarning($"{gameObject}: Failed to build reset clip because Avatar is not specified.\nProxyAnimator or AvatarDescriptor is required to build ResetClip.", gameObject);
                return;
            }

            var allClips = Enumerable.Empty<AnimationClip>()
                .Concat(context.CollectEmoteItems().SelectMany(e => e.AllClipsRec()));

            var curveBindings = CurveBindings.Collect(allClips);
            var boundValues = ResetValuesExtractor.ExtractFromAvatarRoot(curveBindings, avatar);
            boundValues.Build(targetClip);
        }
    }
}