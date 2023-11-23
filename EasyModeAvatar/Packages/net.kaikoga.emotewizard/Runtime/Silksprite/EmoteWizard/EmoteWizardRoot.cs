using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizardSupport.Extensions;
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

        public IEmoteWizardEnvironment ToEnv() => EnvImpl.FromRoot(this);
        public IEmoteWizardEnvironment ToEnv(IBehaviourContext context) => EnvImpl.FromContext(this, context);

        class EnvImpl : IEmoteWizardEnvironment
        {
            readonly EmoteWizardRoot _root;
            readonly List<IBehaviourContext> _contexts = new List<IBehaviourContext>();

            EnvImpl(EmoteWizardRoot root) => _root = root;

            public static EnvImpl FromRoot(EmoteWizardRoot root)
            {
                var env = new EnvImpl(root);
                env.CollectOtherContexts();
                return env;
            }

            public static EnvImpl FromContext(EmoteWizardRoot root, IBehaviourContext context)
            {
                var env = new EnvImpl(root);
                env._contexts.Add(context);
                env.CollectOtherContexts();
                return env;
            }

            void CollectOtherContexts()
            {
                var contexts = _root.GetComponentsInChildren<IContextProvider>().Select(component => component.ToContext());
                foreach (var context in contexts)
                {
                    if (_contexts.Any(c => c.GetType() == context.GetType() && c.GameObject == context.GameObject)) continue;
                    _contexts.Add(context);
                }
            }

            GameObject IEmoteWizardEnvironment.GameObject => _root.gameObject;
            Transform IEmoteWizardEnvironment.Transform => _root.transform;
            AnimationClip IEmoteWizardEnvironment.EmptyClip
            {
                get => _root.emptyClip;
                set => _root.emptyClip = value;
            }
            LayerKind IEmoteWizardEnvironment.GenerateTrackingControlLayer => _root.generateTrackingControlLayer;
            bool IEmoteWizardEnvironment.ShowTutorial => _root.showTutorial;
            bool IEmoteWizardEnvironment.PersistGeneratedAssets { get; set; } = true;

            T IEmoteWizardEnvironment.GetContext<T>() => _contexts.OfType<T>().FirstOrDefault();

            void IEmoteWizardEnvironment.DisconnectAllOutputAssets()
            {
                foreach (var context in _contexts) context.DisconnectOutputAssets();
            }

            string IEmoteWizardEnvironment.GeneratedAssetPath(string relativePath)
            {
                return Path.Combine(_root.generatedAssetRoot, relativePath.Replace("@@@Generated@@@", _root.generatedAssetPrefix));
            }

            IEnumerable<EmoteItem> IEmoteWizardEnvironment.CollectAllEmoteItems()
            {
                return _root.GetComponentsInChildren<IEmoteItemSource>().SelectMany(source => source.EmoteItems);
            }

            T IEmoteWizardEnvironment.GetComponentInChildren<T>() => _root.GetComponentInChildren<T>();
            T[] IEmoteWizardEnvironment.GetComponentsInChildren<T>() => _root.GetComponentsInChildren<T>();
            T IEmoteWizardEnvironment.FindOrCreateChildComponent<T>(string path, Action<T> initializer) => _root.FindOrCreateChildComponent(path, initializer);
        }
    }
}