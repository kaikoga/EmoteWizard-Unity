using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class EditorLayerContextExtension
    {
        public static RuntimeAnimatorController BuildOutputAsset(this EditorLayerContext context, ParametersSnapshot parametersSnapshot)
        {
            var defaultRelativePath = GeneratedPaths.GeneratedEditor;
            var animatorController = context.ReplaceOrCreateOutputAsset(defaultRelativePath);
            var builder = new AnimatorLayerBuilder(context.Environment, LayerKind.None, parametersSnapshot, animatorController);

            builder.BuildEditorLayer(context.CollectEmoteItems());
            builder.BuildParameters();
            return context.OutputAsset;
        }
    }
}