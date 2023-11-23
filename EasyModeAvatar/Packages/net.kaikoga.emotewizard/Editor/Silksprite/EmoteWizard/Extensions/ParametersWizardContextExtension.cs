using Silksprite.EmoteWizard.Contexts;
using UnityEditor;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ParametersWizardContextExtension
    {
        public static bool IsInvalidParameter(this IParametersWizardContext context, string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName)) return false;
            return context != null && context.Snapshot().IsInvalidParameter(parameterName);
        }

        public static VRCExpressionParameters BuildOutputAsset(this IParametersWizardContext context)
        {
            var outputAsset = context.OutputAsset;
            var expressionParams = context.ReplaceOrCreateOutputAsset(ref outputAsset, "Expressions/@@@Generated@@@ExprParams.asset");
            context.OutputAsset = outputAsset;

            expressionParams.parameters = context.Snapshot().ToParameters();

            AssetDatabase.SaveAssets();
            
            return expressionParams;
        }
    }
}