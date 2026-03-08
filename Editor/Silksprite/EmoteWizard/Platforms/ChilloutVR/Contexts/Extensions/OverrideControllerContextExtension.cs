using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Extensions
{
    public static class OverrideControllerContextExtension
    {
        public static AnimatorOverrideController BuildOutputAsset(this OverrideControllerContext context, RuntimeAnimatorController baseController)
        {
            var defaultRelativePath = GeneratedPaths.GeneratedOverride;
            var overrideController = context.ReplaceOrCreateOutputAsset(defaultRelativePath);
            overrideController.runtimeAnimatorController = baseController;
            return context.OutputAsset;
        }
    }
}