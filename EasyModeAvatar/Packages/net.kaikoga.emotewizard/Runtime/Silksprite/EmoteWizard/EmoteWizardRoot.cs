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

        public T GetWizard<T>() where T : EmoteWizardBase => GetComponentInChildren<T>();

        public void DisconnectAllOutputAssets()
        {
            foreach (var wizard in GetComponentsInChildren<EmoteWizardBase>()) wizard.DisconnectOutputAssets();
        }

        public string GeneratedAssetPath(string relativePath) => Path.Combine(generatedAssetRoot, relativePath.Replace("@@@Generated@@@", generatedAssetPrefix));

        public IEnumerable<EmoteItem> CollectAllEmoteItems() => GetComponentsInChildren<IEmoteItemSource>().SelectMany(source => source.EmoteItems);

        public IEmoteWizardEnvironment ToEnv() => new EnvImpl(this);

        class EnvImpl : IEmoteWizardEnvironment
        {
            readonly EmoteWizardRoot _root;

            public EnvImpl(EmoteWizardRoot root) => _root = root;

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

            T IEmoteWizardEnvironment.GetWizard<T>() => _root.GetWizard<T>();
            void IEmoteWizardEnvironment.DisconnectAllOutputAssets() => _root.DisconnectAllOutputAssets();
            string IEmoteWizardEnvironment.GeneratedAssetPath(string relativePath) => _root.GeneratedAssetPath(relativePath);
            IEnumerable<EmoteItem> IEmoteWizardEnvironment.CollectAllEmoteItems() => _root.CollectAllEmoteItems();
            T IEmoteWizardEnvironment.GetComponentInChildren<T>() => _root.GetComponentInChildren<T>();
            T[] IEmoteWizardEnvironment.GetComponentsInChildren<T>() => _root.GetComponentsInChildren<T>();
            T IEmoteWizardEnvironment.FindOrCreateChildComponent<T>(string path, Action<T> initializer) => _root.FindOrCreateChildComponent(path, initializer);
        }
    }
}