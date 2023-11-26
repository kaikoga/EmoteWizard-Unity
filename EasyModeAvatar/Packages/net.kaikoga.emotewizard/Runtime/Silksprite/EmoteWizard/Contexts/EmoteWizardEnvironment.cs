using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts
{
    public class EmoteWizardEnvironment
    {
        [CanBeNull]
        readonly EmoteWizardRoot _root;
        [CanBeNull]
        readonly VRCAvatarDescriptor _avatarDescriptor;
        readonly List<IBehaviourContext> _contexts = new List<IBehaviourContext>();

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
        public readonly bool ShowTutorial;
        public bool PersistGeneratedAssets { get; set; } = true;

        EmoteWizardEnvironment(EmoteWizardRoot root)
        {
            _root = root;
            _avatarDescriptor = GetComponentInChildren<AvatarWizard>()?.avatarDescriptor;
            
            GenerateTrackingControlLayer = root.generateTrackingControlLayer;
            ShowTutorial = root.showTutorial;
        }

        public static EmoteWizardEnvironment FromRoot(EmoteWizardRoot root)
        {
            var env = new EmoteWizardEnvironment(root);
            env.CollectOtherContexts();
            return env;
        }

        void CollectOtherContexts()
        {
            var contexts = GetComponentsInChildren<IContextProvider>().Select(component => component.ToContext(this));
            foreach (var context in contexts)
            {
                if (_contexts.Any(c => c.GetType() == context.GetType() && c.GameObject == context.GameObject)) continue;
                _contexts.Add(context);
            }
        }

        [Obsolete]
        public GameObject GameObject => _root.gameObject;
        [Obsolete]
        public Transform Transform => _root.transform;

        public T GetContext<T>() where T : IBehaviourContext
        {
            var context = _contexts.OfType<T>().FirstOrDefault();
            if (context == null)
            {
                context = (T)Activator.CreateInstance(typeof(T), this);
                _contexts.Add(context);
            }
            return context;
        }

        public void DisconnectAllOutputAssets()
        {
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