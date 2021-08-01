using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ActionWizard : EmoteWizardBase
    {
        [SerializeField] public List<ActionEmote> actionEmotes;
        [SerializeField] public RuntimeAnimatorController outputAsset;
    }
}