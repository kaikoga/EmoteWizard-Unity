using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public interface IPlatformFeatures
    {
        string ParameterForAlwaysTrue { get; }
        List<ParameterInstance> DefaultParameters();
        bool IsDefaultParameter(string parameter);
    }
}
