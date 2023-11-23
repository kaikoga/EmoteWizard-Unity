using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public interface IBehaviourContext
    {
        IEmoteWizardEnvironment Environment { get; }
        Component Component { get; }

        void DisconnectOutputAssets();
    }

    public interface IOutputContext<T> : IBehaviourContext
    {
        T OutputAsset { get; set; }
    }
}