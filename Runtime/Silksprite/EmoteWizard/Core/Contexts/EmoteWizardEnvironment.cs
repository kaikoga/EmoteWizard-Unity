using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.Components;
#endif

#if EW_VRM0
using VRM;
#endif

#if EW_VRM1
using UniVRM10;
#endif

namespace Silksprite.EmoteWizard.Contexts
{
    public partial class EmoteWizardEnvironment
    {
        [CanBeNull]
        readonly EmoteWizardRoot _root;

        public EmoteWizardRoot Root => _root;

        public readonly bool IsDetectedAvatarRoot;

        [CanBeNull]
        Transform _avatarRoot;
        public Transform AvatarRoot
        {
            get => _avatarRoot;
            set => _avatarRoot = value;
        }

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
                    if (!_avatarRoot) return DetectedPlatform.Mixed;

                    var detectedPlatform = DetectedPlatform.None;

#if EW_VRCSDK3_AVATARS
                    if (_avatarRoot.GetComponent<VRCAvatarDescriptor>()) detectedPlatform |= DetectedPlatform.VRChat;
#endif
#if EW_VRM0
                    if (_avatarRoot.GetComponent<VRMMeta>()) detectedPlatform |= DetectedPlatform.VRM0;
#endif
#if EW_VRM1
                    if (_avatarRoot.GetComponent<Vrm10Instance>()) detectedPlatform |= DetectedPlatform.VRM1;
#endif

                    switch (detectedPlatform)
                    {
                        case DetectedPlatform.VRChat: return DetectedPlatform.VRChat;
                        case DetectedPlatform.VRM0: return DetectedPlatform.VRM0;
                        case DetectedPlatform.VRM1: return DetectedPlatform.VRM1;
                        default: return DetectedPlatform.Mixed;
                    }
                }
            }
        }

        [CanBeNull]
        Animator _proxyAnimator;
        public Animator ProxyAnimator
        {
            get => _proxyAnimator;
            set
            {
                _proxyAnimator = value;
                if (_root) _root.proxyAnimator = value;
            }
        }

        AnimationClip _emptyClip;
        public AnimationClip EmptyClip
        {
            get => _emptyClip;
            set
            {
                _emptyClip = value;
                if (_root) _root.emptyClip = value;
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
                if (_root) _root.overrideGesture = value;
            }
        }

        public readonly RuntimeAnimatorController OverrideGestureController;

        OverrideGeneratedControllerType1 _overrideAction;
        public OverrideGeneratedControllerType1 OverrideAction
        {
            get => _overrideAction;
            set
            {
                _overrideAction = value;
                if (_root) _root.overrideAction = value;
            }
        }

        public readonly RuntimeAnimatorController OverrideActionController;

        public readonly OverrideControllerType2 OverrideSitting;

        public readonly RuntimeAnimatorController OverrideSittingController;

        public readonly bool ShowTutorial;
        public bool PersistGeneratedAssets { get; set; }

        EmoteWizardEnvironment(EmoteWizardRoot root, Transform avatarRoot, bool isDetectedAvatarRoot)
        {
            _root = root;
            _avatarRoot = avatarRoot;
            _rootOrAvatarRoot = _root;
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
            _avatarRoot = avatarRoot;
            _rootOrAvatarRoot = _avatarRoot;
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
#if EW_VRCSDK3_AVATARS
            if (!avatarRoot && root.avatarDescriptor)
            {
                avatarRoot = root.avatarDescriptor.transform;
            }
#endif
            if (!avatarRoot)
            {
                avatarRoot = RuntimeUtil.FindAvatarInParents(root.transform);
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

        public Transform Find(string path)
        {
            if (_root)
            {
                if (_root.transform.Find(path) is Transform transform) return transform;
            }
            if (_avatarRoot)
            {
                if (_avatarRoot.Find(path) is Transform transform) return transform;
            }
            return null;
        }

        public T GetComponentInChildren<T>(bool includeInactive)
        {
            {
                if (_root && _root.GetComponentInChildren<T>(includeInactive) is T c) return c;
            }
            {
                if (_avatarRoot && _avatarRoot.GetComponentInChildren<T>(includeInactive) is T c) return c;
            }
            return default;
        }

        public T[] GetComponentsInChildren<T>(bool includeInactive)
        {
            return new Component[] { _root, _avatarRoot }
                .Where(component => component)
                .SelectMany(component => component.GetComponentsInChildren<T>(includeInactive))
                .Distinct()
                .ToArray();
        }
    }
}