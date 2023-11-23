using Silksprite.EmoteWizard.DataObjects.Internal;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public interface IParametersWizardContext : IOutputContext<VRCExpressionParameters>
    {
        ParametersSnapshot Snapshot();
    }
}