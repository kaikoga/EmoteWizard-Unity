using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
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