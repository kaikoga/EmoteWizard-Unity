using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class MultiActionEmoteSource : EmoteWizardDataSourceContainerBase, IActionEmoteSource
    {
        [SerializeField] public List<ActionEmote> actionEmotes = new List<ActionEmote>();

        public IEnumerable<ActionEmote> ActionEmotes => actionEmotes;
    }
}