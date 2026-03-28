using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.Components;
#endif

#if CVR_CCK_EXISTS || ADLIB_CVR_CCK_STUBBED
using Silksprite.AdLib.ChilloutVR.Extensions;
#endif

#if ATIV_DETECTED_VRM0
using VRM;
#endif

#if ATIV_DETECTED_VRM1
using UniVRM10;
#endif

namespace Silksprite.EmoteWizard.Contexts
{
    public partial class EmoteWizardEnvironment
    {
        public EmoteWizardRoot? Root { get; }

        public readonly bool IsDetectedAvatarRoot;

        public Transform AvatarRoot { get; set; }

        readonly Component _rootOrAvatarRoot;

        readonly bool _detectPlatform;
        DetectedPlatform? _detectedPlatform;
        public DetectedPlatform Platform
        {
            get
            {
                return _detectedPlatform ?? DetectPlatform();

                DetectedPlatform DetectPlatform()
                {
                    var detected = DoDetect();
                    _detectedPlatform = detected;
                    return detected;
                }

                DetectedPlatform DoDetect()
                {
                    if (!_detectPlatform) return DetectedPlatform.Mixed;
                    if (AvatarRoot == null) return DetectedPlatform.Mixed;

                    var detectedPlatform = DetectedPlatform.None;

#if EW_VRCSDK3_AVATARS
                    if (AvatarRoot.GetComponent<VRCAvatarDescriptor>()) detectedPlatform |= DetectedPlatform.VRChat;
#endif
#if CVR_CCK_EXISTS || ADLIB_CVR_CCK_STUBBED
                    if (AvatarRoot.TryGetCVRAvatarAccess(out _)) detectedPlatform |= DetectedPlatform.ChilloutVR;
#endif
#if ATIV_DETECTED_VRM0
                    if (AvatarRoot.GetComponent<VRMMeta>()) detectedPlatform |= DetectedPlatform.VRM0;
#endif
#if ATIV_DETECTED_VRM1
                    if (AvatarRoot.GetComponent<Vrm10Instance>()) detectedPlatform |= DetectedPlatform.VRM1;
#endif

                    switch (detectedPlatform)
                    {
                        case DetectedPlatform.VRChat: return DetectedPlatform.VRChat;
                        case DetectedPlatform.ChilloutVR: return DetectedPlatform.ChilloutVR;
                        case DetectedPlatform.VRM0: return DetectedPlatform.VRM0;
                        case DetectedPlatform.VRM1: return DetectedPlatform.VRM1;
                        default: return DetectedPlatform.Mixed;
                    }
                }
            }
        }

        Animator? _proxyAnimator;
        public Animator? ProxyAnimator
        {
            get => _proxyAnimator;
            set
            {
                _proxyAnimator = value;
                if (Root != null) Root.proxyAnimator = value;
            }
        }

        AnimationClip? _emptyClip;
        public AnimationClip? EmptyClip
        {
            get => _emptyClip;
            set
            {
                _emptyClip = value;
                if (Root != null) Root.emptyClip = value;
            }
        }

        public readonly LayerKind GenerateTrackingControlLayer = LayerKind.FX;

        OverrideGeneratedControllerType2 _overrideGesture;
        public OverrideGeneratedControllerType2 OverrideGesture
        {
            get => _overrideGesture;
            set
            {
                _overrideGesture = value;
                if (Root != null) Root.overrideGesture = value;
            }
        }

        public readonly RuntimeAnimatorController? OverrideGestureController;

        OverrideGeneratedControllerType1 _overrideAction;
        public OverrideGeneratedControllerType1 OverrideAction
        {
            get => _overrideAction;
            set
            {
                _overrideAction = value;
                if (Root != null) Root.overrideAction = value;
            }
        }

        public readonly RuntimeAnimatorController? OverrideActionController;

        public readonly OverrideControllerType2 OverrideSitting;

        public readonly RuntimeAnimatorController? OverrideSittingController;

        public readonly bool ShowTutorial;
        public bool PersistGeneratedAssets { get; set; }

        EmoteWizardEnvironment(EmoteWizardRoot root, Transform avatarRoot, bool isDetectedAvatarRoot)
        {
            Root = root;
            AvatarRoot = avatarRoot;
            _rootOrAvatarRoot = Root;
            IsDetectedAvatarRoot = isDetectedAvatarRoot;
            _detectPlatform = root.detectPlatform;
            _proxyAnimator = root.proxyAnimator;
            
            GenerateTrackingControlLayer = root.generateTrackingControlLayer;
            _overrideGesture = root.overrideGesture;
            OverrideGestureController = root.overrideGestureController;
            _overrideAction = root.overrideAction;
            OverrideActionController = root.overrideActionController;
            OverrideSitting = root.overrideSitting;
            OverrideSittingController = root.overrideSittingController;

            ShowTutorial = root.showTutorial;
            PersistGeneratedAssets = root.persistGeneratedAssets;
        }

        EmoteWizardEnvironment(Transform avatarRoot)
        {
            AvatarRoot = avatarRoot;
            _rootOrAvatarRoot = AvatarRoot;
            IsDetectedAvatarRoot = false;
            _detectPlatform = true;
            
            _overrideGesture = OverrideGeneratedControllerType2.Default1;
            _overrideAction = OverrideGeneratedControllerType1.Default;
            OverrideSitting = OverrideControllerType2.Default2;
        }

        public static EmoteWizardEnvironment FromRoot(EmoteWizardRoot root)
        {
            var isDetectedAvatarRoot = false;
            var avatarRoot = root.avatarRootTransform;
            if (avatarRoot == null)
            {
                avatarRoot = RuntimeUtil.FindAvatarInParents(root.transform);
                isDetectedAvatarRoot = true;
            }
            if (avatarRoot == null)
            {
                avatarRoot = root.transform;
                isDetectedAvatarRoot = true;
            }
            var env = new EmoteWizardEnvironment(root, avatarRoot, isDetectedAvatarRoot);
            return env;
        }

        public static EmoteWizardEnvironment FromAvatar(Transform avatarRoot)
        {
            var root = avatarRoot.GetComponentInChildren<EmoteWizardRoot>(true);
            var env = root ? new EmoteWizardEnvironment(root, avatarRoot, false) : new EmoteWizardEnvironment(avatarRoot);
            return env;
        }

        public Transform ContainerTransform => _rootOrAvatarRoot.transform;

        public void DisconnectAllOutputAssets()
        {
            ProxyAnimator = null;
            foreach (var context in ContextsCache.OfType<IBehaviourContext>()) context.DisconnectOutputAssets();
        }

        public T? GetComponentInChildren<T>(bool includeInactive)
        where T : Component
        {
            {
                if (Root != null && Root.GetComponentInChildren<T>(includeInactive) is { } c) return c;
            }
            {
                if (AvatarRoot && AvatarRoot.GetComponentInChildren<T>(includeInactive) is { } c) return c;
            }
            return null;
        }

        public T[] GetComponentsInChildren<T>(bool includeInactive)
        {
            return new Component[] { Root!, AvatarRoot }
                .Where(component => component)
                .SelectMany(component => component.GetComponentsInChildren<T>(includeInactive))
                .Distinct()
                .ToArray();
        }
    }
}