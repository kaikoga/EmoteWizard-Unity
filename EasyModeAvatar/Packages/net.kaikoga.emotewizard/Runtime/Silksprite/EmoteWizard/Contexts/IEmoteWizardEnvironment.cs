using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public interface IEmoteWizardEnvironment
    {
        GameObject GameObject { get; }
        Transform Transform { get; }
        AnimationClip EmptyClip { get; set; }
        LayerKind GenerateTrackingControlLayer { get; }
        bool ShowTutorial { get; }
        bool PersistGeneratedAssets { get; set;  }

        T GetWizard<T>() where T : EmoteWizardBase;

        void DisconnectAllOutputAssets();

        string GeneratedAssetPath(string relativePath);

        IEnumerable<EmoteItem> CollectAllEmoteItems();
        
        T GetComponentInChildren<T>();
        T[] GetComponentsInChildren<T>();
        T FindOrCreateChildComponent<T>(string path = null, Action<T> initializer = null) where T : Component;
    }
}