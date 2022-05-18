using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    public abstract class AnimationMixinSourceBase : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<AnimationMixin> baseMixins;
        [SerializeField] public List<AnimationMixin> mixins;

        public abstract string LayerName { get; }
    }
}