using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.ChilloutVR.Extensions;
using Silksprite.EmoteWizard.Platforms.ChilloutVR.Internal;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using Silksprite.EmoteWizardSupport.Logger;
using Silksprite.Loch;
using Silksprite.Loch.Tools;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Extensions
{
    public static class BaseControllerContextExtension
    {
        static readonly LayerKind[] LayerOrder =
        {
            LayerKind.Action,
            LayerKind.Gesture,
            LayerKind.FX
        };

        public static RuntimeAnimatorController BuildOutputAsset(this MergedLayerContext context, ParametersSnapshot parametersSnapshot)
        {
            var defaultPath = GeneratedPaths.GeneratedBase;
            var animatorController = context.ReplaceOrCreateOutputAsset(defaultPath);
            var builder = new AnimatorLayerBuilder(context.Environment, parametersSnapshot, animatorController);

            builder.MarkDefaultParameters();

            var avatarAnimator = (AnimatorController)CvrCckAssetLocator.AvatarAnimator();
            builder.AddExternalLayer(avatarAnimator.layers[0]);
            
            if (context.DefaultAvatarMask)
            {
                builder.BuildStaticLayer("Default Avatar Mask", null, context.DefaultAvatarMask);
            }

            AnimationClip resetClip;
            if (context.HasResetClip)
            {
                resetClip = context.Environment.EnsureAsset(GeneratedPaths.GeneratedResetClip, context.ResetClip);
                context.BuildResetClip(resetClip);
                builder.BuildStaticLayer("Reset", resetClip, null);
            }
            else
            {
                resetClip = null;
            }
            context.ResetClip = resetClip;

            foreach (var layerKind in LayerOrder)
            {
                builder.BuildEmoteLayers(context.Environment.GetContext<EmoteItemContext>().ForceMirroredEmoteItems(layerKind), layerKind);
            }
            builder.BuildTrackingControlLayers(context.Environment.GetContext<EmoteItemContext>().AllMirroredEmoteItems());
            builder.BuildParameters();
            return context.OutputAsset;
        }

        static void BuildResetClip(this MergedLayerContext context, AnimationClip targetClip)
        {
            var proxyAnimator = context.Environment.ProvideProxyAnimator();
            var avatar = proxyAnimator != null ? proxyAnimator.gameObject : context.GameObject;
            if (!avatar)
            {
                var gameObject = context.Environment.ContainerTransform.gameObject;
                ErrorReportWrapper.LogWarningFormat(LochTool.Loc("Warn::ResetClip::WithoutAvatar."), gameObject,
                    new Substitution
                    {
                        ["gameObjectName"] = gameObject.name 
                    });
                return;
            }

            var allClips = Enumerable.Empty<AnimationClip>()
                .Concat(context.Environment.GetContext<EmoteItemContext>().AllForceMirroredEmoteItems().SelectMany(e => e.AllClipsRec()));

            var curveBindings = CurveBindings.Collect(allClips);
            var boundValues = ResetValuesExtractor.ExtractFromAvatarRoot(curveBindings, avatar);
            boundValues.Build(targetClip);
        }
    }
}
