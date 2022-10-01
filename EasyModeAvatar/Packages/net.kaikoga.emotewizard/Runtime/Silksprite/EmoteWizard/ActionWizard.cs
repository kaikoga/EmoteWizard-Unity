using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ActionWizard : EmoteWizardBase
    {
        [SerializeField] public bool fixedTransitionDuration = true;
        [SerializeField] public string actionSelectParameter = EmoteWizardConstants.Defaults.Params.ActionSelect;
        [SerializeField] public bool afkSelectEnabled = false;
        [SerializeField] public string afkSelectParameter = EmoteWizardConstants.Defaults.Params.AfkSelect;

        [SerializeField] public ActionEmote defaultAfkEmote;
        [SerializeField] public RuntimeAnimatorController outputAsset;

        public override void DisconnectOutputAssets()
        {
            outputAsset = null;
        }

        public IEnumerable<ActionEmote> CollectActionEmotes()
        {
            return EmoteWizardRoot.GetComponentsInChildren<IActionEmoteSource>().SelectMany(source => source.ActionEmotes)
                .Where(item => item.enabled);
        }

        public IEnumerable<ActionEmote> CollectAfkEmotes()
        {
            return EmoteWizardRoot.GetComponentsInChildren<IAfkEmoteSource>().SelectMany(source => source.AfkEmotes)
                .Where(item => item.enabled);
        }

        public bool SelectableAfkEmotes => afkSelectEnabled && CollectAfkEmotes().Any();
    }
}