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
        [SerializeField] public List<ActionEmote> legacyActionEmotes = new List<ActionEmote>();
        [FormerlySerializedAs("afkEmotes")]
        [SerializeField] public List<ActionEmote> legacyAfkEmotes = new List<ActionEmote>();
        [SerializeField] public ActionEmote defaultAfkEmote;
        [SerializeField] public RuntimeAnimatorController outputAsset;

        public override void DisconnectOutputAssets()
        {
            outputAsset = null;
        }

        public bool HasLegacyData => legacyActionEmotes?.Any() ?? legacyAfkEmotes?.Any() ?? false;

        public IEnumerable<ActionEmote> CollectActionEmotes()
        {
            return EmoteWizardRoot.GetComponentsInChildren<ActionEmoteSource>().SelectMany(source => source.actionEmotes)
                .Where(item => item.enabled);
        }

        public IEnumerable<ActionEmote> CollectAfkEmotes()
        {
            return EmoteWizardRoot.GetComponentsInChildren<AfkEmoteSource>().SelectMany(source => source.afkEmotes)
                .Where(item => item.enabled);
        }

        public bool SelectableAfkEmotes => afkSelectEnabled && CollectAfkEmotes().Any();
    }
}