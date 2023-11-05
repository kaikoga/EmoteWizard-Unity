using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public interface IEmoteWizardContext
    {
        GameObject GameObject { get; }
        AnimationClip EmptyClip { get; set; }

        T GetWizard<T>() where T : EmoteWizardBase;

        void DisconnectAllOutputAssets();

        string GeneratedAssetPath(string relativePath);

        IEnumerable<EmoteItem> CollectAllEmoteItems();
        
        T GetComponentInChildren<T>();
    }
}