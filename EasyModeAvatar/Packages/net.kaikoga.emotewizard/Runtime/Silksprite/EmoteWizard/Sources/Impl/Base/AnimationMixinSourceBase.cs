using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl.Base
{
    public abstract class AnimationMixinSourceBase : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<AnimationMixin> baseMixins = new List<AnimationMixin>();
        [SerializeField] public List<AnimationMixin> mixins = new List<AnimationMixin>();

        public abstract string LayerName { get; }
    }
}