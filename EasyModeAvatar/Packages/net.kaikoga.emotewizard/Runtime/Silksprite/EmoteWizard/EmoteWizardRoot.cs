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
    public class EmoteWizardRoot : MonoBehaviour, IEmoteWizardContext
    {
        [SerializeField] [HideInInspector] public string generatedAssetRoot = "Assets/Generated/";
        [SerializeField] [HideInInspector] public string generatedAssetPrefix = "Generated";
        
        [SerializeField] public AnimationClip emptyClip;
        [SerializeField] public LayerKind generateTrackingControlLayer = LayerKind.FX;

        [SerializeField] public bool showTutorial;

        AnimationClip IEmoteWizardContext.EmptyClip
        {
            get => emptyClip;
            set => emptyClip = value;
        }
        LayerKind IEmoteWizardContext.GenerateTrackingControlLayer => generateTrackingControlLayer;
        bool IEmoteWizardContext.ShowTutorial => showTutorial;
        bool IEmoteWizardContext.PersistGeneratedAssets { get; set; } = true;


        GameObject IEmoteWizardContext.GameObject => gameObject;
        Transform IEmoteWizardContext.Transform => transform;

        T IEmoteWizardContext.FindOrCreateChildComponent<T>(string path, Action<T> initializer) => this.FindOrCreateChildComponent(path, initializer);


        public T GetWizard<T>() where T : EmoteWizardBase => GetComponentInChildren<T>();

        public void DisconnectAllOutputAssets()
        {
            foreach (var wizard in GetComponentsInChildren<EmoteWizardBase>()) wizard.DisconnectOutputAssets();
        }

        public string GeneratedAssetPath(string relativePath) => Path.Combine(generatedAssetRoot, relativePath.Replace("@@@Generated@@@", generatedAssetPrefix));

        public IEnumerable<EmoteItem> CollectAllEmoteItems() => GetComponentsInChildren<IEmoteItemSource>().SelectMany(source => source.EmoteItems);
    }
}