using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public abstract class AnimatorLayerContextBase : OutputContextBase<AnimatorLayerWizardBase, RuntimeAnimatorController>
    {
        protected AnimatorLayerContextBase(AnimatorLayerWizardBase wizard) : base(wizard) { }

        public override RuntimeAnimatorController OutputAsset
        {
            get => Wizard.outputAsset;
            set => Wizard.outputAsset = value;
        }

        public override void DisconnectOutputAssets()
        {
            Wizard.resetClip = null;
            Wizard.outputAsset = null;
        }

        public LayerKind LayerKind => Wizard.LayerKind;

        public AvatarMask DefaultAvatarMask => Wizard.defaultAvatarMask;

        public bool HasResetClip => Wizard.HasResetClip;

        public AnimationClip ResetClip
        {
            get => Wizard.resetClip;
            set => Wizard.resetClip = value;
        }

        public IEnumerable<EmoteItem> CollectEmoteItems() => Wizard.Environment.CollectAllEmoteItems().Where(item => item.Sequence.layerKind == Wizard.LayerKind);
    }
}