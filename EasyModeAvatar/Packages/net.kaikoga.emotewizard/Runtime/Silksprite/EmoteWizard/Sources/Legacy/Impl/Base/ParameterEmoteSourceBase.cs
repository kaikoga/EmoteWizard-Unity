using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using Silksprite.EmoteWizard.Sources.Legacy.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl.Base
{
    public abstract class ParameterEmoteSourceBase : EmoteWizardDataSourceBase, IParameterEmoteSourceBase
    {
        [SerializeField] public ParameterEmote parameterEmote;
        
        public IEnumerable<ParameterEmote> ParameterEmotes
        {
            get { yield return parameterEmote; }
        }

        public abstract string LayerName { get; }
    }
}