using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public static class DefaultParameters
    {
        public static List<ParameterInstance> Populate()
        {
            return PlatformFeatures.VRChat.Populate();
        }

        public static bool IsDefaultParameter(string parameter)
        {
            return PlatformFeatures.VRChat.IsDefaultParameter(parameter);
        }
    }
}