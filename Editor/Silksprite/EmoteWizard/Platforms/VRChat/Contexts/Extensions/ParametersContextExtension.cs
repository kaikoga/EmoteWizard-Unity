using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class ParametersContextExtension
    {
        public static VRCExpressionParameters BuildOutputAsset(this ParametersContext context)
        {
            var expressionParams = context.ReplaceOrCreateOutputAsset(GeneratedPaths.GeneratedExprParams);

            expressionParams.parameters = context.Snapshot().ToParameters();

            AssetDatabase.SaveAssets();
            
            return expressionParams;
        }
    }
}