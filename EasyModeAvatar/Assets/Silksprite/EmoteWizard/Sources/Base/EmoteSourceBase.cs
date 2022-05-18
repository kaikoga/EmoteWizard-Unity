using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    public abstract class EmoteSourceBase : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<Emote> emotes;
        
        public abstract string LayerName { get; }
    }
}