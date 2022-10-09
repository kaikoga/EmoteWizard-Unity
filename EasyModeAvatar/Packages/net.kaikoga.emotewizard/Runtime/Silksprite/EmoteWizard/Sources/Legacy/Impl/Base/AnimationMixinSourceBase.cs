using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using Silksprite.EmoteWizard.Sources.Legacy.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl.Base
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