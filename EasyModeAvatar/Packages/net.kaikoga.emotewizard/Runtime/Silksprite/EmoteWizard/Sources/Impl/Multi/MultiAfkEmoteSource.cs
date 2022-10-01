using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl.Multi
{
    public class MultiAfkEmoteSource : EmoteWizardDataSourceBase, IAfkEmoteSource
    {
        [SerializeField] public List<ActionEmote> afkEmotes = new List<ActionEmote>();

        public IEnumerable<ActionEmote> AfkEmotes => afkEmotes;
    }
}