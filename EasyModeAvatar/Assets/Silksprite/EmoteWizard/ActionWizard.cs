using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
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

        public bool HasLegacyData => legacyActionEmotes.Any() || legacyAfkEmotes.Any();

        public IEnumerable<ActionEmote> CollectActionEmotes()
        {
            return GetComponentsInChildren<ActionEmoteSource>().SelectMany(source => source.actionEmotes)
                .Where(item => item.enabled);
        }

        public IEnumerable<ActionEmote> CollectAfkEmotes()
        {
            return GetComponentsInChildren<AfkEmoteSource>().SelectMany(source => source.afkEmotes)
                .Where(item => item.enabled);
        }

        public bool SelectableAfkEmotes => afkSelectEnabled && CollectAfkEmotes().Any();
    }
}