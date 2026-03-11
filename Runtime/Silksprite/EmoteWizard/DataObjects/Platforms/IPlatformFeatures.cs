using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public interface IPlatformFeatures
    {
        string ParameterForAlwaysTrue { get; }
        string GestureLeft { get; }
        string GestureLeftWeight { get; }
        string GestureRight { get; }
        string GestureRightWeight { get; }

        List<ParameterInstance> DefaultParameters();
        bool IsDefaultParameter(string parameter);
    }
}
