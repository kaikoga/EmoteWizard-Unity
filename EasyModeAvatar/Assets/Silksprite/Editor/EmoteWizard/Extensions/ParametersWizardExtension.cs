using System.Linq;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ParametersWizardExtension
    {
        public static bool IsInvalidParameter(this ParametersWizard parametersWizard, string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName)) return false;
            return parametersWizard != null && parametersWizard.parameterItems.Concat(parametersWizard.defaultParameterItems).All(item => item.name != parameterName);
        }
    }
}