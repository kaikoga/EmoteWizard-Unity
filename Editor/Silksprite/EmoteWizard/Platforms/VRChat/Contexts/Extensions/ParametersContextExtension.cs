using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.Platforms.VRChat.Extensions;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Extensions
{
    public static class ParametersContextExtension
    {
        public static VRCExpressionParameters BuildOutputAsset(this ParametersContext context)
        {
            var expressionParams = context.ReplaceOrCreateOutputAsset(GeneratedPaths.GeneratedExprParams);

            expressionParams.parameters = context.Snapshot().ToParameters(context.Environment.GetPlatformFeatures());

            AssetDatabase.SaveAssets();
            
            return expressionParams;
        }
    }
}