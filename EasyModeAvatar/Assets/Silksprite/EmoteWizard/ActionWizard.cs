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
        [SerializeField] public string afkSelectParameter = "EmoteWizard_AFK";

        [SerializeField] public List<ActionEmote> actionEmotes;
        [SerializeField] public List<ActionEmote> afkEmotes;
        [SerializeField] public RuntimeAnimatorController outputAsset;
    }
}