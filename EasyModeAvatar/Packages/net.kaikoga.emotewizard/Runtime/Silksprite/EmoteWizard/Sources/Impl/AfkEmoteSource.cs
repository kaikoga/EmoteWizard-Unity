using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class AfkEmoteSource : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<ActionEmote> afkEmotes = new List<ActionEmote>(); 
    }
}