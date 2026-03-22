using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public interface IContext
    {
        EmoteWizardEnvironment Environment { get; }
    }

    public interface IBehaviourContext : IContext
    {
        GameObject? GameObject { get; }

        void DisconnectOutputAssets();
    }

    public interface IOutputContext<T> : IBehaviourContext
    where T : Object
    {
        T? OutputAsset { get; set; }
    }
}