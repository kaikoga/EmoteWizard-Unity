using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class AnimationClipMixin
    {
        public Motion? clip;
        public LayerKind layerKind;
        public int order;

        public MixinInstance ToInstance() => new MixinInstance(null, clip, layerKind, order);
    }
}
