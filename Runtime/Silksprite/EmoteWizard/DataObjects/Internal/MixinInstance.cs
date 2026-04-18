using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class MixinInstance
    {
        public readonly RuntimeAnimatorController? SourceController;
        public readonly Motion? SourceClip;
        public readonly LayerKind LayerKind;
        public readonly int Order;

        public MixinInstance(RuntimeAnimatorController? sourceController, Motion? sourceClip, LayerKind layerKind, int order)
        {
            SourceController = sourceController;
            SourceClip = sourceClip;
            LayerKind = layerKind;
            Order = order;
        }
    }
}
