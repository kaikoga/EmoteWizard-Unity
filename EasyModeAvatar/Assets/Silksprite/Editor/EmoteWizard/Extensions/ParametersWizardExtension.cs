using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ParametersWizardExtension
    {
        public static void RepopulateParameters(this ParametersWizard parametersWizard)
        {
            parametersWizard.parameterItems = new List<ParameterItem>();
            parametersWizard.ForceRefreshParameters();
        }

        public static bool IsInvalidParameter(this ParametersWizard parametersWizard, string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName)) return false;
            return parametersWizard != null && parametersWizard.parameterItems.Concat(parametersWizard.defaultParameterItems).All(item => item.name != parameterName);
        }
    }
}