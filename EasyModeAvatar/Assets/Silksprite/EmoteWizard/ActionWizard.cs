using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ActionWizard : EmoteWizardBase
    {
        [SerializeField] public bool fixedTransitionDuration = true;
        [SerializeField] public string actionSelectParameter = "VRCEmote";
        [SerializeField] public bool afkSelectEnabled = false;
        [SerializeField] public string afkSelectParameter = "EmoteWizardAFK";

        [FormerlySerializedAs("actionEmotes")]
        [SerializeField] public List<ActionEmote> legacyActionEmotes;
        [FormerlySerializedAs("afkEmotes")]
        [SerializeField] public List<ActionEmote> legacyAfkEmotes;
        [SerializeField] public ActionEmote defaultAfkEmote;
        [SerializeField] public RuntimeAnimatorController outputAsset;

        public IEnumerable<ActionEmote> CollectActionEmotes() => legacyActionEmotes.Where(item => item.enabled);
        public IEnumerable<ActionEmote> CollectAfkEmotes() => legacyAfkEmotes.Where(item => item.enabled);

        public bool SelectableAfkEmotes => afkSelectEnabled && CollectAfkEmotes().Any();
    }
}