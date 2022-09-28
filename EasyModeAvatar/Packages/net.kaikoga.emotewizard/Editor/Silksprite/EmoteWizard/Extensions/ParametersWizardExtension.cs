using System.Linq;
using UnityEditor;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ParametersWizardExtension
    {
        public static bool IsInvalidParameter(this ParametersWizard parametersWizard, string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName)) return false;
            return parametersWizard != null && parametersWizard.AllParameterItems.All(item => item.name != parameterName);
        }

        public static VRCExpressionParameters BuildOutputAsset(this ParametersWizard parametersWizard)
        {
            var expressionParams = parametersWizard.ReplaceOrCreateOutputAsset(ref parametersWizard.outputAsset, "Expressions/@@@Generated@@@ExprParams.asset");

            expressionParams.parameters = parametersWizard.ToParameters();

            AssetDatabase.SaveAssets();
            
            return expressionParams;
        }
    }
}