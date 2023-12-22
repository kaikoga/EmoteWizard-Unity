using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts
{
    public partial class EmoteWizardEnvironment
    {
        [CanBeNull]
        readonly EmoteWizardRoot _root;

        public EmoteWizardRoot Root => _root;

        [CanBeNull]
        VRCAvatarDescriptor _avatarDescriptor;
        public VRCAvatarDescriptor AvatarDescriptor
        {
            get => _avatarDescriptor;
            set
            {
                _avatarDescriptor = value;
                if (_root) _root.avatarDescriptor = value;
            }
        }

        readonly Component _rootOrAvatarDescriptor;

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

        EmoteWizardEnvironment(EmoteWizardRoot root)
        {
            _root = root;
            _avatarDescriptor = root.avatarDescriptor ? root.avatarDescriptor : root.GetComponentInParent<VRCAvatarDescriptor>();
            _rootOrAvatarDescriptor = _root;
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

        EmoteWizardEnvironment(VRCAvatarDescriptor avatarDescriptor)
        {
            _avatarDescriptor = avatarDescriptor;
            _root = avatarDescriptor.GetComponentInChildren<EmoteWizardRoot>();
            _rootOrAvatarDescriptor = _avatarDescriptor;
            
            _overrideGesture = OverrideGeneratedControllerType2.Default1;
            _overrideAction = OverrideGeneratedControllerType1.Default;
            OverrideSitting = OverrideControllerType2.Default2;
        }

        public static EmoteWizardEnvironment FromRoot(EmoteWizardRoot root)
        {
            var env = new EmoteWizardEnvironment(root);
            env.CollectOtherContexts();
            return env;
        }

        public static EmoteWizardEnvironment FromAvatar(VRCAvatarDescriptor avatarDescriptor)
        {
            var env = new EmoteWizardEnvironment(avatarDescriptor);
            env.CollectOtherContexts();
            return env;
        }

        public Transform ContainerTransform => _rootOrAvatarDescriptor.transform;
        
        public void DisconnectAllOutputAssets()
        {
            ProxyAnimator = null;
            foreach (var context in _contexts) context.DisconnectOutputAssets();
        }

        public Transform Find(string path)
        {
            if (_root)
            {
                if (_root.transform.Find(path) is Transform transform) return transform;
            }
            if (_avatarDescriptor)
            {
                if (_avatarDescriptor.transform.Find(path) is Transform transform) return transform;
            }
            return null;
        }

        public T GetComponentInChildren<T>()
        {
            {
                if (_root && _root.GetComponentInChildren<T>() is T c) return c;
            }
            {
                if (_avatarDescriptor && _avatarDescriptor.GetComponentInChildren<T>() is T c) return c;
            }
            return default;
        }

        public T[] GetComponentsInChildren<T>()
        {
            return new Component[] { _root, _avatarDescriptor }
                .Where(component => component)
                .SelectMany(component => component.GetComponentsInChildren<T>())
                .Distinct()
                .ToArray();
        }
    }
}