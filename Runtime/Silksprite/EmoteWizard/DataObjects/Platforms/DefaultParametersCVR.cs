using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    // FIXME
    public static class DefaultParametersCVR
    {
        public static List<ParameterInstance> Populate()
        {
            return PlatformFeatures.ChilloutVR.Populate();
        }
    }
}