using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class MixinInstance
    {
        public readonly RuntimeAnimatorController? SourceController;
        public readonly Motion? SourceClip;
        public readonly string Name;
        public readonly LayerKind LayerKind;
        public readonly int Order;

        public MixinInstance(RuntimeAnimatorController? sourceController, Motion? sourceClip, string name, LayerKind layerKind, int order)
        {
            SourceController = sourceController;
            SourceClip = sourceClip;
            Name = name;
            LayerKind = layerKind;
            Order = order;
        }
    }
}
