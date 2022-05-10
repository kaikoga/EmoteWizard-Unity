using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    public abstract class ParameterEmoteSourceBase : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<ParameterEmote> parameterEmotes;
        
        public abstract string LayerName { get; }
    }
}