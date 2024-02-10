using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.Components;
#endif

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Emote Wizard Root", -200)]
    public class EmoteWizardRoot : EmoteWizardBehaviour
    {
        [SerializeField] public Transform avatarRootTransform;
#if EW_VRCSDK3_AVATARS
        [SerializeField] public VRCAvatarDescriptor avatarDescriptor;
#endif

        [SerializeField] public Animator proxyAnimator;

        [SerializeField] public bool persistGeneratedAssets;

        [SerializeField] [HideInInspector] public string generatedAssetRoot = "Assets/Generated/";
        [SerializeField] [HideInInspector] public string generatedAssetPrefix = "Generated";
        
        [SerializeField] public AnimationClip emptyClip;
        [SerializeField] public LayerKind generateTrackingControlLayer = LayerKind.FX;
        [SerializeField] public OverrideGeneratedControllerType2 overrideGesture = OverrideGeneratedControllerType2.Default1;
        [SerializeField] public RuntimeAnimatorController overrideGestureController;
        [SerializeField] public OverrideGeneratedControllerType1 overrideAction = OverrideGeneratedControllerType1.Default;
        [SerializeField] public RuntimeAnimatorController overrideActionController;
        [SerializeField] public OverrideControllerType2 overrideSitting = OverrideControllerType2.Default2;
        [SerializeField] public RuntimeAnimatorController overrideSittingController;


        [SerializeField] public bool showTutorial;

        public EmoteWizardEnvironment ToEnv() => EmoteWizardEnvironment.FromRoot(this);
    }
}