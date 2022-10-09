using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using Silksprite.EmoteWizard.Sources.Legacy.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl.Base
{
    public abstract class MultiAnimationMixinSourceBase : EmoteWizardDataSourceContainerBase, IAnimationMixinSourceBase
    {
        [SerializeField] public List<AnimationMixin> baseMixins = new List<AnimationMixin>();
        [SerializeField] public List<AnimationMixin> mixins = new List<AnimationMixin>();

        public IEnumerable<AnimationMixin> BaseMixins => baseMixins;
        public IEnumerable<AnimationMixin> Mixins => mixins;

        public abstract string LayerName { get; }
    }
}