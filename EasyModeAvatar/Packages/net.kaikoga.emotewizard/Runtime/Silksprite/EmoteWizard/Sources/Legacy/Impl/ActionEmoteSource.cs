using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class ActionEmoteSource : EmoteWizardDataSourceBase, IActionEmoteSource
    {
        [SerializeField] public ActionEmote actionEmote;

        public IEnumerable<ActionEmote> ActionEmotes
        {
            get { yield return actionEmote; }
        }
    }
}