using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts
{
    public partial class EmoteWizardEnvironment
    {
        [CanBeNull]
        readonly EmoteWizardRoot _root;

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
        public readonly OverrideGeneratedControllerType2 OverrideGesture;
        public readonly RuntimeAnimatorController OverrideGestureController;
        public readonly OverrideGeneratedControllerType1 OverrideAction;
        public readonly RuntimeAnimatorController OverrideActionController;
        public readonly OverrideControllerType2 OverrideSitting;
        public readonly RuntimeAnimatorController OverrideSittingController;

        public readonly bool ShowTutorial;
        public bool PersistGeneratedAssets { get; set; } = true;

        EmoteWizardEnvironment(EmoteWizardRoot root)
        {
            _root = root;
            _avatarDescriptor = root.avatarDescriptor ? root.avatarDescriptor : root.GetComponentInParent<VRCAvatarDescriptor>();
            _proxyAnimator = root.proxyAnimator;
            
            GenerateTrackingControlLayer = root.generateTrackingControlLayer;
            OverrideGesture = root.overrideGesture;
            OverrideGestureController = root.overrideGestureController;
            OverrideAction = root.overrideAction;
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
            
            OverrideGesture = OverrideGeneratedControllerType2.Generate;
            OverrideAction = OverrideGeneratedControllerType1.Default;
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

        [Obsolete]
        public GameObject GameObject => _root.gameObject;
        [Obsolete]
        public Transform Transform => _root.transform;
        
        public void DisconnectAllOutputAssets()
        {
            ProxyAnimator = null;
            foreach (var context in _contexts) context.DisconnectOutputAssets();
        }

        public string GeneratedAssetPath(string relativePath)
        {
            return _root ? Path.Combine(_root.generatedAssetRoot, relativePath.Replace("@@@Generated@@@", _root.generatedAssetPrefix)) : relativePath;
        }

        public IEnumerable<EmoteItem> CollectAllEmoteItems()
        {
            return GetComponentsInChildren<IEmoteItemSource>().SelectMany(source => source.EmoteItems);
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

        public T FindOrCreateChildComponent<T>(string path, Action<T> initializer = null) where T : Component
        {
            if (_root)
            {
                var child = _root.transform.Find(path);
                if (child && child.EnsureComponent<T>() is T c) return c;
            }
            if (_avatarDescriptor)
            {
                var child = _avatarDescriptor.transform.Find(path);
                if (child && child.EnsureComponent<T>() is T c) return c;
            }
            return ((Component)_root ?? _avatarDescriptor).AddChildComponent(path, initializer);
        }
    }
}