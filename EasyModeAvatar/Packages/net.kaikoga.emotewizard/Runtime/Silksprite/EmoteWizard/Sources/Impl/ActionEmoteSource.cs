using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class ActionEmoteSource : EmoteWizardDataSourceBase, IActionEmoteSource
    {
        [SerializeField] public List<ActionEmote> actionEmotes = new List<ActionEmote>();

        public IEnumerable<ActionEmote> ActionEmotes => actionEmotes;
    }
}