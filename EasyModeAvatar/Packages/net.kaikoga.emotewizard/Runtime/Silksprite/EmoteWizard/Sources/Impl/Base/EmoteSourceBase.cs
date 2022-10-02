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
        [SerializeField] public Emote emote;
        [SerializeField] public bool advancedAnimations;

        public IEnumerable<Emote> Emotes
        {
            get { yield return emote; }
        }

        public abstract string LayerName { get; }

        public bool HasComplexAnimations => emote.clipLeft != null && emote.clipRight != null && emote.clipLeft != emote.clipRight;

        public bool AdvancedAnimations
        {
            get => advancedAnimations;
            set
            {
                advancedAnimations = value;
                if (value)
                {
                    emote.clipRight = emote.clipLeft;
                }
                else
                {
                    emote.clipLeft = emote.clipLeft ? emote.clipLeft : emote.clipRight;
                    emote.clipRight = null;
                }
            }
        }
    }
}