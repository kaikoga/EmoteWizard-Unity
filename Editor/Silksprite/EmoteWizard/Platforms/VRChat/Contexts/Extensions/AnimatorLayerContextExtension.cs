using System;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.VRChat.Extensions;
using Silksprite.EmoteWizard.Platforms.VRChat.Internal;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using Silksprite.EmoteWizardSupport.Logger;
using Silksprite.Loch;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Extensions
{
    public static class AnimatorLayerContextExtension
    {
        public static RuntimeAnimatorController BuildOutputAsset(this AnimatorLayerContextBase context, ParametersSnapshot parametersSnapshot)
        {
            var layerOutputKind = context.LayerOutputKind;
            var layerKind = context.LayerOutputKind switch
            {
                LayerOutputKind.Fx => LayerKind.FX,
                LayerOutputKind.Gesture => LayerKind.Gesture,
                LayerOutputKind.Action => LayerKind.Action,
                LayerOutputKind.Editor or LayerOutputKind.Merged => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
            var defaultPath = GeneratedPaths.GeneratedLayer(layerOutputKind);
            var animatorController = context.ReplaceOrCreateOutputAsset(defaultPath);
            var builder = new AnimatorLayerBuilder(context.Environment, parametersSnapshot, animatorController);

            if (context.DefaultAvatarMask)
            {
                builder.BuildStaticLayer("Default Avatar Mask", null, context.DefaultAvatarMask);
            }

            AnimationClip resetClip;
            if (context.HasResetClip)
            {
                resetClip = context.Environment.EnsureAsset(GeneratedPaths.GeneratedResetLayer(layerOutputKind), context.ResetClip);
                context.BuildResetClip(layerKind, resetClip);
                builder.BuildStaticLayer("Reset", resetClip, null);
            }
            else
            {
                resetClip = null;
            }
            context.ResetClip = resetClip;

            builder.BuildEmoteLayers(context.Environment.GetContext<EmoteItemContext>().ForceMirroredEmoteItems(layerKind), layerKind);
            if (layerKind == context.Environment.GenerateTrackingControlLayer)
            {
                builder.BuildTrackingControlLayers(context.Environment.GetContext<EmoteItemContext>().AllMirroredEmoteItems());
            }
            builder.BuildParameters();
            return context.OutputAsset;
        }

        static void BuildResetClip(this AnimatorLayerContextBase context, LayerKind layerKind, AnimationClip targetClip)
        {
            var proxyAnimator = context.Environment.ProvideProxyAnimator();
            var avatar = proxyAnimator != null ? proxyAnimator.gameObject : context.GameObject;
            if (!avatar)
            {
                var gameObject = context.Environment.ContainerTransform.gameObject;
                ErrorReportWrapper.LogWarningFormat(Loc("Warn::ResetClip::WithoutAvatar."), gameObject,
                    new Substitution
                    {
                        ["gameObjectName"] = gameObject.name 
                    });
                return;
            }

            var allClips = Enumerable.Empty<AnimationClip>()
                .Concat(context.Environment.GetContext<EmoteItemContext>().ForceMirroredEmoteItems(layerKind).SelectMany(e => e.AllClipsRec()));

            var curveBindings = CurveBindings.Collect(allClips);
            var boundValues = ResetValuesExtractor.ExtractFromAvatarRoot(curveBindings, avatar);
            boundValues.Build(targetClip);
        }
    }
}