using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ActionWizard : EmoteWizardBase
    {
        [SerializeField] public bool fixedTransitionDuration = true;
        [SerializeField] public bool afkSelectEnabled = false;
        [SerializeField] public string afkSelectParameter = "EmoteWizardAFK";

        [SerializeField] public List<ActionEmote> actionEmotes;
        [SerializeField] public List<ActionEmote> afkEmotes;
        [SerializeField] public RuntimeAnimatorController outputAsset;

        public bool SelectableAfkEmotes => afkSelectEnabled && afkEmotes.Count > 1;
    }
}