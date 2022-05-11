using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;

namespace Silksprite.EmoteWizard.Sources.Extensions
{
    public static class ParameterEmoteSourceBaseExtension
    {
        public static void RepopulateParameterEmotes(this ParameterEmoteSourceBase parameterEmoteSourceBase, ParametersWizard parametersWizard)
        {
            parametersWizard.TryRefreshParameters();
            parameterEmoteSourceBase.parameterEmotes = new List<ParameterEmote>();
            var animationWizardBase = parameterEmoteSourceBase.LayerName == "FX" ? (AnimationWizardBase)parametersWizard.EmoteWizardRoot.GetWizard<FxWizard>() : parametersWizard.EmoteWizardRoot.GetWizard<GestureWizard>();
            parameterEmoteSourceBase.GenerateParameters(parametersWizard, animationWizardBase);
        }
    }
}