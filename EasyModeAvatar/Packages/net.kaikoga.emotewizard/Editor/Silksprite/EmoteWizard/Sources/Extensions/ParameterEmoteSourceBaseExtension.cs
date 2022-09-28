using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;

namespace Silksprite.EmoteWizard.Sources.Extensions
{
    public static class ParameterEmoteSourceBaseExtension
    {
        public static void RepopulateParameterEmotes(this ParameterEmoteSourceBase parameterEmoteSourceBase, ParametersWizard parametersWizard)
        {
            parameterEmoteSourceBase.parameterEmotes = new List<ParameterEmote>();
            parameterEmoteSourceBase.GenerateParameters(parametersWizard);
        }
    }
}