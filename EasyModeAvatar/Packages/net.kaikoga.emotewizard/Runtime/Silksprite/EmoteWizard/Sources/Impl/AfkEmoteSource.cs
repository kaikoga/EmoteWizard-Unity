using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class AfkEmoteSource : EmoteWizardDataSourceBase, IAfkEmoteSource
    {
        [SerializeField] public ActionEmote afkEmote;

        public IEnumerable<ActionEmote> AfkEmotes
        {
            get { yield return afkEmote; }
        }
    }
}