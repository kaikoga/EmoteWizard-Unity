using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using Silksprite.EmoteWizard.Sources.Legacy.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl.Base
{
    public abstract class MultiParameterEmoteSourceBase : EmoteWizardDataSourceContainerBase, IParameterEmoteSourceBase
    {
        [SerializeField] public List<ParameterEmote> parameterEmotes = new List<ParameterEmote>();
        
        public IEnumerable<ParameterEmote> ParameterEmotes => parameterEmotes;

        public void GenerateSingleParameter(ParameterItem parameterItem)
        {
            var parameter = new ParameterEmote
            {
                enabled = true,
                name = parameterItem.name,
                valueKind = parameterItem.ValueKind,
                parameter = parameterItem.name,
                emoteKind = ParameterEmoteKind.Transition
            };
            parameter.CollectStates(parameterItem);
            parameterEmotes.Add(parameter);
        }
    }
}