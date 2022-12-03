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

        public abstract LayerKind LayerKind { get; }

        public override void DisconnectOutputAssets()
        {
            resetClip = null;
            outputAsset = null;
        }

        public IEnumerable<EmoteItem> CollectAllEmoteItems() => EmoteWizardRoot.GetComponentsInChildren<IEmoteItemSource>().SelectMany(source => source.EmoteItems);
        public IEnumerable<EmoteItem> CollectEmoteItems() => CollectAllEmoteItems().Where(item => item.sequence.layerKind == LayerKind);
    }
}