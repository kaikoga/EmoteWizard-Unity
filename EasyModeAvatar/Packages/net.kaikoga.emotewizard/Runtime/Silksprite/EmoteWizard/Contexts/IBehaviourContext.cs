using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public interface IBehaviourContext
    {
        IEmoteWizardEnvironment Environment { get; }
        GameObject GameObject { get; }

        void DisconnectOutputAssets();
    }

    public interface IOutputContext<T> : IBehaviourContext
    {
        T OutputAsset { get; set; }
    }
}