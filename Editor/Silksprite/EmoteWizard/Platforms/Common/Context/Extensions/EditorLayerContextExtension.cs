using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Common.Internal;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Common.Context.Extensions
{
    public static class EditorLayerContextExtension
    {
        public static RuntimeAnimatorController BuildOutputAsset(this EditorLayerContext context, ParametersSnapshot parametersSnapshot)
        {
            var defaultRelativePath = GeneratedPaths.GeneratedEditor;
            var animatorController = context.ReplaceOrCreateOutputAsset(defaultRelativePath);
            var builder = new AnimatorLayerBuilder(context.Environment, EditorChilloutVRFeatures.Instance, parametersSnapshot, animatorController);

            builder.BuildEditorLayer(context.Environment.GetContext<EmoteItemContext>().AllMirroredEmoteItems());
            builder.BuildParameters();
            return context.OutputAsset;
        }
    }
}