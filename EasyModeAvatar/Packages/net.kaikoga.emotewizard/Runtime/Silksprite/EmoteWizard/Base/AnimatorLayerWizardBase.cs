using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimatorLayerWizardBase : EmoteWizardBase, IAnimatorLayerWizardContext
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public AnimationClip resetClip;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        public abstract bool HasResetClip { get; }
        public abstract LayerKind LayerKind { get; }

        Component IBehaviourContext.Component => this;
        RuntimeAnimatorController IOutputContext<RuntimeAnimatorController>.OutputAsset
        {
            get => outputAsset;
            set => outputAsset = value;
        }

        AvatarMask IAnimatorLayerWizardContext.DefaultAvatarMask => defaultAvatarMask;

        AnimationClip IAnimatorLayerWizardContext.ResetClip
        {
            get => resetClip;
            set => resetClip = value;
        }

        public override void DisconnectOutputAssets()
        {
            resetClip = null;
            outputAsset = null;
        }

        public IEnumerable<EmoteItem> CollectEmoteItems() => Environment.CollectAllEmoteItems().Where(item => item.Sequence.layerKind == LayerKind);
    }
}