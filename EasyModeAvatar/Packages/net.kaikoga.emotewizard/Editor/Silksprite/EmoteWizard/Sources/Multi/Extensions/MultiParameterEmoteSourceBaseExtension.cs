using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;

namespace Silksprite.EmoteWizard.Sources.Multi.Extensions
{
    public static class MultiParameterEmoteSourceBaseExtension
    {
        public static void RepopulateParameterEmotes(this MultiParameterEmoteSourceBase multiParameterEmoteSourceBase, ParametersWizard parametersWizard)
        {
            multiParameterEmoteSourceBase.parameterEmotes = new List<ParameterEmote>();
            multiParameterEmoteSourceBase.GenerateParameters(parametersWizard);
        }
    }
}