using UnityEditor;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ParametersWizardExtension
    {
        public static bool IsInvalidParameter(this ParametersWizard parametersWizard, string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName)) return false;
            return parametersWizard != null && parametersWizard.Snapshot().IsInvalidParameter(parameterName);
        }

        public static VRCExpressionParameters BuildOutputAsset(this ParametersWizard parametersWizard)
        {
            var expressionParams = parametersWizard.ReplaceOrCreateOutputAsset(ref parametersWizard.outputAsset, "Expressions/@@@Generated@@@ExprParams.asset");

            expressionParams.parameters = parametersWizard.Snapshot().ToParameters();

            AssetDatabase.SaveAssets();
            
            return expressionParams;
        }
    }
}