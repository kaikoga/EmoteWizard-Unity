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

        bool IsHandSignParameterReference(string parameterReference);
        string ResolveParameterReference(string parameterReference);
        int HandSignValue(HandSign handSign);
        List<ParameterInstance> DefaultParameters();
        bool IsDefaultParameterReference(string parameterReference);
    }
}
