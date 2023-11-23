using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public interface IAnimatorLayerWizardContext : IOutputContext<RuntimeAnimatorController>
    {
        LayerKind LayerKind { get; }
        AvatarMask DefaultAvatarMask { get; }

        bool HasResetClip { get; }
        AnimationClip ResetClip { get; set; }

        IEnumerable<EmoteItem> CollectEmoteItems();
    }
}