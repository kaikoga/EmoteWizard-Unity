using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl.Base
{
    public abstract class EmoteSourceBase : EmoteWizardDataSourceBase, IEmoteSourceBase
    {
        [SerializeField] public List<Emote> emotes = new List<Emote>();
        [SerializeField] public bool advancedAnimations;

        public IEnumerable<Emote> Emotes => emotes;

        public abstract string LayerName { get; }

        public bool HasComplexAnimations => emotes.Any(emote => emote.clipLeft != null && emote.clipRight != null && emote.clipLeft != emote.clipRight);

        public bool AdvancedAnimations
        {
            get => advancedAnimations;
            set
            {
                advancedAnimations = value;
                if (value)
                {
                    foreach (var emote in emotes)
                    {
                        emote.clipRight = emote.clipLeft;
                    }
                }
                else
                {
                    foreach (var emote in emotes)
                    {
                        emote.clipLeft = emote.clipLeft ? emote.clipLeft : emote.clipRight;
                        emote.clipRight = null;
                    }
                }
            }
        }
    }
}