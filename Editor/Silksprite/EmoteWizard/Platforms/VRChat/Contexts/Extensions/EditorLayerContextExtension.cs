using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.VRChat.Internal;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Extensions
{
    public static class EditorLayerContextExtension
    {
        public static RuntimeAnimatorController BuildOutputAsset(this EditorLayerContext context, ParametersSnapshot parametersSnapshot)
        {
            var defaultRelativePath = GeneratedPaths.GeneratedEditor;
            var animatorController = context.ReplaceOrCreateOutputAsset(defaultRelativePath);
            var builder = new AnimatorLayerBuilder(context.Environment, parametersSnapshot, animatorController);

            builder.BuildEditorLayer(context.Environment.GetContext<EmoteItemContext>().AllMirroredEmoteItems());
            builder.BuildParameters();
            return context.OutputAsset;
        }
    }
}