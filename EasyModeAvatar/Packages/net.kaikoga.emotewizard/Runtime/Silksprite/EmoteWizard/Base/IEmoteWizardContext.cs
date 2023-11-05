using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public interface IEmoteWizardContext
    {
        GameObject GameObject { get; }
        Transform Transform { get; }
        AnimationClip EmptyClip { get; set; }
        LayerKind GenerateTrackingControlLayer { get; }
        bool ShowTutorial { get; }

        T GetWizard<T>() where T : EmoteWizardBase;

        void DisconnectAllOutputAssets();

        string GeneratedAssetPath(string relativePath);

        IEnumerable<EmoteItem> CollectAllEmoteItems();
        
        T GetComponentInChildren<T>();
        T[] GetComponentsInChildren<T>();
        T FindOrCreateChildComponent<T>(string path = null, Action<T> initializer = null) where T : Component;
    }
}