using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public interface IBehaviourContext
    {
        IEmoteWizardContext Context { get; }
        Component Component { get; }
    }

    public interface IOutputContext<T> : IBehaviourContext
    {
        T OutputAsset { get; set; }
    }
}