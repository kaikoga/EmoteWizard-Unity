using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class MultiAfkEmoteSource : EmoteWizardDataSourceContainerBase, IAfkEmoteSource
    {
        [SerializeField] public List<ActionEmote> afkEmotes = new List<ActionEmote>();

        public IEnumerable<ActionEmote> AfkEmotes => afkEmotes;
    }
}