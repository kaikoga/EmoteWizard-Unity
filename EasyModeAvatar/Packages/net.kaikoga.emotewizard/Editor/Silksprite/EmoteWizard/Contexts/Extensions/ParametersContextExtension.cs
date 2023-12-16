using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class ParametersContextExtension
    {
        public static bool IsInvalidParameter(this ParametersContext context, string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName)) return false;
            return context != null && context.Snapshot().IsInvalidParameter(parameterName);
        }

        public static VRCExpressionParameters BuildOutputAsset(this ParametersContext context)
        {
            var expressionParams = context.ReplaceOrCreateOutputAsset(new GeneratedPath("Expressions/@@@Generated@@@ExprParams.asset"));

            expressionParams.parameters = context.Snapshot().ToParameters();

            AssetDatabase.SaveAssets();
            
            return expressionParams;
        }
    }
}