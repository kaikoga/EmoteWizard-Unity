using Silksprite.EmoteWizard.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureLayerWizard : AnimatorLayerWizardBase
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}