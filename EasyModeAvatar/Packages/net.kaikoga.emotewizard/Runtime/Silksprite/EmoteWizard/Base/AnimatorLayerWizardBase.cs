using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimatorLayerWizardBase : EmoteWizardBase
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public AnimationClip resetClip;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        public abstract bool HasResetClip { get; }
        public abstract LayerKind LayerKind { get; }

        public override void DisconnectOutputAssets()
        {
            resetClip = null;
            outputAsset = null;
        }

        public IEnumerable<EmoteItem> CollectEmoteItems() => Context.CollectAllEmoteItems().Where(item => item.Sequence.layerKind == LayerKind);
    }
}