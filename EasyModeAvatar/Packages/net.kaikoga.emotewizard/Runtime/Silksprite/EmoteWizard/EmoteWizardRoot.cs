using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class EmoteWizardRoot : EmoteWizardBehaviour
    {
        [SerializeField] public VRCAvatarDescriptor avatarDescriptor;
        [SerializeField] public Animator proxyAnimator;

        [SerializeField] public bool persistGeneratedAssets = true;

        [SerializeField] [HideInInspector] public string generatedAssetRoot = "Assets/Generated/";
        [SerializeField] [HideInInspector] public string generatedAssetPrefix = "Generated";
        
        [SerializeField] public AnimationClip emptyClip;
        [SerializeField] public LayerKind generateTrackingControlLayer = LayerKind.FX;
        [SerializeField] public EmoteWizardEnvironment.OverrideGeneratedControllerType2 overrideGesture = EmoteWizardEnvironment.OverrideGeneratedControllerType2.Generate;
        [SerializeField] public RuntimeAnimatorController overrideGestureController;
        [SerializeField] public EmoteWizardEnvironment.OverrideGeneratedControllerType1 overrideAction = EmoteWizardEnvironment.OverrideGeneratedControllerType1.Default;
        [SerializeField] public RuntimeAnimatorController overrideActionController;
        [SerializeField] public EmoteWizardEnvironment.OverrideControllerType2 overrideSitting = EmoteWizardEnvironment.OverrideControllerType2.Default2;
        [SerializeField] public RuntimeAnimatorController overrideSittingController;


        [SerializeField] public bool showTutorial;

        public EmoteWizardEnvironment ToEnv() => EmoteWizardEnvironment.FromRoot(this);
    }
}