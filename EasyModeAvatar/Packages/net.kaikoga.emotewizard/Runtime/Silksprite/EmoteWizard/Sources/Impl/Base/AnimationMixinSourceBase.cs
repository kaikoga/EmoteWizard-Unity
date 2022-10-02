using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl.Base
{
    public abstract class AnimationMixinSourceBase : EmoteWizardDataSourceBase, IAnimationMixinSourceBase
    {
        [SerializeField] public AnimationMixin mixin;
        public bool isBaseMixin;

        public IEnumerable<AnimationMixin> BaseMixins
        {
            get { if (isBaseMixin) yield return mixin; }
        }

        public IEnumerable<AnimationMixin> Mixins
        {
            get { if (!isBaseMixin) yield return mixin; }
        }

        public abstract string LayerName { get; }
    }
}