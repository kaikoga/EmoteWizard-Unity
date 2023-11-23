using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class EmoteWizardRoot : MonoBehaviour
    {
        [SerializeField] [HideInInspector] public string generatedAssetRoot = "Assets/Generated/";
        [SerializeField] [HideInInspector] public string generatedAssetPrefix = "Generated";
        
        [SerializeField] public AnimationClip emptyClip;
        [SerializeField] public LayerKind generateTrackingControlLayer = LayerKind.FX;

        [SerializeField] public bool showTutorial;

        public EmoteWizardEnvironment ToEnv() => EmoteWizardEnvironment.FromRoot(this);
        public EmoteWizardEnvironment ToEnv(IBehaviourContext context) => EmoteWizardEnvironment.FromContext(this, context);
    }
}