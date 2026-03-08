using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public interface IPlatformFeatures
    {

        List<ParameterInstance> Populate();
        bool IsDefaultParameter(string parameter);
    }
}
