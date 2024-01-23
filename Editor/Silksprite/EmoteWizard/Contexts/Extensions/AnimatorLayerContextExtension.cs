using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using Silksprite.EmoteWizardSupport.Logger;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

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

            builder.BuildEmoteLayers(context.Environment.GetContext<EmoteItemContext>().ForceMirroredEmoteItems(context.LayerKind));
            if (layerKind == context.Environment.GenerateTrackingControlLayer)
            {
                builder.BuildTrackingControlLayers(context.Environment.GetContext<EmoteItemContext>().AllMirroredEmoteItems());
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
                ErrorReportWrapper.LogWarningFormat(Loc("Warn::ResetClip::WithoutAvatar."), gameObject,
                    new Dictionary<string, string>
                    {
                        ["gameObjectName"] = gameObject.name 
                    });
                return;
            }

            var allClips = Enumerable.Empty<AnimationClip>()
                .Concat(context.Environment.GetContext<EmoteItemContext>().ForceMirroredEmoteItems(context.LayerKind).SelectMany(e => e.AllClipsRec()));

            var curveBindings = CurveBindings.Collect(allClips);
            var boundValues = ResetValuesExtractor.ExtractFromAvatarRoot(curveBindings, avatar);
            boundValues.Build(targetClip);
        }
    }
}