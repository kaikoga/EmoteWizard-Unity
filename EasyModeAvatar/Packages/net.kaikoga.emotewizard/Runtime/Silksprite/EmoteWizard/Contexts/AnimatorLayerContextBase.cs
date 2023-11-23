using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public abstract class AnimatorLayerContextBase : OutputContextBase<AnimatorLayerWizardBase, RuntimeAnimatorController>
    {
        RuntimeAnimatorController _outputAsset;
        public override RuntimeAnimatorController OutputAsset
        {
            get => _outputAsset;
            set
            {
                _outputAsset = value;
                if (Wizard) Wizard.outputAsset = value;
            }
        }

        AnimationClip _resetClip;
        public AnimationClip ResetClip
        {
            get => _resetClip;
            set
            {
                _resetClip = value;
                if (Wizard) Wizard.resetClip = value;
            }
        }

        public readonly AvatarMask DefaultAvatarMask;
        public readonly LayerKind LayerKind;
        public readonly bool HasResetClip;

        protected AnimatorLayerContextBase(AnimatorLayerWizardBase wizard) : base(wizard)
        {
            _outputAsset = Wizard.outputAsset;
            _resetClip = Wizard.resetClip;

            DefaultAvatarMask = Wizard.defaultAvatarMask;
            LayerKind = Wizard.LayerKind;
            HasResetClip = Wizard.HasResetClip;
        }

        public override void DisconnectOutputAssets()
        {
            OutputAsset = null;
            ResetClip = null;
        }

        public IEnumerable<EmoteItem> CollectEmoteItems() => Environment.CollectAllEmoteItems().Where(item => item.Sequence.layerKind == Wizard.LayerKind);
    }
}