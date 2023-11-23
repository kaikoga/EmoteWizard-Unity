using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public class EmoteWizardEnvironment
    {
        readonly EmoteWizardRoot _root;
        readonly List<IBehaviourContext> _contexts = new List<IBehaviourContext>();

        EmoteWizardEnvironment(EmoteWizardRoot root) => _root = root;

        public static EmoteWizardEnvironment FromRoot(EmoteWizardRoot root)
        {
            var env = new EmoteWizardEnvironment(root);
            env.CollectOtherContexts();
            return env;
        }

        void CollectOtherContexts()
        {
            var contexts = _root.GetComponentsInChildren<IContextProvider>().Select(component => component.ToContext(this));
            foreach (var context in contexts)
            {
                if (_contexts.Any(c => c.GetType() == context.GetType() && c.GameObject == context.GameObject)) continue;
                _contexts.Add(context);
            }
        }

        public GameObject GameObject => _root.gameObject;
        public Transform Transform => _root.transform;

        public AnimationClip EmptyClip
        {
            get => _root.emptyClip;
            set => _root.emptyClip = value;
        }

        public LayerKind GenerateTrackingControlLayer => _root.generateTrackingControlLayer;
        public bool ShowTutorial => _root.showTutorial;
        public bool PersistGeneratedAssets { get; set; } = true;

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
            return Path.Combine(_root.generatedAssetRoot, relativePath.Replace("@@@Generated@@@", _root.generatedAssetPrefix));
        }

        public IEnumerable<EmoteItem> CollectAllEmoteItems()
        {
            return _root.GetComponentsInChildren<IEmoteItemSource>().SelectMany(source => source.EmoteItems);
        }

        public T GetComponentInChildren<T>() => _root.GetComponentInChildren<T>();
        public T[] GetComponentsInChildren<T>() => _root.GetComponentsInChildren<T>();
        public T FindOrCreateChildComponent<T>(string path, Action<T> initializer = null) where T : Component => _root.FindOrCreateChildComponent(path, initializer);
    }
}