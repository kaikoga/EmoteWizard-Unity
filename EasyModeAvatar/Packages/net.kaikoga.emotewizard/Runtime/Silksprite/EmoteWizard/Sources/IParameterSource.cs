using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Sources
{
    public interface IParameterSource
    {
        IEnumerable<ParameterItem> ParameterItems { get; }
    }
}