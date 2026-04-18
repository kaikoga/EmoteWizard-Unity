using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class AnimatorControllerMixin
    {
        public RuntimeAnimatorController? animatorController;
        public LayerKind layerKind;
        public int order;

        public MixinInstance ToInstance() => new MixinInstance(animatorController, null, animatorController?.name ?? "", layerKind, order);
    }
}
